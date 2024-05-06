using FluentResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Woby.Core.Abstractions;
using Woby.Core.Clients;
using Woby.Core.CommonLanguage.Messages;
using Woby.Core.Commons.Errors;
using Woby.Core.Network.Abstractions;

namespace Woby.Core.Sagas.Core
{
    internal class DefaultSaga<InputMessage> : ISaga<InputMessage>
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<DefaultSaga<InputMessage>> _logger;
        private readonly Type _builder;
        private readonly Type _converterType;
        private readonly Type _parserType;
        private readonly Type _clientType;
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
            Type client,
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
                    .MakeGenericType(typeof(ILogger<>)
                        .MakeGenericType(typeof(DefaultSaga<>)
                            .MakeGenericType(typeof(InputMessage))))) as ILogger<DefaultSaga<InputMessage>>)!;

            _builder = builder;
            _converterType = converter;
            _parserType = parser;
            _clientType = client;
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

            var parser = (_provider.GetRequiredService(_parserType) as IParser<InputMessage>)!;

            var parsed = parser.Parse(message);

            if (parsed.IsFailed)
            {
                _parserErrorsCallback?.Invoke(parsed.Errors.Select(e => (e as ErrorBase)!));
                return;
            }

            var converter = (_provider.GetRequiredService(_converterType) as IConverter<InputMessage>)!;

            var common = converter.Convert(parsed.Value);
            
            if(common.IsFailed)
            {
                _converterErrorsCallback?.Invoke(common.Errors.Select(e => (e as ErrorBase)!));
                return;
            }

            var client = (_provider.GetRequiredService(_clientType) as ClientBase)!;

            if (common.Value is ResponseBase response)
                client.GetResponse(response);
            else if(common.Value is RequestBase request)
                client.GetRequest(request);
            else
                client.GetGeneralMessage(common.Value);

            var builder = (_provider.GetRequiredService(_builder) as IBuilder)!;

            var outputStream = await builder.Build(common.Value);

            if (outputStream.IsFailed)
            {
                _builderErrorsCallback?.Invoke(outputStream.Errors.Select(e => (e as ErrorBase)!));
                return;
            }

            var trasmissionResult = await _transmitter.Transmit(outputStream.Value);

            if(trasmissionResult.IsFailed)
            {
                _transmissionErrorsCallback?.Invoke(trasmissionResult.Errors.Select(e => (e as ErrorBase)!));
                return;
            }
        }

        public Result Start() => _receiver.Subscribe(this);

        public Result End() => _receiver.Unsubscribe(this);
    }
}
