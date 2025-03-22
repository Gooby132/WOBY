using FluentResults;
using System.Collections.Immutable;
using Woby.Core.CommonLanguage.Messages;
using Woby.Core.CommonLanguage.Signaling.Identities;
using Woby.Core.Signaling.UserAgents.ValueObjects;
using Woby.Core.Signaling.Primitives;

namespace Woby.Core.Signaling.Dialogs
{
    public class Dialog : Entity<DialogId>
    {
        public required UserAgentId Callee { get; init; }
        public required UserAgentId Caller { get; init; }
        public ImmutableList<MessageBase> Messages { get; init; }

        internal Dialog() {     }

        public Result AppendMessage(MessageBase message)
        {
            Messages.Add(message);

            return Result.Ok();
        }

    }
}
