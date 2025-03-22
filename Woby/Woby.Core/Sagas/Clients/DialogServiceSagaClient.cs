using FluentResults;
using Microsoft.Extensions.Logging;
using Woby.Core.CommonLanguage.Messages;
using Woby.Core.CommonLanguage.Signaling.Roles;
using Woby.Core.Commons.Errors;
using Woby.Core.Signaling.UserAgents.Repository;
using Woby.Core.Signaling.UserAgents.ValueObjects;
using Woby.Core.UserAgents;

namespace Woby.Core.Sagas.Clients
{

    /// <summary>
    /// SIP Dialog Service is responsible for managing incoming messages and routing them to correct user agents
    /// </summary>
    public class DialogServiceSagaClient : SagaClientBase
    {

        #region Fields

        private readonly ILogger<DialogServiceSagaClient> _logger;
        private readonly IUserAgentsRepository _userAgentRepository;

        #endregion

        #region Constructor

        public DialogServiceSagaClient(
            ILogger<DialogServiceSagaClient> logger,
            IUserAgentsRepository userAgentRepository
            )
        {
            _logger = logger;
            _userAgentRepository = userAgentRepository;
        }

        #endregion

        public override Func<RequestBase, Task<Result>> GetRequest() => async (request) =>
        {
            Result<UserAgentId> userAgentId;
            Result<UserAgent>? userAgent;

            var trying = await SendMessage(MessageFactory.Trying(request.Signaling));

            if(trying.IsFailed)
            {
                _logger.LogWarning("{this} sent trying to message. failed - '{errors}'", 
                    this, 
                    trying.Reasons.Select(r => r.Message));
            }

            userAgentId = UserAgentId.CreateUserAgentIdFromRoute(request.Signaling.To); // TODO: should support multiples

            if (userAgentId.IsFailed)
                return Result.Fail(userAgentId.Errors);

            userAgent = _userAgentRepository.GetUserAgent(userAgentId.Value);

            if (userAgent.IsFailed)
            {
                if (userAgent.HasError<NotFoundErrorBase>())
                {
                    _logger.LogWarning("{this} request was made to an unknown user agent - '{id}'", this, userAgentId);

                    return await SendMessage(MessageFactory.UserAgentNotFound(request.Signaling));
                }

                _logger.LogError("{this} request made to user agent - '{userAgent}' failed. error(s) - '{errors}'",
                    this, userAgentId, string.Join(", ", userAgent.Reasons.Select(r => r.Message)));

                return await SendMessage(MessageFactory.EndOfTransaction());
            }

            if (request.Signaling.Role == RoleType.DialogCreationRequest)
            {
                var notifyDialogCreation = userAgent.Value.HandleNotifyIncomingCall(request);

                if (notifyDialogCreation.IsFailed)
                {
                    _logger.LogWarning("{this} notify dialog creation made to user agent - '{userAgent}' failed. error(s) - '{errors}'",
                        this, userAgentId, string.Join(", ", notifyDialogCreation.Reasons.Select(r => r.Message)));

                    return await SendMessage(MessageFactory.EndOfTransaction());
                }

                await SendMessage(MessageFactory.Ringing(request.Signaling));

                var incomingResult = userAgent.Value.HandleIncomingCallRequest(request);

                if (incomingResult.IsFailed)
                {
                    _logger.LogWarning("{this} incoming call user agent - '{userAgentId}' result failed. error(s) - '{errors}'",
                        this, userAgentId, string.Join(", ", incomingResult.Errors.Select(r => r.Message)));

                    return await SendMessage(MessageFactory.EndOfTransaction());
                }

                _logger.LogTrace("{this} incoming call user agent - '{userAgentId}' result was to answer",
                    this, userAgentId);

                var persist = _userAgentRepository.PersistUserAgent(userAgent.Value);

                if (persist.IsFailed)
                {
                    _logger.LogWarning("{this} user agent - '{id}' failed to presist user agent state for request - {request}. error(s) - '{errors}'",
                        this, userAgentId, request, string.Join(", ", persist.Reasons.Select(r => r.Message)));

                    return await SendMessage(MessageFactory.EndOfTransaction());
                }

                var sentMessage = await SendMessage(incomingResult.Value);

                if (sentMessage.IsFailed)
                {
                    _logger.LogWarning("{this} saga client failed to send message. error(s) - '{errors}'",
                        this, string.Join(", ", sentMessage.Reasons.Select(r => r.Message)));

                    return await SendMessage(MessageFactory.EndOfTransaction());
                }
            }

            // not creation thus already exists
            userAgent.Value.HandleDialogRequest(request);

            return await SendMessage(MessageFactory.EndOfTransaction());
        };

        public override Func<ResponseBase, Task<Result>> GetResponse() => throw new NotImplementedException();

        public override string ToString() => nameof(DialogServiceSagaClient);

    }
}
