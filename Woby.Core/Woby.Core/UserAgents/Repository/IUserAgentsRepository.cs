using FluentResults;
using Woby.Core.Signaling.UserAgents.ValueObjects;

namespace Woby.Core.Signaling.UserAgents.Repository
{
    public interface IUserAgentsRepository
    {

        public Result<UserAgent> GetUserAgent(UserAgentId id);

        public Result PersistUserAgent(UserAgent userAgent);

    }
}
