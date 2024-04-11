using Woby.Core.Core.Headers.Core;

namespace Woby.Core.Core.Headers.Linguistics
{
    public class Language : HeaderBase
    {
        public IEnumerable<LanguageOrder> LanguageOrders { get; }

        public Language(IEnumerable<LanguageOrder> languageOrders, string key, string body) : base(key, body, HeaderType.Linguistics)
        {
            LanguageOrders = languageOrders;
        }
    }
}
