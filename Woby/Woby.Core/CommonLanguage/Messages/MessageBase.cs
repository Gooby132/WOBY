namespace Woby.Core.CommonLanguage.Messages
{
    public class MessageBase
    {

        public required SignalingSection Signaling { get; init; }
        public required ContentSection? Content { get; init; }
    }

    public class NoMessage : MessageBase { }

    public class EndOfTransaction : MessageBase { }

    public class UserAgentNotFound : MessageBase { }

    public class Trying : MessageBase { }

    public class Ringing : MessageBase { }

}
