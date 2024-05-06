using FluentResults;
using Woby.Core.Abstractions;
using Woby.Core.Network.Abstractions;
using Woby.Core.Sagas.Errors;

namespace Woby.Core.Sagas.Core
{
    internal class DefaultSagaBuilder<InputMessage> : ISagaBuilder<InputMessage>
    {

        private readonly IServiceProvider _provider;

        private Type? _client;
        private Type? _builder;
        private Type? _converter;
        private Type? _parser;
        private IChannel? _receiver;
        private IChannel? _transmitter;

        public DefaultSagaBuilder(IServiceProvider provider)
        {
            _provider = provider;
        }

        public ISagaBuilder<InputMessage> AddClient<Client>() where Client : IClient
        {
            _client = typeof(Client);
            return this;
        }

        public ISagaBuilder<InputMessage> AppendBuilder<Builder, MessagesType>() where Builder : IBuilder
        {
            _builder = typeof(Builder)
                .MakeGenericType(typeof(MessagesType));
            return this;
        }

        public ISagaBuilder<InputMessage> AppendConverter<Converter, MessagesType>() where Converter : IConverter<MessagesType>
        {
            _converter = typeof(Converter)
                .MakeGenericType(typeof(MessagesType));
            return this;
        }

        public ISagaBuilder<InputMessage> AppendParser<ParserType, MessagesType>() where  ParserType : IParser<MessagesType> , new()
        {
            _parser = typeof(ParserType)
                .MakeGenericType(typeof(MessagesType));

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
        
        public Result<ISaga<InputMessage>> Build()
        {
            if (_client is null)
                return Result.Fail(SagaBuilderErrors.ClientWasNotProvided());

            if(_builder is null)
                return Result.Fail(SagaBuilderErrors.BuilderWasNotProvided());
            
            if(_receiver is null)
                return Result.Fail(SagaBuilderErrors.ReceiverWasNotProvided());
            
            if(_converter is null)
                return Result.Fail(SagaBuilderErrors.ConverterWasNotProvided());

            if (_transmitter is null)
                return Result.Fail(SagaBuilderErrors.TransmitterWasNotProvided());

            if (_parser is null)
                return Result.Fail(SagaBuilderErrors.ParserWasNotProvided());

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

            return saga;
        }
    }
}
