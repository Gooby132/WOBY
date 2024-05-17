using FluentResults;
using Woby.Core.CommonLanguage.Signaling.Identities;
using Woby.Core.Signaling.Dialogs.Repository;

namespace Woby.Core.Signaling.Dialogs.Repositories
{
    internal class InMemoryDialogRepository : IDialogRepository
    {

        private readonly IDictionary<DialogId, Dialog> _dialogs;

        public Result<Dialog> GetDialog(DialogId id)
        {
            throw new NotImplementedException();
        }

        public Result RegisterNewDialog(Dialog dialog)
        {
            throw new NotImplementedException();
        }
    }
}
