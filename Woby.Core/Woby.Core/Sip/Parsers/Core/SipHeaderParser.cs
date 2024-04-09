using FluentResults;
using Microsoft.Extensions.Logging;
using System.Reflection.PortableExecutable;
using System.Text;
using Woby.Core.Core.Headers;
using Woby.Core.Sip.Parsers.Headers;
using Woby.Core.Sip.Parsers.RouteHeaderParser;
using Woby.Core.Sip.Parsers.Utils;

namespace Woby.Core.Sip.Parsers.Core
{
    /*
     * https://datatracker.ietf.org/doc/html/rfc2822
     * 
     * RFC rules for header parsing:
     * 
     *     Header fields are lines composed of a field name, followed by a colon
     *     (":"), followed by a field body, and terminated by CRLF.  A field
     *     name MUST be composed of printable US-ASCII characters (i.e.,
     *     characters that have values between 33 and 126, inclusive), except
     *     colon.  A field body may be composed of any US-ASCII characters,
     *     except for CR and LF.  However, a field body may contain CRLF when
     *     used in header "folding" and  "unfolding" as described in section
     *     2.2.3. 
     * 
     *     Each header field is logically a single line of characters comprising
     *     the field name, the colon, and the field body.  
     *     
     *     For convenience however, and to deal with the 998/78 character limitations per line,
     *     the field body portion of a header field can be split into a multiple
     *     line representation; this is called "folding".
     * 
     * 
     * unfolding:
     *     The process of moving from this folded multiple-line representation
     *     of a header field to its single line representation is called
     *     "unfolding". Unfolding is accomplished by simply removing any CRLF
     *     that is immediately followed by WSP.  Each header field should be
     *     treated in its unfolded form for further syntactic and semantic
     *     evaluation.
     */
    public class SipHeaderParser
    {

        #region Fields

        private readonly ILogger<SipHeaderParser> _logger;
        private readonly SipRouteHeaderParser _sipRouteHeaderParser;

        #endregion

        #region Properties

        public string Name { get; set; } = nameof(SipHeaderParser);

        #endregion

        public SipHeaderParser(ILogger<SipHeaderParser> logger, SipRouteHeaderParser sipRouteHeaderParser) 
        {
            _logger = logger;
            _sipRouteHeaderParser = sipRouteHeaderParser;
        }

        public Result<IEnumerable<HeaderBase>> Parse(string headerInUtf)
        {

            StringReader reader = new StringReader(headerInUtf.Trim());

            var parsedHeadersAsStrings = ParseFoldingHeaders(reader);

            reader.Dispose();

            if (parsedHeadersAsStrings.IsFailed)
            {
                _logger.LogWarning("{this} failed to parse headers section to folding headers. error(s) - '{errors}'", 
                    this, string.Join(", ", parsedHeadersAsStrings.Reasons.Select(r => r.Message)));

                return Result.Fail(parsedHeadersAsStrings.Errors);
            }

            var headersAsKeyValue = ParseHeadersStringsAsKeyValue(parsedHeadersAsStrings.Value);

            if(headersAsKeyValue.IsFailed)
            {
                _logger.LogWarning("{this} failed to parse headers section to key value headers. error(s) - '{errors}'",
                    this, string.Join(", ", parsedHeadersAsStrings.Reasons.Select(r => r.Message)));

                return Result.Fail(headersAsKeyValue.Errors);
            }

            var sipHeaders = ParseHeaderParametersAsSipHeaders(headersAsKeyValue.Value);

            if (sipHeaders.IsFailed)
            {
                _logger.LogWarning("{this} failed to parse headers to sip headers. error(s) - '{errors}'",
                    this, string.Join(", ", sipHeaders.Reasons.Select(r => r.Message)));

                return Result.Fail(headersAsKeyValue.Errors);
            }

            _logger.LogTrace("{this} objs - '{val}'", this, string.Join(Environment.NewLine, headersAsKeyValue.Value.Select(kv => $"{kv.Item1} - value {kv.Item2}")));

            List<HeaderBase> parsedHeaders = new List<HeaderBase>();

            foreach (var header in sipHeaders.Value)
            {
                if (_sipRouteHeaderParser.TryParse(header, out var parsed))
                    parsedHeaders.Add(parsed);
                else parsedHeaders.Add(new UnknownHeader(header.Key, header.Body));
            }

            return parsedHeaders;
        }

        private Result<IEnumerable<SipHeader>> ParseHeaderParametersAsSipHeaders(IEnumerable<(string, string)> keyValueHeaders)
        {
            var sipHeaders = new List<SipHeader>();

            foreach (var keyValueHeader in keyValueHeaders)
            {
                if (!HeaderFieldsUtils.TryParseHeaderParameters(keyValueHeader.Item2, out var headerValue, out var parameters))
                {
                    _logger.LogWarning("{this} failed parsing header parameters for header name - '{name}' with value - {value}",
                        this, keyValueHeader.Item1, keyValueHeader.Item2);

                    continue;
                }

                sipHeaders.Add(new (keyValueHeader.Item1, headerValue, HeaderTypes.Unknown, parameters));
            }

            return sipHeaders;
        }
        private Result<IEnumerable<(string, string)>> ParseHeadersStringsAsKeyValue(IEnumerable<string> headersAsStrings)
        {
            List<(string, string)> keyValueHeaders = new();

            foreach (var header in headersAsStrings) 
            { 
                var sections = header.Split(":", 2, StringSplitOptions.None);

                if(sections.Length != 2)
                    return Result.Fail(SipCoreErrors.GeneralInvalidMessageError("Header does not contains a ':' seperator"));

                keyValueHeaders.Add(new(sections[0].Trim(), sections[1].Trim()));
            }

            return keyValueHeaders;
        }

        private Result<IEnumerable<string>> ParseFoldingHeaders(StringReader reader)
        {
            List<string> headers = new();
            StringBuilder headerBuilder = new StringBuilder();

            int character;
            while ((character = reader.Read()) != -1)
            {

                if(!char.IsAscii((char)character))
                    return Result.Fail(SipCoreErrors.GeneralInvalidMessageError("Message contains a non ascii character"));

                headerBuilder.Append((char)character);

                if (character == '\n')
                {
                    // if not whitespace its the end of the header (see folding headers)
                    if (!char.IsWhiteSpace((char)reader.Peek()))
                    {
                        headers.Add(headerBuilder.ToString());
                        headerBuilder = new StringBuilder();
                        continue;
                    }
                }
            }

            return Result.Ok<IEnumerable<string>>(headers);
        }


    }
}
