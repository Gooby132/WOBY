using Woby.Core.CommonLanguage.Messages;
using Woby.Core.CommonLanguage.Signaling.Routings;
using Woby.Core.Signaling.Dialogs;

namespace Woby.Core.UserAgents.Test
{
    public sealed class UserAgentTest : UserAgent
    {
        public required IEnumerator<DialogUpdateRequestOptions> UpdateRequest { get; init; }
        public required IEnumerator<DialogCreationOptions> DialogCreation { get; init; }
        public required IEnumerator<NotifyIncomingCallOptions> NotifyIncomingCallCommands { get; init; }

        public UserAgentTest()
        {
            Dialogs = new List<Dialog>();
        }

        public override DialogUpdateRequestOptions DialogUpdateRequest(Dialog previous, Dialog updated)
        {
            var temp = UpdateRequest.Current;
            UpdateRequest.MoveNext();
            return temp;
        }

        public override DialogCreationOptions IncomingCall(RequestBase request)
        {
            var temp = DialogCreation.Current;
            DialogCreation.MoveNext();
            return temp;
        }

        public override NotifyIncomingCallOptions NotifyIncomingCall(Route from)
        {
            NotifyIncomingCallCommands.MoveNext();
            var temp = NotifyIncomingCallCommands.Current;
            return temp;
        }
    }
}
