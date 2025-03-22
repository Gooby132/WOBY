using Woby.Core.CommonLanguage.Signaling.Identities;
using Woby.Core.Commons.Errors;
using Woby.Core.Signaling.UserAgents.ValueObjects;

namespace Woby.Core.Signaling.UserAgents.Errors
{
    public static class UserAgentErrors
    {

        public const int GroupCode = 1;

        public static ErrorBase UserWasNotFound(UserAgentId userAgentId) => new NotFoundErrorBase(GroupCode, 1, userAgentId.Id); 
        public static ErrorBase DialogWasNotFound(DialogId dialogId) => new NotFoundErrorBase(GroupCode, 2, dialogId.Id); 
        public static ErrorBase UserAgentIdIsInvalid() => new InvalidError(GroupCode, 3, "failed to parse user agent id"); 

    }
}
