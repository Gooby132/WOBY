using System.Collections.Immutable;
using Woby.Core.Core.Headers.Identities;
using Woby.Core.Core.Headers.Routings;

namespace Woby.Core.Core.Messages
{
    public abstract class MessageBase
    {

        public DialogId DialogId { get; init; }
        public SequenceHeader Sequence { get; init; }
        public MaxForwardings? MaxForwardings { get; init; }
        /// <summary>
        /// The desired recipient request or the address-of-record of the user or resource that is the target request
        /// </summary>
        public Route To { get; init; }
        public Route From { get; init; }
        public IImmutableList<Route> Proxies { get; init; }

        protected MessageBase(
            DialogId id,
            SequenceHeader sequence,
            MaxForwardings? maxForwardings,
            Route to,
            Route from,
            ICollection<Route> proxies
            )
        {
            DialogId = id;
            Sequence = sequence;
            MaxForwardings = maxForwardings;
            To = to; From = from;
            Proxies = proxies.ToImmutableList();
        }
    }
}
