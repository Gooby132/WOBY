using FluentResults;
using System.Collections.Immutable;
using System.Net;
using Woby.Core.CommonLanguage.Signaling.Core;

namespace Woby.Core.CommonLanguage.Signaling.Routings
{
    /// <summary>
    /// Class representing a route from / to / between (proxy) 
    /// thus storing the direction intended:
    ///     * from - recepient that sent message
    ///     * to - recepient that meant to receive the message
    ///     * proxy - additional header representing where the message came from
    /// </summary>
    public class Route : SignalingHeader
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
            string? displayName = null,
            IImmutableDictionary<string, string>? additinalMetadata = null) : base(key, body, HeaderType.Routing, additinalMetadata)
        {
            Uri = uri;
            Role = role;
            DisplayName = displayName;
            Protocol = protocol;
        }

        public bool HasDisplayName() => !string.IsNullOrEmpty(DisplayName);
        // no equaty required as body and key are compared
    }
}
