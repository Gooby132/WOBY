using Microsoft.Extensions.Logging;
using Woby.Core.CommonLanguage.Messages;
using Woby.Core.Commons.Errors;
using Woby.Core.SagaClients;
using Woby.Core.Signaling.Sip.Dialogs.Repository;
using Woby.Core.Signaling.Sip.UserAgents.Repository;

namespace Woby.Core.Signaling.Sip.SagaClients
{
    public class SipDialogService : ISagaClientBase
    {

        #region Fields
        
        private readonly ILogger<SipDialogService> _logger;
        private readonly IDialogRepository _dialogRepository;
        private readonly IUserAgentsRepository _userAgentRepository;

        #endregion

        #region Constructor

        public SipDialogService(
            ILogger<SipDialogService> logger,
            IDialogRepository dialogRepository,
            IUserAgentsRepository userAgentRepository
            )
        {
            _logger = logger;
            _dialogRepository = dialogRepository;
            _userAgentRepository = userAgentRepository;
        }

        #endregion

        public Func<MessageBase, MessageBase> GetGeneralMessage() => (message) =>
        {
            throw new NotImplementedException();
        };

        public Func<RequestBase, MessageBase> GetRequest() => (request) =>
        {
            var dialog = _dialogRepository.GetDialog(request.Signaling.DialogId);

            if (dialog.IsFailed && !dialog.HasError<NotFoundErrorBase>())
            {
                _logger.LogTrace("{this} dialog with id - '{id}' was not found", this, request.Signaling.DialogId);
            }

            if (dialog.HasError<NotFoundErrorBase>())
            {
                request.Signaling.To.Uri;
            }

        };

        public Func<ResponseBase, MessageBase> GetResponse() => (response) =>
        {
            throw new NotImplementedException();
        };

        public override string ToString() => nameof(SipDialogService);

    }
}
