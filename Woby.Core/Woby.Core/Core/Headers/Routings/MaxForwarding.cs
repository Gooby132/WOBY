using Woby.Core.Core.Headers.Core;

namespace Woby.Core.Core.Headers.Routings
{
    public class MaxForwarding : HeaderBase
    {

        public int MaxForwards { get; }

        public MaxForwarding(string key, uint maxForwards, string body) : base(key, body, HeaderType.Routing)
        {
            MaxForwards = (int)maxForwards;
        }
    }
}
