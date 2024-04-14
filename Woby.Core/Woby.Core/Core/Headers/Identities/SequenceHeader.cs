using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woby.Core.Core.Headers.Core;

namespace Woby.Core.Core.Headers.Identities
{
    public class SequenceHeader : HeaderBase
    {
        public int Sequence { get; }

        // other protocols might not support this
        public string? Method { get; }

        public SequenceHeader(string key, uint sequence, string? method, string body) : base(key, body, HeaderType.Identity)
        {
            Sequence = (int)sequence;
            Method = method;
        }
    }
}
