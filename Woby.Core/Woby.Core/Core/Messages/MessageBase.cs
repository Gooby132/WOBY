using System.Collections.Immutable;
using Woby.Core.Core.Headers.Identities;
using Woby.Core.Core.Headers.Routings;

namespace Woby.Core.Core.Messages
{
    public abstract class MessageBase
    {

        public SignalingSection Signaling { get; }
        public ContentSection? Content { get; }

        protected MessageBase(SignalingSection signaling, ContentSection? content = null)
        {
            Signaling = signaling;
            Content = content;
        }

    }
}
