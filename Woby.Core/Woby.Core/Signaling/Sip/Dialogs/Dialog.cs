using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woby.Core.CommonLanguage.Messages;
using Woby.Core.CommonLanguage.Signaling.Identities;

namespace Woby.Core.Signaling.Sip.Dialogs
{
    public class Dialog
    {

        public DialogId Id { get; }
        public ICollection<MessageBase> Messages { get; }

        public Dialog(DialogId id)
        {
            Id = id;
        }

        public Result AppendMessage(MessageBase message)
        {
            Messages.Add(message);

            return Result.Ok();
        }

    }
}
