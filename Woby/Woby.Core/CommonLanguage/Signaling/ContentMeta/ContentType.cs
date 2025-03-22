using Woby.Core.CommonLanguage.Signaling.Core;

namespace Woby.Core.CommonLanguage.Signaling.ContentMeta
{
    public class ContentType : SignalingHeader
    {
        public System.Net.Mime.ContentType Content { get; }

        public ContentType(string key, string body, System.Net.Mime.ContentType content) : base(key, body, HeaderType.ContentMetadata)
        {
            Content = content;
        }
    }
}
