using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woby.Core.CommonLanguage.Signaling.Core;

namespace Woby.Core.CommonLanguage.Signaling.Linguistics
{
    public class ContentLanguage : SignalingHeader
    {
        public ContentLanguage(string key, string body) : base(key, body, HeaderType.Linguistics)
        {
        }
    }
}
