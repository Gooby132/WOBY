using FluentResults;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Immutable;
using System.Net;
using Woby.Core.CommonLanguage.Signaling.Core;
using Woby.Core.Network.Core;

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
        public RouteRole Role { get; }
        public NetworkProtocol NetworkProtocol { get; }
        public string? DisplayName { get; }
        public string Protocol { get; }
        public string? User { get; }
        public string? Password { get; }
        public string Host { get; }
        public int? Port { get; }
        public string? Tag { get; }

        public Route(
            RouteRole role,
            string key,
            string body,
            string? displayName,
            string protocol,
            string? user,
            string? password,
            string host,
            int? port,
            NetworkProtocol? networkProtocol,
            IImmutableDictionary<string, string>? additinalMetadata = null) : base(key, body, HeaderType.Routing, additinalMetadata)
        {
            Role = role;
            DisplayName = displayName;
            Protocol = protocol;
            User = user;
            Password = password;
            Host = host;
            Port = port;
            NetworkProtocol = networkProtocol ?? NetworkProtocol.Unknown;
            Tag = additinalMetadata?.GetValueOrDefault("tag");
        }

        public bool HasDisplayName() => !string.IsNullOrEmpty(DisplayName);

        public bool HasUser() => !string.IsNullOrEmpty(User);
        public bool HasPassword() => !string.IsNullOrEmpty(Password);
        public bool ContainsUserInfo() => !string.IsNullOrEmpty(User) || !string.IsNullOrEmpty(Password);
        public bool HasPort() => Port.HasValue;

    }
}
