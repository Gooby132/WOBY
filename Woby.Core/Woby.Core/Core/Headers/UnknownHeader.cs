using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woby.Core.Core.Headers
{
    public class UnknownHeader : HeaderBase
    {
        public UnknownHeader(string key, string body) : base(key, body, HeaderType.Unknown)
        {
        }
    }
}
