using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Woby.Core.CommonLanguage.Signaling.Core;
using Woby.Core.Signaling.Sip.Converters;
using Woby.Core.Signaling.Sip.Headers;
using Woby.Core.Signaling.Sip.Parsers.Core;

namespace Woby.Core.Unit.Test.Sip.Parsers.SpecializedHeaderParser
{
    [TestClass]
    public class SipSpecializedHeaderParserTest
    {

        private ServiceProvider _provider;
        private SipHeaderParser _headerParser;
        private SipConverter _specializedParser;

        [TestInitialize]
        public void TestInitialize()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddLogging(conf => conf.AddSerilog());
            services.AddTransient<SipHeaderParser>();
            services.AddTransient<SipConverter>();

            _provider = services.BuildServiceProvider();
            _headerParser = _provider.GetRequiredService<SipHeaderParser>();
            _specializedParser = _provider.GetRequiredService<SipConverter>();
        }

        [TestMethod]
        public void ParseSpecializedHeader_From_Successful()
        {
            string test1 = "To: \"Bob\" sip:bob@example.net";
            string test2 = "To: sip:bob@example.net;tag=98765";
            string test3 = "To: sip:bob@example.net;tag=zyxwv";
            string test4 = "To: \"Bob\" sip:bob@example.net;tag=54321";
            string test5 = "To: \"Bob\" sip:bob@example.net;tag=78901";

            var header1 = _headerParser.ParseSingleHeader(test1);
            var header2 = _headerParser.ParseSingleHeader(test2);
            var header3 = _headerParser.ParseSingleHeader(test3);
            var header4 = _headerParser.ParseSingleHeader(test4);
            var header5 = _headerParser.ParseSingleHeader(test5);

            var res1 = _specializedParser.Parse(header1.Value);
            var res2 = _specializedParser.Parse(header2.Value);
            var res3 = _specializedParser.Parse(header3.Value);
            var res4 = _specializedParser.Parse(header4.Value);
            var res5 = _specializedParser.Parse(header5.Value);

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

            Assert.IsTrue(res1.IsSuccess);
            Assert.IsTrue(res2.IsSuccess);
            Assert.IsTrue(res3.IsSuccess);
            Assert.IsTrue(res4.IsSuccess);
            Assert.IsTrue(res5.IsSuccess);

            Assert.IsTrue(expectedHeader1 == res1.Value);
            Assert.IsTrue(expectedHeader2 == res2.Value);
            Assert.IsTrue(expectedHeader3 == res3.Value);
            Assert.IsTrue(expectedHeader4 == res4.Value);
            Assert.IsTrue(expectedHeader5 == res5.Value);
        }

        [TestMethod]
        public void ParseSpecializedHeader_To_Successful()
        {

            string test1 = "To: \"Bob\" sip:bob@example.net";
            string test2 = "To: sip:bob@example.net;tag=98765";
            string test3 = "To: sip:bob@example.net;tag=zyxwv";
            string test4 = "To: \"Bob\" sip:bob@example.net;tag=54321";
            string test5 = "To: \"Bob\" sip:bob@example.net;tag=78901";

            var header1 = _headerParser.ParseSingleHeader(test1);
            var header2 = _headerParser.ParseSingleHeader(test2);
            var header3 = _headerParser.ParseSingleHeader(test3);
            var header4 = _headerParser.ParseSingleHeader(test4);
            var header5 = _headerParser.ParseSingleHeader(test5);

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

    }
}
