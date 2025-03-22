namespace Woby.Core.Signaling.Sip.UserAgents.ValueObjects
{
    public class UserAgentId
    {
        public string Uri { get; }

        public UserAgentId(string uri)
        {
            Uri = uri;
        }
    }
}
