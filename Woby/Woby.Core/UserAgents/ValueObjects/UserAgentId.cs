using FluentResults;
using Woby.Core.CommonLanguage.Signaling.Routings;
using Woby.Core.Signaling.UserAgents.Errors;

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

        public static Result<UserAgentId> CreateUserAgentIdFromRoute(Route route)
        {
            if (string.IsNullOrWhiteSpace(route.User))
                return Result.Fail(UserAgentErrors.UserAgentIdIsInvalid());
         
            return new UserAgentId() { 
                Id = route.User,
                Tag = route.Tag
            };
        }

        public override int GetHashCode() => Id.GetHashCode();

        public override bool Equals(object obj)
        {
            UserAgentId? other = obj as UserAgentId;
            if (other == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Id == other.Id;
        }

    }
}
