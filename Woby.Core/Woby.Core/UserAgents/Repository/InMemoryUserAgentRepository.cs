using FluentResults;
using Woby.Core.Commons.Errors;
using Woby.Core.Signaling.UserAgents.ValueObjects;
using Woby.Core.Signaling.UserAgents.Errors;

namespace Woby.Core.Signaling.UserAgents.Repository
{
    internal class InMemoryUserAgentRepository : IUserAgentsRepository
    {

        private readonly IDictionary<UserAgentId, UserAgent> _userAgents = new Dictionary<UserAgentId, UserAgent>();

        public Result<UserAgent> GetUserAgent(UserAgentId id)
        {
            if (!_userAgents.TryGetValue(id, out var userAgent))
                return UserAgentErrors.UserWasNotFound(id);

            return userAgent;
        }

        public Result PersistUserAgent(UserAgent userAgent)
        {
            if(!_userAgents.TryGetValue(userAgent.Id, out _))
            {
                return new NotFoundErrorBase(3, 1, "user agent was not found");
            }

            _userAgents[userAgent.Id] = userAgent;

            return Result.Ok();
        }
    }
}
