using System.Collections.Immutable;
using Woby.Core.CommonLanguage.Signaling.ContentMeta;
using Woby.Core.CommonLanguage.Signaling.Identities;
using Woby.Core.CommonLanguage.Signaling.Roles;
using Woby.Core.CommonLanguage.Signaling.Routings;

namespace Woby.Core.CommonLanguage.Messages
{
    public class SignalingSection
    {
        public required DialogId DialogId { get; init; }
        public required NegotiationId NegitiationId { get; init; }
        public required RoleType Role { get; init; }
        public required SequenceHeader Sequence { get; init; }
        public required Route To { get; init; }
        public required Route From { get; init; }
        public required IImmutableList<Proxy> Proxies { get; init; }
        public MaxForwardings? MaxForwardings { get; init; } // TODO: should be added to docs
        public ContentType? ContentType { get; init; }
        public ContentLength? ContentLength { get; init; }

    }
}
