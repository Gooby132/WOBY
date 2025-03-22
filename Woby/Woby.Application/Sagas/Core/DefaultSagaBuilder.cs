using FluentResults;
using Woby.Core.Abstractions;
using Woby.Core.Network.Abstractions;
using Woby.Core.Sagas.Errors;
using Woby.Core.Sagas.Clients;
using Woby.Core.Commons.Errors;

namespace Woby.Application.Sagas.Core
{

    /// <summary>
    /// Builds a default Saga making and orchestrating output and input channels
    /// Builder has to be provided with the followings
    /// <list type="bullet">
    /// <item>
    /// <description> Common Language - <c>IBuilder</c> </description>
    /// </item>
    /// <item>
    /// <description> <c>IChannel</c> Receiver </description>
    /// </item>
    /// <item>
    /// <description> <c>IChannel</c> Transmitter </description>
    /// </item>
    /// <item>
    /// <description> Common Lanugage <c>IConverter</c> </description>
    /// </item>
    /// <item>
    /// <description> Common Language <c>IParser</c> </description>
    /// </item>
    /// </list>
    /// </summary>
    /// <typeparam name="InputMessage"></typeparam>
    public class DefaultSagaBuilder<InputMessage> : ISagaBuilder<InputMessage>
    {

        private readonly IServiceProvider _provider;

        private SagaClientBase? _client;
        private Type? _builder;
        private Type? _converter;
        private Type? _parser;
        private IChannel? _receiver;
        private IChannel? _transmitter;

        public DefaultSagaBuilder(IServiceProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// The <c>SagaBaseClient</c> which receives the finalized message after the converter phase
        /// </summary>
        /// <param name="sagaClient"></param>
        /// <returns></returns>
        public ISagaBuilder<InputMessage> AddClient(SagaClientBase sagaClient)
        {
            _client = sagaClient;
            return this;
        }

        public ISagaBuilder<InputMessage> AppendSignalingBuilder<Builder, MessagesType>() where Builder : IBuilder
        {
            _builder = typeof(Builder);
            return this;
        }

        public ISagaBuilder<InputMessage> AppendSignalingConverter<Converter, MessagesType>() where Converter : ISignalingConverter<MessagesType>
        {
            _converter = typeof(Converter);
            return this;
        }

        public ISagaBuilder<InputMessage> AppendParser<ParserType, MessagesType>() where ParserType : IParser<MessagesType>
        {
            _parser = typeof(ParserType);

            return this;
        }

        public ISagaBuilder<InputMessage> ReceiveFromChannel(IChannel channel)
        {
            _receiver = channel;
            return this;
        }

        public ISagaBuilder<InputMessage> TransmitToChannel(IChannel channel)
        {
            _transmitter = channel;
            return this;
        }

        public Result<ISaga<InputMessage>> BuildSaga()
        {
            if (_client is null)
                return Result.Fail(SagaErrors.ClientWasNotProvided());

            if (_builder is null)
                return Result.Fail(SagaErrors.BuilderTypeWasNotProvided());

            if (_receiver is null)
                return Result.Fail(SagaErrors.ReceiverWasNotProvided());

            if (_converter is null)
                return Result.Fail(SagaErrors.ConverterTypeWasNotProvided());

            if (_transmitter is null)
                return Result.Fail(SagaErrors.TransmitterWasNotProvided());

            if (_parser is null)
                return Result.Fail(SagaErrors.ParserTypeWasNotProvided());

            var saga = new DefaultSaga<InputMessage>(
                _provider,
                _builder,
                _converter,
                _parser,
                _client,
                _receiver,
                _transmitter,
                null,
                null,
                null,
                null
                );

            _client.SetSagaTransmittor(saga);

            return saga;
        }
    }
}
