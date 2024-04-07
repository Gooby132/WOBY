using FluentResults;
using Microsoft.Extensions.Logging;
using System.Reflection.PortableExecutable;
using System.Text;
using Woby.Core.Core.Headers;
using Woby.Core.Sip.Parsers.RouteHeaderParser;

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
            StringReader reader = new StringReader(headerInUtf);
            List<Tuple<string,string>> headers = new();

            int character = -1;
            string key = string.Empty;

            // while till end of message
            while ((character = reader.Read()) != -1)
            {

                if (!char.IsAscii((char)character)) // TODO : remove this should be seperated by /r/n/r/n
                {
                    // invalid sip message - character is not ascii

                    return Result.Fail(SipCoreErrors.GeneralInvalidMessageError("Message contains a non ascii character"));
                }

                string body = string.Empty;

                StringBuilder builder = new StringBuilder();
                builder.Append((char)character);

                // while till Key ; Value parse
                while ((character = reader.Read()) != -1)
                {

                    if (!char.IsAscii((char)character))
                    {
                        // invalid sip message - character is not ascii

                        return Result.Fail(SipCoreErrors.GeneralInvalidMessageError("Message contains a non ascii character"));
                    }

                    if (character == ':')
                    {
                        key = builder.ToString();
                        break; // key found
                    }

                    // parse header body

                    if (character != '\r')
                    {
                        builder.Append((char)character);
                        continue;
                    }

                    // start of carrier return statement
                    character = reader.Read();

                    if (!char.IsAscii((char)character))
                    {
                        // invalid sip message - character is not ascii

                        return Result.Fail(SipCoreErrors.GeneralInvalidMessageError("Message contains a non ascii character"));
                    }

                    if (character != '\n')
                    {
                        builder.Append((char)character);
                        continue;
                    }

                    builder.Append((char)character); // character appended is '\n'

                    // folding message can continue to be read till crlf with no whitespace after
                    if (char.IsWhiteSpace((char)reader.Peek()))
                        continue;

                    _logger.LogTrace("{this} header parsed key - '{header}' value - '{value}'", this, key, body);

                    // header did finish with folding mechanisem adding full header to list
                    headers.Add(new(key, builder.ToString()));

                    break;
                }

            }

            reader.Dispose();

            _logger.LogTrace("{this} objs - '{val}'", this, string.Join(Environment.NewLine, headers.Select(kv => $"{kv.Item1} - value {kv.Item2}")));

            List<HeaderBase> parsedHeaders = new List<HeaderBase>();

            foreach (var header in headers)
            {
                if (_sipRouteHeaderParser.TryParse(header.Item1, header.Item2, out var parsed))
                    parsedHeaders.Add(parsed);
                else parsedHeaders.Add(new UnknownHeader(header.Item1, header.Item2));
            }

            return parsedHeaders;
        }

    }
}
