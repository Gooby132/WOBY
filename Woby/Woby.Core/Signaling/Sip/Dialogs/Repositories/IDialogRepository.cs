using FluentResults;
using Woby.Core.CommonLanguage.Signaling.Identities;

namespace Woby.Core.Signaling.Sip.Dialogs.Repository
{
    public interface IDialogRepository
    {

        public Result RegisterNewDialog(Dialog dialog);

        public Result<Dialog> GetDialog(DialogId id);

    }
}
