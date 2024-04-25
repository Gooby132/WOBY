using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Woby.Core.Signaling.Sip.Builder;

namespace Woby.Core.Unit.Test.Sip.Converters
{
    [TestClass]
    public class SipBuilderTests
    {
        private ServiceProvider _provider;
        private SipBuilder _builder;

        [TestInitialize]
        public void TestInitialize()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddLogging(conf => conf.AddSerilog());
            services.AddTransient<SipBuilder>();

            _provider = services.BuildServiceProvider();
            _builder = _provider.GetRequiredService<SipBuilder>();
        }

        [TestMethod]
        public void TestBuildingFromMessageBase()
        {

        }

    }
}
