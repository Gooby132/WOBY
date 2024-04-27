using System.Collections.Immutable;
using Woby.Core.CommonLanguage.Signaling.ContentMeta;
using Woby.Core.CommonLanguage.Signaling.Identities;
using Woby.Core.CommonLanguage.Signaling.Routings;

namespace Woby.Core.CommonLanguage.Messages
{
    public class SignalingSection
    {
        public DialogId DialogId { get; init; }
        public SequenceHeader Sequence { get; init; }
        public MaxForwardings MaxForwardings { get; init; }
        public Route To { get; init; }
        public Route From { get; init; }
        public IImmutableList<Route> Proxies { get; init; }
        public ContentType ContentType { get; init; }
        public ContentLength ContentLength { get; init; }

        public SignalingSection(
            DialogId dialogId, 
            SequenceHeader sequence, 
            MaxForwardings maxForwardings, 
            Route to, 
            Route from, 
            IImmutableList<Route> proxies,
            ContentType contentType,
            ContentLength contentLength)
        {
            DialogId = dialogId;
            Sequence = sequence;
            MaxForwardings = maxForwardings;
            To = to;
            From = from;
            Proxies = proxies;
            ContentType = contentType;
            ContentLength = contentLength;
        }
    }
}
