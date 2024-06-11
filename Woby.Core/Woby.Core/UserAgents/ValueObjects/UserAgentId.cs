using Woby.Core.CommonLanguage.Signaling.Routings;

namespace Woby.Core.Signaling.UserAgents.ValueObjects
{
    /// <summary>
    /// Represents a user agent by the following SIP URI scheme not including right side domain
    /// see - https://datatracker.ietf.org/doc/html/rfc3261#section-8.1.1.2
    /// </summary>
    public class UserAgentId
    {
        public required string Id { get; init; }
        public string? Tag { get; init; }

        public UserAgentId()
        {
        }

        public static UserAgentId CreateUserAgentIdFromRoute(Route route) => new UserAgentId
        {
            Id = route.User,
            Tag = route.Tag
        };
    }
}
