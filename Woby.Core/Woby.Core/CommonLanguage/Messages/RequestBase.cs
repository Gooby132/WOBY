namespace Woby.Core.CommonLanguage.Messages
{
    public class RequestBase : MessageBase
    {
        public RequestBase(SignalingSection signaling, ContentSection? content = null) : base()
        {
            Signaling = signaling;
            Content = content;
        }
    }
}
