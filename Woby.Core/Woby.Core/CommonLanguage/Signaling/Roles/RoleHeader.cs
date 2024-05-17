using System.Collections.Immutable;
using Woby.Core.CommonLanguage.Signaling.Core;

namespace Woby.Core.CommonLanguage.Signaling.Roles
{
    public class RoleHeader : SignalingHeader
    {
        public RoleType Role { get; }

        public RoleHeader(string key, string body, RoleType role, HeaderType type, IImmutableDictionary<string, string>? additinalMetadata = null) : base(key, body, type, additinalMetadata)
        {
            Role = role;
        }
    }
}
