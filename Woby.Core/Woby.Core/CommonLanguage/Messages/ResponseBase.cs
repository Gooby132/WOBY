namespace Woby.Core.CommonLanguage.Messages
{
    public class ResponseBase : MessageBase
    {
        public ResponseBase(SignalingSection signaling, ContentSection? content = null) : base(signaling, content)
        {
        }
    }
}
