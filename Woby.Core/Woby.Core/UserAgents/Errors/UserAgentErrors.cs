using Woby.Core.CommonLanguage.Signaling.Identities;
using Woby.Core.Commons.Errors;
using Woby.Core.Signaling.UserAgents.ValueObjects;

namespace Woby.Core.Signaling.UserAgents.Errors
{
    public static class UserAgentErrors
    {

        public const int GroupCode = 1;

        public static NotFoundErrorBase UserWasNotFound(UserAgentId userAgentId) => new NotFoundErrorBase(GroupCode, 1, userAgentId.Id); 
        public static NotFoundErrorBase DialogWasNotFound(DialogId dialogId) => new NotFoundErrorBase(GroupCode, 1, dialogId.Id); 

    }
}
