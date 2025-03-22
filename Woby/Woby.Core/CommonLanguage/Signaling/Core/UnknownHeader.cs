using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woby.Core.CommonLanguage.Signaling.Core
{
    public class UnknownHeader : SignalingHeader
    {
        public UnknownHeader(string key, string body) : base(key, body, HeaderType.Unknown)
        {
        }
    }
}
