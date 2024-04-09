namespace Woby.Core.Sip.Parsers.Headers
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
    }
}
