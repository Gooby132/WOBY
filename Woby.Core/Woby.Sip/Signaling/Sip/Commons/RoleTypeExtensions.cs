using Woby.Core.CommonLanguage.Signaling.Roles;
using Woby.Sip.Signaling.Sip.Converters;

namespace Woby.Sip.Signaling.Sip.Commons
{
    /// <summary>
    /// Role Type Extensions presents translations for SIP method types
    /// </summary>
    public static class RoleTypeExtensions
    {

        public static SipMessageMethod? ToSipMethods(this RoleType roleType, bool compressed = false)
        {
            if (roleType == RoleType.DialogCreationRequest) return SipMessageMethod.Invite;
            if (roleType == RoleType.DialogCreationRequestAccepted) return SipMessageMethod.Ok;
            if (roleType == RoleType.DialogCreationRequestDeclined) return SipMessageMethod.Bye;
            if (roleType == RoleType.NotifyIncomingCallRequestNotified) return SipMessageMethod.Ok;

            return null;
        }

        public static RoleType? FromSipMethod(SipMessageMethod method)
        {
            if (method == SipMessageMethod.Invite) return RoleType.DialogCreationRequest;
            if (method == SipMessageMethod.Ok) return RoleType.Accepted;
            if (method == SipMessageMethod.Bye) return RoleType.Declined;

            return null;
        }

    }
}
