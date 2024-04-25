using Woby.Core.CommonLanguage.Signaling.Core;

namespace Woby.Core.CommonLanguage.Signaling.Routings
{
    public class MaxForwardings : SignalingHeader
    {

        public int MaxForwards { get; }

        public MaxForwardings(string key, uint maxForwards, string body) : base(key, body, HeaderType.Routing)
        {
            MaxForwards = (int)maxForwards;
        }
    }
}
