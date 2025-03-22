using FluentResults;
using Microsoft.Extensions.Logging;
using Woby.Core.Abstractions;
using Woby.Core.CommonLanguage.Messages;
using Woby.Core.Sagas.Errors;

namespace Woby.Application.Sagas.Clients
{

    /// <summary>
    /// This class represent an interceptor of a SAGA pattern it is used for creating a different responses and routing them accordingly
    /// </summary>
    public abstract class SagaClientBase
    {

        #region Fields

        private ILogger<SagaClientBase>? _logger;
        private ISagaTransmitter? _transmittor;

        #endregion

        #region Constructor
        
        public SagaClientBase()
        {
        }

        #endregion

        #region Methods
        
        public void SetLogger(ILogger<SagaClientBase> logger)
        {
            _logger = logger;
        }

        public void SetSagaTransmittor(ISagaTransmitter transmittor)
        {
            _transmittor = transmittor;
        }

        protected async Task<Result> SendMessage(MessageBase message)
        {
            if(_transmittor is null)
            {
                _logger?.LogError("{this} transmittor was not set on saga client",
                    this);

                return Result.Fail(SagaErrors.ClientSagaTransmissionWasNotProvided());
            }

            return await _transmittor.SendMessage(message);
        } 

        public abstract Func<RequestBase, Task<Result>> GetRequest();

        public abstract Func<ResponseBase, Task<Result>> GetResponse();

        public override string ToString() => nameof(SagaClientBase);

        #endregion

    }
}
