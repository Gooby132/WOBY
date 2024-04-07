using FluentResults;
using System.Diagnostics.CodeAnalysis;
using Woby.Core.Core.Headers;
using Woby.Core.Sip.Messages;

namespace Woby.Core.Sip.Parsers.RouteHeaderParser
{
    public class SipRouteHeaderParser
    {

        public static readonly char[] Whitespaces = [' ', '\t', '\n', '\r'];

        public bool TryParse(string key, string body, [NotNullWhen(true)] out Route? route)
        {
            HeaderBase? temp = null;
            var type = SipHeaderTypes.GetType(key);
            var headerParts = body.Trim().Split(Whitespaces, StringSplitOptions.RemoveEmptyEntries);

            type.When(SipHeaderTypes.From).Then(() =>
            {
                string username;
                username = headerParts[0];
                // Address Specification ; https://datatracker.ietf.org/doc/html/rfc2822#section-3.4
                // using angle-addr 
                string addrSpec = headerParts[1].Substring(1, headerParts[1].Length - 1);
                temp = new Route(new Uri(addrSpec), RouteIntake.From, key, body);
            })
            .When(SipHeaderTypes.To).Then(() =>
            {
                string username;
                username = headerParts[0];
                // Address Specification ; https://datatracker.ietf.org/doc/html/rfc2822#section-3.4
                // using angle-addr 
                string addrSpec = headerParts[1].Substring(1, headerParts[1].Length - 1);
                temp = new Route(new Uri(addrSpec), RouteIntake.To, key, body);
            })
            .When(SipHeaderTypes.Via).Then(() =>
             {
                 string username;
                 username = headerParts[0];
                 // Address Specification ; https://datatracker.ietf.org/doc/html/rfc2822#section-3.4
                 // using angle-addr 
                 string addrSpec = headerParts[1].Substring(1, headerParts[1].Length - 1);
                 temp = new Route(new Uri(addrSpec), RouteIntake.Proxy, key, body);
             });

            route = temp as Route;

            return route is not null;
        }

    }
}
