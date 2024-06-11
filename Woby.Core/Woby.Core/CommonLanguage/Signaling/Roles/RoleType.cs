using Ardalis.SmartEnum;

namespace Woby.Core.CommonLanguage.Signaling.Roles
{
    public class RoleType : SmartEnum<RoleType>
    {

        public readonly static RoleType None = new RoleType("None", 0, false);

        // dialog creation request
        public readonly static RoleType DialogCreationRequest = new RoleType("dialog creation request", 1, true);
        // dialog creation available responses
        public readonly static RoleType DialogCreationRequestAccepted = new RoleType("dialog creation request accepted", 2, false);
        public readonly static RoleType DialogCreationRequestDeclined = new RoleType("dialog creation request declined", 3, false);
        public readonly static RoleType DialogCreationRequestIgnored = new RoleType("dialog creation request ignored", 4, false);

        // notify incoming call request and available responses
        public readonly static RoleType NotifyIncomingCallRequestIgnored = new RoleType("dialog creation request ignored", 4, true);
        // notify incoming call available responses
        public readonly static RoleType NotifyIncomingCallRequestNotified = new RoleType("dialog creation request ignored", 4, false);
        public readonly static RoleType NotifyIncomingCallRequestNotSupported = new RoleType("dialog creation request ignored", 4, false);

        // general responses
        public readonly static RoleType Acknolaged = new RoleType("dialog creation request ignored", 4, false);
        public readonly static RoleType Accepted = new RoleType("dialog creation request ignored", 4, false);
        public readonly static RoleType Declined = new RoleType("dialog creation request ignored", 4, false);

        private RoleType(string name, int value, bool isRequestOrResponse) : base(name, value)
        {
            IsRequestOrResponse = isRequestOrResponse;
        }

        public bool IsRequestOrResponse { get; private set; }
    }
}
