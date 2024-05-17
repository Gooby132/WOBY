using Woby.Core.CommonLanguage.Signaling.Core;

namespace Woby.Core.CommonLanguage.Signaling.Identities
{
    public class NegotiationId : SignalingHeader
    {
        public string Id => Body;

        public NegotiationId(string key, string body) : base(key, body, HeaderType.Identity)
        {
        }

        public static bool operator ==(NegotiationId left, NegotiationId right) => 
            left.Id == right.Id;
        public static bool operator !=(NegotiationId left, NegotiationId right) => !(left == right);

        public override string ToString() => Id;

        public override int GetHashCode() => Id.GetHashCode();

    }
}
