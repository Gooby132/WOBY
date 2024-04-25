using Woby.Core.CommonLanguage.Signaling.Core;

namespace Woby.Core.CommonLanguage.Signaling.Identities
{
    public class DialogId : SignalingHeader
    {
        public string Id => Body;
        public DialogId(string key, string body) : base(key, body, HeaderType.Identity)
        {

        }
    }
}
