using FluentResults;
using Woby.Core.Commons.Errors;
using Woby.Core.Signaling.UserAgents.ValueObjects;
using Woby.Core.Signaling.UserAgents.Errors;
using Woby.Core.UserAgents;

namespace Woby.Core.Signaling.UserAgents.Repository
{
    internal class InMemoryUserAgentRepository : IUserAgentsRepository
    {

        private static readonly IDictionary<UserAgentId, UserAgent> _userAgents = new Dictionary<UserAgentId, UserAgent>();

        public Result<UserAgent> GetUserAgent(UserAgentId id)
        {
            if (!_userAgents.TryGetValue(id, out var userAgent))
                return UserAgentErrors.UserWasNotFound(id);

            return userAgent;
        }

        public Result PersistUserAgent(UserAgent userAgent)
        {
            _userAgents.TryAdd(userAgent.Id, userAgent);

            return Result.Ok();
        }
    }
}
