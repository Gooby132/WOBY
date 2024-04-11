﻿using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Woby.Core.Sip.Parsers.Core;
using Woby.Core.Sip.Parsers.SpecializedHeaderParsers;

namespace Woby.Core.Unit.Test.Sip.Parsers.SpecializedHeaderParser
{
    [TestClass]
    public class SipRouteHeaderParserTest
    {

        private ServiceProvider _provider;

        [TestInitialize]
        public void TestInitialize()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddLogging(conf => conf.AddSerilog());
            services.AddTransient<SipHeaderParser>();
            services.AddTransient<SipHeaderParser>();

            _provider = services.BuildServiceProvider();
        }

        [TestMethod]
        public void ParseSimpleHeader_Successful()
        {
            SipRouteHeaderParser parser = new SipRouteHeaderParser();

            string test1 = "From: \"Alice\" sip:alice@example.com;tag=12345";
            string test2 = "From: sip:alice@example.com;tag=54321";
            string test3 = "From: sip:alice@example.com;tag=abcde";
            string test4 = "From: \"Alice\" sip:alice@example.com;tag=67890";
            string test5 = "From: \"Alice\" sip:alice@example.com;tag=xyz987";


        }

    }
}
