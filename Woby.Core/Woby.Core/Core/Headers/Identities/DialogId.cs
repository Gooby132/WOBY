using Woby.Core.Core.Headers.Core;

namespace Woby.Core.Core.Headers.Identities
{
    public class DialogId : HeaderBase
    {
        public string Id => Body;
        public DialogId(string key, string body) : base(key, body, HeaderType.Identity)
        {

        }
    }
}
