using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Text;
using Woby.Core.Signaling.Sip.Builder;
using Woby.Core.Signaling.Sip.Converters;
using Woby.Core.Signaling.Sip.Headers;
using Woby.Core.Signaling.Sip.Parsers.Core;

namespace Woby.Core.Integration.Test.Sip.ParserBuilder
{
    [TestClass]
    public class ParserBuilderEquality
    {
        private ServiceProvider _provider;

        [TestInitialize]
        public void TestInitialize()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddLogging(conf => conf.AddSerilog());
            services.AddTransient<SipHeaderParser>();
            services.AddTransient<SipConverter>();
            services.AddTransient<SipBuilder>();

            _provider = services.BuildServiceProvider();
        }


        [TestMethod]
        public void MessageEquality_Successful()
        {
            var parser = _provider.GetRequiredService<SipHeaderParser>();
            var converter = _provider.GetRequiredService<SipConverter>();
            var builder = _provider.GetRequiredService<SipBuilder>();

            string sipMessage =
                "Via: SIP/2.0/TCP user1pc.domain.com;branch=z9hG4bK776sgdkse\r\n" +
                "Max-Forwards: 70\r\n" +
                "From: sip:user1@domain.com;tag=49583\r\n" +
                "To: sip:user2@domain.com\r\n" +
                "Call-ID: asd88asd77a@1.2.3.4\r\n" +
                "CSeq: 1 MESSAGE\r\n" +
                "Content-Type: text/plain\r\n" +
                "Content-Length: 18\r\n";

            // parse into intemiddiate language

            var parsedMessage = parser.Parse(sipMessage);

            // convert into common language

            var common = converter.Parse(
                parsedMessage
                    .Value
                    .Headers);

            Assert.IsTrue(common.IsSuccess, "common language compilation failed");

            // build into encoded stream

            var encodedSip = builder.Build(new CommonLanguage.Messages.MessageBase(common.Value));

            Assert.IsTrue(encodedSip.IsSuccess, "could not encode sip message");

            var messageAsString = new StreamReader(encodedSip.Value).ReadToEnd();

            Assert.AreEqual(sipMessage, messageAsString, "messages are not equal");

        }

    }
}
