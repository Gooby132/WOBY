using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace Woby.Core.CommonLanguage.Signaling.Core
{
    public abstract class SignalingHeader
    {
        public HeaderType Type { get; }
        public string Key { get; }
        public string Body { get; }
        public IImmutableDictionary<string, string>? AdditinalMetadata { get; }

        public SignalingHeader(string key, string body, HeaderType type, IImmutableDictionary<string, string>? additinalMetadata = null)
        {
            Key = key;
            Body = body;
            Type = type;
            AdditinalMetadata = additinalMetadata;
        }

        [MemberNotNullWhen(true, nameof(AdditinalMetadata))]
        public bool HasAdditinalMetadata() => AdditinalMetadata is not null && AdditinalMetadata.Any();

        public static bool operator ==(SignalingHeader left, SignalingHeader right) =>
            left.Type == right.Type &&
            left.Key == right.Key &&
            left.Body == right.Body;

        public static bool operator !=(SignalingHeader left, SignalingHeader right) => !(left == right);

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            return obj is SignalingHeader && this == (SignalingHeader)obj;
        }

        public override int GetHashCode() => GetHashCode();
    }
}
