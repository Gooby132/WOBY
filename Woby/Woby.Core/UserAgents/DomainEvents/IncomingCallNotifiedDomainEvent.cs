using Woby.Core.CommonLanguage.Messages;
using Woby.Core.CommonLanguage.Signaling.Identities;
using Woby.Core.Signaling.UserAgents.ValueObjects;
using Woby.Core.Signaling.Primitives;

namespace Woby.Core.Signaling.UserAgents.DomainEvents
{
    public class IncomingCallNotifiedDomainEvent : IDomainEvent
    {
        public required DialogId DialogId { get; init; }
        public required UserAgentId UserAgentId { get; init; }
        public required MessageBase Response { get; init; }
    }
}
