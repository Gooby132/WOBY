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

        public static UserAgentNotFound UserAgentNotFound(SignalingSection signaling) => new UserAgentNotFound()
        {
            Signaling = signaling,
            Content = null,
        };
    }

    public class NoMessage : MessageBase { }

    public class EndOfTransaction : MessageBase { }

    public class UserAgentNotFound: MessageBase { }
}
