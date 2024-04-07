namespace Woby.Core.Core.Headers
{
    public abstract class HeaderBase
    {
        public HeaderTypes Type { get; }
        public string Key { get; }
        public string Body { get; }

        public HeaderBase(string key, string body, HeaderTypes type)
        {
            Key = key;
            Body = body;
            Type = type;
        }
    }
}
