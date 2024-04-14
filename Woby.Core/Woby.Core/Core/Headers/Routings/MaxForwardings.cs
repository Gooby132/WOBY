using Woby.Core.Core.Headers.Core;

namespace Woby.Core.Core.Headers.Routings
{
    public class MaxForwardings : HeaderBase
    {

        public int MaxForwards { get; }

        public MaxForwardings(string key, uint maxForwards, string body) : base(key, body, HeaderType.Routing)
        {
            MaxForwards = (int)maxForwards;
        }
    }
}
