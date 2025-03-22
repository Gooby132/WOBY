using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Woby.Core.CommonLanguage.Signaling.Core;
using Woby.Core.CommonLanguage.Signaling.Routings;
using Woby.Core.Network.Core;
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

        static object[][] ValidFromData => new object[][] {
                [
                    "From: \"Bob\" <sip:bob@example.net>",
                    new SipHeader("From", "\"Bob\" <sip:bob@example.net>", HeaderType.Unknown),
                    new Route(RouteRole.Sender, "From", "\"Bob\" <sip:bob@example.net>", "Bob", "sip", "bob", null, "example.net", null, NetworkProtocol.Unknown)
                ],
                [
                    "From: sip:bob@example.net;tag=98765",
                    new SipHeader("From", "sip:bob@example.net", HeaderType.Unknown, new List<SipParameter> { new SipParameter("tag", "98765") }),
                    new Route(RouteRole.Sender, "From", "sip:bob@example.net", null, "sip", "bob", null, "example.net", null, NetworkProtocol.Unknown)
                ],
                [
                    "From: sip:bob:222@example.net;tag=zyxwv",
                    new SipHeader("From", "sip:bob:222@example.net", HeaderType.Unknown, new List<SipParameter> { new SipParameter("tag", "zyxwv") }),
                    new Route(RouteRole.Sender, "From", "\"Bob\" sip:bob:222@example.net", null, "sip", "bob", "222", "example.net", null, NetworkProtocol.Unknown)
                ],
                [
                    "From: \"Bob\" <sip:bob:222@example.net;tag=54321>",
                    new SipHeader("From", "\"Bob\" <sip:bob:222@example.net;tag=54321>", HeaderType.Unknown, new List<SipParameter> { new SipParameter("tag", "54321") }),
                    new Route(RouteRole.Sender, "From", "\"Bob\" <sip:bob:222@example.net;tag=54321>", "Bob", "sip", "bob", "222", "example.net", null, NetworkProtocol.Unknown)
                ],
                [
                    "From: \"Bob\" <sip:bob@example.net;tag=78901>",
                    new SipHeader("From", "\"Bob\" <sip:bob@example.net;tag=78901>", HeaderType.Unknown, new List<SipParameter> { new SipParameter("tag", "78901") }),
                    new Route(RouteRole.Sender, "From", "\"Bob\" <sip:bob@example.net;tag=78901>", "Bob", "sip", "bob", null, "example.net", null, NetworkProtocol.Unknown)
                ]
            };

        static object[][] ValidToData => new object[][] {
                [
                    "To: \"Bob\" sip:bob@example.net",
                    new SipHeader("To", "\"Bob\" sip:bob@example.net", HeaderType.Unknown)
                ],
                [
                    "To: sip:bob@example.net;tag=98765",
                    new SipHeader("To", "sip:bob@example.net", HeaderType.Unknown, new List<SipParameter> { new SipParameter("tag", "98765") })
                ],
                [
                    "To: sip:bob@example.net;tag=zyxwv",
                    new SipHeader("To", "sip:bob@example.net", HeaderType.Unknown, new List<SipParameter> { new SipParameter("tag", "zyxwv") })
                ],
                [
                    "To: \"Bob\" sip:bob@example.net;tag=54321",
                    new SipHeader("To", "\"Bob\" sip:bob@example.net", HeaderType.Unknown, new List<SipParameter> { new SipParameter("tag", "54321") })
                ],
                [
                    "To: \"Bob\" sip:bob@example.net;tag=78901",
                    new SipHeader("To", "\"Bob\" sip:bob@example.net", HeaderType.Unknown, new List<SipParameter> { new SipParameter("tag", "78901") })
                ]
            };

        [TestMethod]
        [DynamicData(nameof(ValidFromData))]
        public void ParseSpecializedHeader_From_Successful(
            string headerAsString,
            SipHeader expectedHeader,
            Route expectedRoute)
        {
            var header1 = _headerParser.ParseSingleHeader(headerAsString);
            var route = _specializedParser.ConvertHeader(
                header1.Value,
                new NetworkMetadata
                {
                    NetworkProtocol = NetworkProtocol.Unknown,
                    ReceviedOn = new System.Net.IPEndPoint(0, 0),
                });

            Assert.IsTrue(header1.IsSuccess);
            Assert.AreEqual(header1.Value, expectedHeader, "Header are not equal");
            Assert.IsTrue(route.IsSuccess, "Could not parse specialized header");
            Assert.AreEqual(route.Value, expectedRoute, "Route are not equal");
        }

        [TestMethod]
        [DynamicData(nameof(ValidToData))]
        public void ParseSpecializedHeader_To_Successful(
            string headerAsString,
            SipHeader expectedHeader,
            Route expectedRoute)
        {
            var header1 = _headerParser.ParseSingleHeader(headerAsString);
            var route = _specializedParser.ConvertHeader(
                header1.Value,
                new NetworkMetadata
                {
                    NetworkProtocol = NetworkProtocol.Unknown,
                    ReceviedOn = new System.Net.IPEndPoint(0, 0),
                });

            Assert.IsTrue(header1.IsSuccess);
            Assert.AreEqual(header1.Value, expectedHeader, "Header are not equal");
            Assert.IsTrue(route.IsSuccess, "Could not parse specialized header");
            Assert.AreEqual(route.Value, expectedRoute, "Route are not equal");
        }

    }
}
