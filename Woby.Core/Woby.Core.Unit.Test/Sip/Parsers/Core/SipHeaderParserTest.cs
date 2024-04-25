using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Woby.Core.CommonLanguage.Signaling.Core;
using Woby.Core.Signaling.Sip.Headers;
using Woby.Core.Signaling.Sip.Parsers.Core;

namespace Woby.Core.Unit.Test.Sip.Parsers.Core
{
    [TestClass]
    public class SipHeaderParserTest
    {
        private ServiceProvider _provider;

        [TestInitialize] 
        public void TestInitialize()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddLogging(conf => conf.AddSerilog());
            services.AddTransient<SipSignalingHeaderParser>();

            _provider = services.BuildServiceProvider();
        }

        [TestMethod]
        public void ParseSipHeaders_GeneralFrom_Successful()
        {
            var parser = _provider.GetRequiredService<SipSignalingHeaderParser>();

            string test1 = "From: \"Alice\" sip:alice@example.com;tag=12345";
            string test2 = "From: sip:alice@example.com;tag=54321";
            string test3 = "From: sip:alice@example.com;tag=abcde";
            string test4 = "From: \"Alice\" sip:alice@example.com;tag=67890";
            string test5 = "From: \"Alice\" sip:alice@example.com;tag=xyz987";

            var header1 = parser.ParseSingleHeader(test1);
            var header2 = parser.ParseSingleHeader(test2);
            var header3 = parser.ParseSingleHeader(test3);
            var header4 = parser.ParseSingleHeader(test4);
            var header5 = parser.ParseSingleHeader(test5);

            SipHeader expectedHeader1 = new SipHeader("From", "\"Alice\" sip:alice@example.com", HeaderType.Unknown, new List<SipParameter>
            {
                new SipParameter("tag", "12345")
            });

            SipHeader expectedHeader2 = new SipHeader("From", "sip:alice@example.com", HeaderType.Unknown, new List<SipParameter>
            {
                new SipParameter("tag", "54321")
            });

            SipHeader expectedHeader3 = new SipHeader("From", "sip:alice@example.com", HeaderType.Unknown, new List<SipParameter>
            {
                new SipParameter("tag", "abcde")
            });

            SipHeader expectedHeader4 = new SipHeader("From", "\"Alice\" sip:alice@example.com", HeaderType.Unknown, new List<SipParameter>
            {
                new SipParameter("tag", "67890")
            });

            SipHeader expectedHeader5 = new SipHeader("From", "\"Alice\" sip:alice@example.com", HeaderType.Unknown, new List<SipParameter>
            {
                new SipParameter("tag", "xyz987")
            });

            Assert.IsTrue(header1.IsSuccess);
            Assert.IsTrue(header2.IsSuccess);
            Assert.IsTrue(header3.IsSuccess);
            Assert.IsTrue(header4.IsSuccess);
            Assert.IsTrue(header5.IsSuccess);

            Assert.IsTrue(expectedHeader1 == header1.Value);
            Assert.IsTrue(expectedHeader2 == header2.Value);
            Assert.IsTrue(expectedHeader3 == header3.Value);
            Assert.IsTrue(expectedHeader4 == header4.Value);
            Assert.IsTrue(expectedHeader5 == header5.Value);
        }

