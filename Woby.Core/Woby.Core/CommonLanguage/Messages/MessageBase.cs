namespace Woby.Core.CommonLanguage.Messages
{
    public class MessageBase
    {

        public required SignalingSection Signaling { get; init; }
        public required ContentSection? Content { get; init; }

        public static NoMessage NoMessage(SignalingSection signaling) => new NoMessage() 
        { 
            Signaling = signaling,
            Content = null,
        };

        public static EndOfTransaction EndOfTransaction() => new EndOfTransaction()
        {
            Signaling = null!,
            Content = null,
        };
    }

    public class NoMessage : MessageBase
    {
        public NoMessage() : base() { }
    }

    public class EndOfTransaction : MessageBase
    {
        public EndOfTransaction() : base() { }
    }
}
