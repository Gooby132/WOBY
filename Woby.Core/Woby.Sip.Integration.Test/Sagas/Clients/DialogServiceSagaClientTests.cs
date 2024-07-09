using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Net;
using Woby.Core.Network.Core;
using Woby.Core.Sagas.Clients;
using Woby.Core.Sagas.Core;
using Woby.Core.Signaling.Sip.Builder;
using Woby.Core.Signaling.Sip.Converters;
using Woby.Core.Signaling.Sip.Parsers.Core;
using Woby.Core.Signaling.UserAgents.Repository;
using Woby.Core.UserAgents;
using Woby.Core.UserAgents.DependencyInjection;
using Woby.Core.UserAgents.Test;
using Woby.Sip.Integration.Test.Sagas.Clients.TestCases;
using static Woby.Core.UserAgents.UserAgent;

namespace Woby.Core.Integration.Test.Sagas.Clients
{
    [TestClass]
    public class DialogServiceSagaClientTests
    {
        private ServiceProvider _provider;

        [TestInitialize]
        public void Initialize()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddLogging(conf => conf.AddSerilog());
            services.AddInMemoryUserAgentRepository();
            services.AddTransient<DialogServiceSagaClient>();
            services.AddTransient<SipHeaderParser>();
            services.AddTransient<SipConverter>();
            services.AddTransient<SipBuilder>();

            _provider = services.BuildServiceProvider();
            _provider.GetRequiredService<IUserAgentsRepository>().PersistUserAgent(new UserAgentTest
            {
                Id = new Core.Signaling.UserAgents.ValueObjects.UserAgentId
                {
                    Id = "b"
                },
                DialogCreation = new List<DialogCreationOptions> { DialogCreationOptions.Answer }.GetEnumerator(),
                NotifyIncomingCallCommands = new List<NotifyIncomingCallOptions> { NotifyIncomingCallOptions.Notified }.GetEnumerator(),
                UpdateRequest = new List<DialogUpdateRequestOptions> { DialogUpdateRequestOptions.Notified }.GetEnumerator()
            });
        }

        [TestMethod(nameof(TestCases.UserAgentNotFound))]
        [DynamicData(nameof(TestCases.UserAgentNotFound), typeof(TestCases))]
        public void UserAgent_WasNotFound_Successful(string message, string expectedMessage)
        {
            string send = message;
            string expectedNotFound = expectedMessage;



            DefaultSagaBuilder<SipMessage> builder = new(_provider);

            var receiverTransmitter = new InMemoryChannel(_provider.GetRequiredService<ILogger<InMemoryChannel>>())
            {
                Message = send,
                NetworkMetadata = new NetworkMetadata
                {
                    NetworkProtocol = NetworkProtocol.Unknown,
                    ReceviedOn = IPEndPoint.Parse("192.0.2.4"),
                }
            };

            builder.ReceiveFromChannel(receiverTransmitter);
            builder.TransmitToChannel(receiverTransmitter);
            builder.AppendSignalingConverter<SipConverter, SipMessage>();
            builder.AppendSignalingBuilder<SipBuilder, SipMessage>();
            builder.AppendParser<SipHeaderParser, SipMessage>();
            builder.AddClient(_provider.GetRequiredService<DialogServiceSagaClient>());

            var saga = builder.BuildSaga();

            Assert.IsTrue(saga.IsSuccess, "building saga failed");

            saga.Value.Start();

            Assert.IsTrue(receiverTransmitter.MessagesSent.Any(), "No sent message");
            Assert.AreEqual(receiverTransmitter.MessagesSent[0], expectedNotFound, "Not all messages were sent");
        }

        [TestMethod(nameof(TestCases.UserAgentTryingAndRinging))]
        [DynamicData(nameof(TestCases.UserAgentTryingAndRinging), typeof(TestCases))]
        public void UserAgent_TryingAndRinging_Successful(
            string message,
            string expectedTrying,
            string expectedRinging)
        {
            string send = message;

            DefaultSagaBuilder<SipMessage> builder = new(_provider);

            var receiverTransmitter = new InMemoryChannel(_provider.GetRequiredService<ILogger<InMemoryChannel>>())
            {
                Message = send,
                NetworkMetadata = new NetworkMetadata
                {
                    NetworkProtocol = NetworkProtocol.Unknown,
                    ReceviedOn = IPEndPoint.Parse("192.0.2.4"),
                }
            };

            builder.ReceiveFromChannel(receiverTransmitter);
            builder.TransmitToChannel(receiverTransmitter);
            builder.AppendSignalingConverter<SipConverter, SipMessage>();
            builder.AppendSignalingBuilder<SipBuilder, SipMessage>();
            builder.AppendParser<SipHeaderParser, SipMessage>();
            builder.AddClient(_provider.GetRequiredService<DialogServiceSagaClient>());

            var saga = builder.BuildSaga();

            Assert.IsTrue(saga.IsSuccess, "building saga failed");

            saga.Value.Start();
            Assert.IsTrue(receiverTransmitter.MessagesSent.Any(), "No sent message");
            Assert.AreEqual(receiverTransmitter.MessagesSent[0], expectedTrying, "Expected trying are not equal");
            Assert.AreEqual(receiverTransmitter.MessagesSent[1], expectedRinging, "Expected ringing are not equal");

        }

    }
}
