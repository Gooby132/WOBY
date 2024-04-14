using FluentResults;
using System.Net;
using Woby.Core.Core.Headers.Core;

namespace Woby.Core.Core.Headers.Routings
{
    /// <summary>
    /// Class representing a route from / to / between (proxy) 
    /// thus storing the direction intended:
    ///     * from - recepient that sent message
    ///     * to - recepient that meant to receive the message
    ///     * proxy - additional header representing where the message came from
    /// </summary>
    public class Route : HeaderBase
    {
        public Uri Uri { get; }
        public RouteRole Role { get; }
        public string? DisplayName { get; }
        public string? Protocol { get; }

        internal Route(
            Uri uri,
            RouteRole role, 
            string key, 
            string body,
            string? protocol = null, 
            string? displayName = null) : base(key, body, HeaderType.Routing)
        {
            Uri = uri;
            Role = role;
            DisplayName = displayName;
            Protocol = protocol;
        }

        // no equaty required as body and key are compared
    }
}
