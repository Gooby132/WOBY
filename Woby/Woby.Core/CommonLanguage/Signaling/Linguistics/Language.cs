using Woby.Core.CommonLanguage.Signaling.Core;

namespace Woby.Core.CommonLanguage.Signaling.Linguistics
{
    public class Language : SignalingHeader
    {
        public IEnumerable<LanguageOrder> LanguageOrders { get; }

        public Language(IEnumerable<LanguageOrder> languageOrders, string key, string body) : base(key, body, HeaderType.Linguistics)
        {
            LanguageOrders = languageOrders;
        }
    }
}
