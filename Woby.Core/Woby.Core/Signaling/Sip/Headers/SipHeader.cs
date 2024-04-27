using System.Diagnostics.CodeAnalysis;
using Woby.Core.CommonLanguage.Signaling.Core;

namespace Woby.Core.Signaling.Sip.Headers
{
    public class SipHeader : SignalingHeader
    {

        public IEnumerable<SipParameter>? Parameters { get; } // Additional parameters for appended to the sip header

        public SipHeader(string name, string value, HeaderType type, IEnumerable<SipParameter>? parameters = null) : base(name, value, type)
        {
            Parameters = parameters;
        }

        public static bool operator ==(SipHeader left, SipHeader right)
        {

            if (left.Type != right.Type ||
                left.Body != right.Body ||
                left.Key != right.Key)
                return false;

            if (left.Parameters is null && right.Parameters is null)
                return true;

            if (left.Parameters is not null && right.Parameters is null)
                return false;

            if (left.Parameters is null && right.Parameters is not null)
                return false;

            if (left!.Parameters!.Count() != right!.Parameters!.Count())
                return false;

            return !left!.Parameters!.Any(lp => right!.Parameters!.FirstOrDefault(rp => rp == lp) is null);

        }

        public static bool operator !=(SipHeader left, SipHeader right) => !(left == right);

        [MemberNotNullWhen(true, nameof(Parameters))]
        public bool HasParamerters() => Parameters is not null && Parameters.Any();

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

            return obj is SipHeader && this == (SipHeader)obj;
        }

        public override int GetHashCode() => Key.GetHashCode() ^ Body.GetHashCode();
    }
}
