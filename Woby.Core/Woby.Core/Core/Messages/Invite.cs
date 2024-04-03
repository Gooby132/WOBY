using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woby.Core.Core.Messages
{
    public class Invite : MessageBase
    {
        public Invite(string to, string from, string cSeq, string callId, int maxForward, string via) : base(to, from, cSeq, callId, maxForward, via)
        {
        }
    }
}