        [TestMethod]
        public void ParseSipHeaders_GeneralTo_Successful()
        {
            var parser = _provider.GetRequiredService<SipSignalingHeaderParser>();

            string test1 = "To: \"Bob\" sip:bob@example.net";
            string test2 = "To: sip:bob@example.net;tag=98765";
            string test3 = "To: sip:bob@example.net;tag=zyxwv";
            string test4 = "To: \"Bob\" sip:bob@example.net;tag=54321";
            string test5 = "To: \"Bob\" sip:bob@example.net;tag=78901";

            var header1 = parser.ParseSingleHeader(test1);
            var header2 = parser.ParseSingleHeader(test2);
            var header3 = parser.ParseSingleHeader(test3);
            var header4 = parser.ParseSingleHeader(test4);
            var header5 = parser.ParseSingleHeader(test5);

            SipHeader expectedHeader1 = new SipHeader("To", "\"Bob\" sip:bob@example.net", HeaderType.Unknown);

            SipHeader expectedHeader2 = new SipHeader("To", "sip:bob@example.net", HeaderType.Unknown, new List<SipParameter>
            {
                new SipParameter("tag", "98765")
            });

            SipHeader expectedHeader3 = new SipHeader("To", "sip:bob@example.net", HeaderType.Unknown, new List<SipParameter>
            {
                new SipParameter("tag", "zyxwv")
            });

            SipHeader expectedHeader4 = new SipHeader("To", "\"Bob\" sip:bob@example.net", HeaderType.Unknown, new List<SipParameter>
            {
                new SipParameter("tag", "54321")
            });

            SipHeader expectedHeader5 = new SipHeader("To", "\"Bob\" sip:bob@example.net", HeaderType.Unknown, new List<SipParameter>
            {
                new SipParameter("tag", "78901")
            });

            Assert.IsTrue(header1.IsSuccess);
            Assert.IsTrue(header2.IsSuccess);
            Assert.IsTrue(header3.IsSuccess);
            Assert.IsTrue(header4.IsSuccess);
            Assert.IsTrue(header5.IsSuccess);

            Assert.IsTrue(expectedHeader1 == header1.Value);
            Assert.IsTrue(expectedHeader2 == header2.Value);
            Assert.IsTrue(expectedHeader3 == header3.Value);
            Assert.IsTrue(expectedHeader4 == header4.Value);
            Assert.IsTrue(expectedHeader5 == header5.Value);
        }

        [TestMethod]
        public void ParseSipHeaders_GeneralVia_Successful()
        {
            var parser = _provider.GetRequiredService<SipSignalingHeaderParser>();

            string test1 = "Via: SIP/2.0/UDP 192.0.2.1:5060;branch=z9hG4bK874h87";
            string test2 = "Via: SIP/2.0/UDP 198.51.100.1:5060;branch=z9hG4bKaa76d";
            string test3 = "Via: SIP/2.0/UDP 203.0.113.1:5060;branch=z9hG4bKabcd34";
            string test4 = "Via: SIP/2.0/TCP 192.0.2.50:5060;branch=z9hG4bKkjiu32";
            string test5 = "Via: SIP/2.0/TLS 198.51.100.20:5061;branch=z9hG4bKqwert1";

            var header1 = parser.ParseSingleHeader(test1);
            var header2 = parser.ParseSingleHeader(test2);
            var header3 = parser.ParseSingleHeader(test3);
            var header4 = parser.ParseSingleHeader(test4);
            var header5 = parser.ParseSingleHeader(test5);

            SipHeader expectedHeader1 = new SipHeader("Via", "SIP/2.0/UDP 192.0.2.1:5060", HeaderType.Unknown, new List<SipParameter>
            {
                new SipParameter("branch", "z9hG4bK874h87")
            });

            SipHeader expectedHeader2 = new SipHeader("Via", "SIP/2.0/UDP 198.51.100.1:5060", HeaderType.Unknown, new List<SipParameter>
            {
                new SipParameter("branch", "z9hG4bKaa76d")
            });

            SipHeader expectedHeader3 = new SipHeader("Via", "SIP/2.0/UDP 203.0.113.1:5060", HeaderType.Unknown, new List<SipParameter>
            {
                new SipParameter("branch", "z9hG4bKabcd34")
            });

            SipHeader expectedHeader4 = new SipHeader("Via", "SIP/2.0/TCP 192.0.2.50:5060", HeaderType.Unknown, new List<SipParameter>
            {
                new SipParameter("branch", "z9hG4bKkjiu32")
            });

            SipHeader expectedHeader5 = new SipHeader("Via", "SIP/2.0/TLS 198.51.100.20:5061", HeaderType.Unknown, new List<SipParameter>
            {
                new SipParameter("branch", "z9hG4bKqwert1")
            });

            Assert.IsTrue(header1.IsSuccess);
            Assert.IsTrue(header2.IsSuccess);
            Assert.IsTrue(header3.IsSuccess);
            Assert.IsTrue(header4.IsSuccess);
            Assert.IsTrue(header5.IsSuccess);

            Assert.IsTrue(expectedHeader1 == header1.Value);
            Assert.IsTrue(expectedHeader2 == header2.Value);
            Assert.IsTrue(expectedHeader3 == header3.Value);
            Assert.IsTrue(expectedHeader4 == header4.Value);
            Assert.IsTrue(expectedHeader5 == header5.Value);
        }

        // TODO : add all variations of common sip headers

    }
}
