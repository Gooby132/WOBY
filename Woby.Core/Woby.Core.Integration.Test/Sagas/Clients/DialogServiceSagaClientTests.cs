using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Woby.Core.Network.Core;
using Woby.Core.Sagas.Clients;
using Woby.Core.Sagas.Core;
using Woby.Core.Signaling.Dialogs;
using Woby.Core.Signaling.Sip.Builder;
using Woby.Core.Signaling.Sip.Converters;
using Woby.Core.Signaling.Sip.Parsers.Core;
using Woby.Core.Signaling.UserAgents.Repository;
using Woby.Core.Signaling.UserAgents.ValueObjects;
using Woby.Core.UserAgents;
using Woby.Core.UserAgents.Test;

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
            services.AddTransient<IUserAgentsRepository, InMemoryUserAgentRepository>();
            services.AddTransient<DialogServiceSagaClient>();
            services.AddTransient<SipHeaderParser>();
            services.AddTransient<SipConverter>();
            services.AddTransient<SipBuilder>();

            _provider = services.BuildServiceProvider();

        }

        [TestMethod]
        public void UserAgent_WasNotFound_Successful()
        {
            DefaultSagaBuilder<SipMessage> builder = new(_provider);

            var receiverTransmitter = new InMemoryChannel(_provider.GetRequiredService<ILogger<InMemoryChannel>>())
            {
                Message =
                    "INVITE sip:user2@domain.com SIP/2.0\r\n" +
                    "Via: SIP/2.0/TCP user1pc.domain.com;branch=z9hG4bK776sgdkse\r\n" +
                    "Max-Forwards: 70\r\n" +
                    "From: sip:user1@domain.com;tag=49583\r\n" +
                    "To: sip:user2@domain.com\r\n" +
                    "Call-ID: asd88asd77a@1.2.3.4\r\n" +
                    "CSeq: 1 MESSAGE\r\n" +
                    "Content-Type: text/plain\r\n" +
                    "Content-Length: 18\r\n"
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

            Assert.AreEqual(1, receiverTransmitter.MessagesSent.Count, "Not all messages were sent" );
        }

    }
}
