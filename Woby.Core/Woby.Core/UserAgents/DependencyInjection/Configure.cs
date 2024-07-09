using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Woby.Core.Signaling.UserAgents.Repository;

namespace Woby.Core.UserAgents.DependencyInjection;

public static class Configure 
{

    public static IServiceCollection AddInMemoryUserAgentRepository(this IServiceCollection services)
    {
        services.AddTransient<IUserAgentsRepository, InMemoryUserAgentRepository>();

        return services;
    }

}
