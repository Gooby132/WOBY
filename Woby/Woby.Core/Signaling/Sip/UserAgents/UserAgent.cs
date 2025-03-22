using Woby.Core.Signaling.Sip.UserAgents.ValueObjects;

namespace Woby.Core.Signaling.Sip.UserAgents
{
    public class UserAgent
    {
        public UserAgentId Id { get; }

        public UserAgent(UserAgentId id)
        {
            Id = id;
        }
    }
}
