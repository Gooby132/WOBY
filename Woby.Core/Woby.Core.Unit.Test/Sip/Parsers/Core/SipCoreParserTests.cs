using Microsoft.Extensions.DependencyInjection;
using Woby.Core.Network;
using Woby.Core.Sip.Parsers.Core;
using Serilog;
using Woby.Core.Sip.Converters;

namespace Woby.Core.Unit.Test.Sip.Parsers.Core
{
    [TestClass]
    public class SipCoreParserTests
    {

        public const string SimpleInviteHeaderSection = @"Via: SIP/2.0/UDP 192.168.10.10:5060;branch=z9hG4bK776asdhds
Max-Forwards: 70
To: ""Bob"" <sip:+14155552222@example.pstn.twilio.com>
From: ""Alice"" <sip:+14155551111@example.pstn.twilio.com>;tag=1
Call-ID: a84b4c76e66710
CSeq: 1 INVITE
";
        private ServiceProvider _provider;

        [TestInitialize]
        public void Container()
        {
            var collection = new ServiceCollection();

            collection.AddLogging(conf => conf.AddSerilog());
            collection.AddTransient<SipCoreParser>();
            collection.AddTransient<SipCoreHeaderParser>();
            collection.AddTransient<SipConverter>();

            _provider = collection.BuildServiceProvider();
        }

        [TestMethod]
        public void Validate_InsureAllMustHeadersArePresent_Successful()
        {
            var parser = _provider.GetRequiredService<SipCoreParser>();

            var message = parser.Parse(SimpleInviteHeaderSection, NetworkProtocol.Tcp);

            Assert.IsTrue(message.IsSuccess);
        }

    }
}
