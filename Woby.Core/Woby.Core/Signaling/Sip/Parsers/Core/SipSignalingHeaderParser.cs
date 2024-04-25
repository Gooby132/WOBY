using FluentResults;
using Microsoft.Extensions.Logging;
using System.Text;
using Woby.Core.CommonLanguage.Signaling.Core;
using Woby.Core.Signaling.Sip.Headers;
using Woby.Core.Signaling.Sip.Parsers.Utils;

namespace Woby.Core.Signaling.Sip.Parsers.Core
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
    public class SipSignalingHeaderParser
    {

        #region Fields

        private readonly ILogger<SipSignalingHeaderParser> _logger;

        #endregion

        #region Properties

        public string Name { get; set; } = nameof(SipSignalingHeaderParser);

        #endregion

        public SipSignalingHeaderParser(ILogger<SipSignalingHeaderParser> logger)
        {
            _logger = logger;
        }

        public Result<IEnumerable<Result<SipHeader>>> Parse(string headers)
        {

            StringReader reader = new StringReader(headers.Trim());

            var parsedHeadersAsStrings = ParseFoldingHeaders(reader);

            reader.Dispose();

            if (parsedHeadersAsStrings.IsFailed)
            {
                _logger.LogWarning("{this} failed to parse headers section to folding headers. error(s) - '{errors}'",
                    this, string.Join(", ", parsedHeadersAsStrings.Reasons.Select(r => r.Message)));

                return Result.Fail(parsedHeadersAsStrings.Errors);
            }

            var sipHeaders = parsedHeadersAsStrings
                .Value
                .Select(ParseSingleHeader);

            var errors = sipHeaders
                .Where(header => header.IsFailed)
                .SelectMany(header => header.Errors);

            return Result
                .Ok(sipHeaders)
                .WithErrors(errors); // TODO : add a validation of headers that deems to be neccesery 
        }

        public Result<SipHeader> ParseSingleHeader(string header)
        {
            (string, string) keyVal;

            var sections = header.Split(":", 2, StringSplitOptions.None);

            if (sections.Length != 2)
                return Result.Fail(SipCoreErrors.GeneralInvalidMessageError("Header does not contains a ':' seperator"));

            keyVal = new(sections[0].Trim(), sections[1].Trim());

            if (!HeaderFieldsUtils.TryParseHeaderParameters(keyVal.Item2, out var headerValue, out var parameters))
            {
                _logger.LogWarning("{this} failed parsing header parameters for header name - '{name}' with value - {value}",
                    this, keyVal.Item1, keyVal.Item2);

                return Result.Fail(SipCoreErrors.FailedParsingHeaderParameters());
            }

            return new SipHeader(keyVal.Item1, headerValue, HeaderType.Unknown, parameters);
        }

        private Result<IEnumerable<string>> ParseFoldingHeaders(StringReader reader)
        {
            List<string> headers = new();
            StringBuilder headerBuilder = new StringBuilder();

            int character;
            while ((character = reader.Read()) != -1)
            {

                if (!char.IsAscii((char)character))
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
