using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Woby.Core.CommonLanguage.Signaling.Core;
using Woby.Core.Network.Core;

namespace Woby.Core.CommonLanguage.Signaling.Routings;

public class Proxy : SignalingHeader
{
    public Proxy(
        string key, 
        string body, 
        IImmutableDictionary<string, string>? additinalMetadata = null) : base(key, body, HeaderType.Routing, additinalMetadata)
    {
    }

    public required string Host { get; init; }
    public int? Port { get; init; }
    public NetworkMetadata? Metadata { get; init; }
    public string? Protocol { get; init; }
    public string? DeclaredTransport { get; init; }

    public bool HasPort() => Port.HasValue;

    [MemberNotNullWhen(true, nameof(Metadata))]
    public bool HasNetworkMetadata() => Metadata is not null;

}
