using FluentResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Woby.Core.Abstractions;
using Woby.Core.CommonLanguage.Messages;
using Woby.Core.Commons.Errors;
using Woby.Core.Network.Abstractions;
using Woby.Core.Sagas.Clients;

namespace Woby.Core.Sagas.Core
{
    internal class DefaultSaga<InputMessage> : ISaga<InputMessage>, ISagaTransmitter
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<DefaultSaga<InputMessage>> _logger;
        private readonly Type _builder;
        private readonly Type _converterType;
        private readonly Type _parserType;
        private readonly SagaClientBase _client;
        private readonly IChannel _receiver;
        private readonly IChannel _transmitter;
        private readonly Action<IEnumerable<ErrorBase>>? _parserErrorsCallback;
        private readonly Action<IEnumerable<ErrorBase>>? _converterErrorsCallback;
        private readonly Action<IEnumerable<ErrorBase>>? _builderErrorsCallback;
        private readonly Action<IEnumerable<ErrorBase>>? _transmissionErrorsCallback;

        internal DefaultSaga(
            IServiceProvider provider,
            Type builder, 
            Type converter,
            Type parser,
            SagaClientBase client,
            IChannel receiver, 
            IChannel transmitter,
            Action<IEnumerable<ErrorBase>>? parserErrorsCallback,
            Action<IEnumerable<ErrorBase>>? converterErrorsCallback,
            Action<IEnumerable<ErrorBase>>? builderErrorsCallback,
            Action<IEnumerable<ErrorBase>>? transmissionErrorsCallback
            )
        {
            _provider = provider;
            _logger = (provider
                .GetRequiredService(typeof(ILogger<>)
                    .MakeGenericType(typeof(DefaultSaga<>)
                        .MakeGenericType(typeof(InputMessage)))) as ILogger<DefaultSaga<InputMessage>>)!;

            _builder = builder;
            _converterType = converter;
            _parserType = parser;
            _client = client;
            _receiver = receiver;
            _transmitter = transmitter;
            _parserErrorsCallback = parserErrorsCallback;
            _converterErrorsCallback = converterErrorsCallback;
            _builderErrorsCallback = builderErrorsCallback;
            _transmissionErrorsCallback = transmissionErrorsCallback;
        }

        public async Task ReceiveMessage(Stream inputStream)
        {
            using var reader = new StreamReader(inputStream);

            var message = await reader.ReadToEndAsync();

            var parser = (_provider.GetRequiredService(_parserType) as IParser<InputMessage>);

            var parsed = parser.Parse(message);

            if (parsed.IsFailed)
            {
                _parserErrorsCallback?.Invoke(parsed.Errors.Select(e => (e as ErrorBase)!));
                return;
            }

            var converter = (_provider.GetRequiredService(_converterType) as ISignalingConverter<InputMessage>)!;

            var common = converter.Convert(parsed.Value);
            
            if(common.IsFailed)
            {
                _converterErrorsCallback?.Invoke(common.Errors.Select(e => (e as ErrorBase)!));
                return;
            }

            if (!common.Value.Role.IsRequestOrResponse)
                await _client.GetResponse().Invoke(new ResponseBase()
                {
                    Signaling = common.Value,
                    Content = null
                });
            else
                await _client.GetRequest().Invoke(new RequestBase()
                {
                    Signaling = common.Value,
                    Content = null
                });
        }

        public Result Start() => _receiver.Subscribe(this);

        public Result End() => _receiver.Unsubscribe(this);

        public async Task<Result> SendMessage(MessageBase message)
        {
            if (message is NoMessage)
                return Result.Ok();

            var builder = (_provider.GetRequiredService(_builder) as IBuilder)!;

            Result<Stream> outputStream = Result.Fail("Unknown response");
            if (message is UserAgentNotFound)
            {
                outputStream = await builder.BuildUserAgentWasNotFound(message);
            }

            if(message is ResponseBase response)
            {
                outputStream = await builder.Build(response);
            }

            if (outputStream.IsFailed)
            {
                _builderErrorsCallback?.Invoke(outputStream.Errors.Select(e => (e as ErrorBase)!));
                return Result.Fail(outputStream.Errors);
            }

            var trasmissionResult = await _transmitter.Transmit(outputStream.Value);

            if (trasmissionResult.IsFailed)
            {
                _transmissionErrorsCallback?.Invoke(trasmissionResult.Errors.Select(e => (e as ErrorBase)!));
                return Result.Fail(trasmissionResult.Errors);
            }

            return Result.Ok();
        }
    }
}
