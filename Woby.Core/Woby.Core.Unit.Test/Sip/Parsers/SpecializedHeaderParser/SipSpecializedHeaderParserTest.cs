using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Woby.Core.Core.Headers.Core;
using Woby.Core.Sip.Headers;
using Woby.Core.Sip.Parsers.Core;
using Woby.Core.Sip.Parsers.SpecializedHeaderParsers;

namespace Woby.Core.Unit.Test.Sip.Parsers.SpecializedHeaderParser
{
    [TestClass]
    public class SipSpecializedHeaderParserTest
    {

        private ServiceProvider _provider;
        private SipCoreHeaderParser _headerParser;
        private SipSpecializedHeaderParser _specializedParser;

        [TestInitialize]
        public void TestInitialize()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddLogging(conf => conf.AddSerilog());
            services.AddTransient<SipCoreHeaderParser>();
            services.AddTransient<SipSpecializedHeaderParser>();

            _provider = services.BuildServiceProvider();
            _headerParser = _provider.GetRequiredService<SipCoreHeaderParser>();
            _specializedParser = _provider.GetRequiredService<SipSpecializedHeaderParser>();
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


        }

        [TestMethod]
        public void ParseSpecializedHeader_From_Successful()
        {

            _headerParser.



        }



    }
}
