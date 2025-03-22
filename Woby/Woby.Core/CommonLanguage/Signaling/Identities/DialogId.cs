using Woby.Core.CommonLanguage.Signaling.Core;

namespace Woby.Core.CommonLanguage.Signaling.Identities
{
    public class DialogId : SignalingHeader
    {
        public string Id => Body;

        public DialogId(string key, string body) : base(key, body, HeaderType.Identity)
        {
        }

        public static bool operator ==(DialogId left, DialogId right) => 
            left.Id == right.Id;
        public static bool operator !=(DialogId left, DialogId right) => !(left == right);

        public override string ToString() => Id;

        public override int GetHashCode() => Id.GetHashCode();

    }
}
