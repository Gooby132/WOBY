namespace Woby.Core.Core.Headers
{
    public abstract class HeaderBase
    {
        public HeaderType Type { get; }
        public string Key { get; }
        public string Body { get; }

        public HeaderBase(string key, string body, HeaderType type)
        {
            Key = key;
            Body = body;
            Type = type;
        }
    }
}
