namespace Woby.Core.CommonLanguage.Messages;

public static class MessageFactory
{

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

    public static Trying Trying(SignalingSection signaling) => new Trying()
    {
        Signaling = signaling,
        Content = null,
    };

    public static Ringing Ringing(SignalingSection signaling) => new Ringing()
    {
        Signaling = signaling,
        Content = null,
    };

}
