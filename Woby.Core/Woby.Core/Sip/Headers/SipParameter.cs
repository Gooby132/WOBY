namespace Woby.Core.Sip.Headers
{
    public class SipParameter
    {

        public string Name { get; }
        public string Value { get; }

        public SipParameter(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public static bool operator==(SipParameter x, SipParameter y) => 
            x.Value == y.Value && 
            x.Name == y.Name;

        public static bool operator !=(SipParameter x, SipParameter y) => !(x == y);

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

            return obj is SipParameter && this == (SipParameter)obj;
        }

        public override int GetHashCode() => Name.GetHashCode() ^ Value.GetHashCode();
    }
}
