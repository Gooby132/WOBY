using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using Woby.Core.Core.Headers;
using Woby.Core.Sip.Headers;
using Woby.Core.Sip.Messages;
using static Woby.Core.Utils.Rfc.SyntaxHelper;

namespace Woby.Core.Sip.Parsers.SpecializedHeaderParsers
{

    /// <summary>
    /// Sip Route Header is specified for use for the following headers ; 
    /// - When the header field value contains a display name , the URI including all URI parameters is enclosed in "<" and ">"
    /// - If no "<" and ">" are present, all parameters after the URI are header parameters, not URI parameters.
    /// - The display name can be tokens, or a quoted string, if a larger character set is desired.
    /// - Even if the "display-name" is empty, the "name-addr" form MUST be used if the "addr-spec" contains a comma, semicolon, or question mark.
    /// - There may or may not be LWS between the display-name and the "<".
    /// 
    /// These rules for parsing a display name, URI and URI parameters, and header parameters also apply for the header fields To and From.
    /// 
    /// </summary>
    public class SipRouteHeaderParser
    {

        public static readonly char[] Whitespaces = [' ', '\t', '\n', '\r'];
        private readonly ILogger<SipRouteHeaderParser> _logger;

        public SipRouteHeaderParser(ILogger<SipRouteHeaderParser> logger)
        {
            _logger = logger;
        }

        public bool TryParse(SipHeader header, [NotNullWhen(true)] out Route? route)
        {
            HeaderBase? temp = null;
            var type = SipHeaderTypes.GetType(header.Key);

            type.When(SipHeaderTypes.From).Then(() =>
            {
                // Address Specification ; https://datatracker.ietf.org/doc/html/rfc2822#section-3.4
                // using name-addr

                if (!AddressSpecifications.TryParseNameAddr(header.Body, out var displayName, out var address))
                    return;

                temp = new Route(new Uri(address), RouteIntake.From, displayName, header.Key, header.Body);
            })
            .When(SipHeaderTypes.To).Then(() =>
            {
                // Address Specification ; https://datatracker.ietf.org/doc/html/rfc2822#section-3.4
                // using name-addr

                if (!AddressSpecifications.TryParseNameAddr(header.Body, out var displayName, out var address))
                    return;

                temp = new Route(new Uri(address), RouteIntake.To, displayName, header.Key, header.Body);
            })
            .When(SipHeaderTypes.Via).Then(() =>
             {
                 // Address Specification ; https://datatracker.ietf.org/doc/html/rfc2822#section-3.4
                 // using name-addr

                 if (!AddressSpecifications.TryParseNameAddr(header.Body, out var displayName, out var address))
                     return;

                 temp = new Route(new Uri(address), RouteIntake.Proxy, displayName, header.Key, header.Body);
             });

            route = temp as Route;

            return route is not null;
        }

    }
}
