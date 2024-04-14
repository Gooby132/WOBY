using System.Collections.Immutable;
using Woby.Core.Core.Headers.Identities;
using Woby.Core.Core.Headers.Routings;

namespace Woby.Core.Core.Messages
{
    public class SignalingSection
    {
        public DialogId DialogId { get; init; }
        public SequenceHeader Sequence { get; init; }
        public MaxForwardings? MaxForwardings { get; init; }
        public Route To { get; init; }
        public Route From { get; init; }
        public IImmutableList<Route> Proxies { get; init; }

        public SignalingSection(DialogId dialogId, SequenceHeader sequence, MaxForwardings? maxForwardings, Route to, Route from, IImmutableList<Route> proxies)
        {
            DialogId = dialogId;
            Sequence = sequence;
            MaxForwardings = maxForwardings;
            To = to;
            From = from;
            Proxies = proxies;
        }
    }
}
