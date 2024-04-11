using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woby.Core.Core.Headers.Core;

namespace Woby.Core.Core.Headers.Linguistics
{
    public class ContentLanguage : HeaderBase
    {
        public ContentLanguage(string key, string body) : base(key, body, HeaderType.Linguistics)
        {
        }
    }
}
