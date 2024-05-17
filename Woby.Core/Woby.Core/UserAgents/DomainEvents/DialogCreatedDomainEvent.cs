﻿using Woby.Core.CommonLanguage.Messages;
using Woby.Core.CommonLanguage.Signaling.Identities;
using Woby.Core.Signaling.UserAgents;
using Woby.Core.Signaling.Primitives;

namespace Woby.Core.Signaling.UserAgents.DomainEvents
{
    public class DialogCreatedDomainEvent : IDomainEvent
    {

        public required UserAgent UserAgent { get; init; }
        public required DialogId DialogId { get; init; }
        public required MessageBase Response { get; init; }
    }
}
