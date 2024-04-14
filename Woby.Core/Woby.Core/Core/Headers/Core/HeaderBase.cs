using System.Collections.Immutable;

namespace Woby.Core.Core.Headers.Core
{
    public abstract class HeaderBase
    {
        public HeaderType Type { get; }
        public string Key { get; }
        public string Body { get; }
        public IImmutableDictionary<string, string>? AdditinalMetadata { get; }

        public HeaderBase(string key, string body, HeaderType type, IImmutableDictionary<string, string>? additinalMetadata = null)
        {
            Key = key;
            Body = body;
            Type = type;
            AdditinalMetadata = additinalMetadata;
        }

        public static bool operator== (HeaderBase left, HeaderBase right) => 
            left.Type == right.Type &&
            left.Key == right.Key &&
            left.Body == right.Body;

        public static bool operator !=(HeaderBase left, HeaderBase right) => !(left == right);

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

            return obj is HeaderBase && this == (HeaderBase)obj;
        }

        public override int GetHashCode() => GetHashCode();
    }
}
