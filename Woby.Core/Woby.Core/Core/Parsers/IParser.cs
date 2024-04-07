using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woby.Core.Core.Messages;

namespace Woby.Core.Core.Parsers
{
    public interface IParser
    {

        public Result<MessageBase> Parse();
        public Result Validate();
        public bool TryParse(out MessageBase message);

    }
}
