namespace Woby.Core.CommonLanguage.Messages
{
    public class MessageBase
    {

        public SignalingSection Signaling { get; }
        public ContentSection? Content { get; }

        public MessageBase(SignalingSection signaling, ContentSection? content = null)
        {
            Signaling = signaling;
            Content = content;
        }

    }
}
