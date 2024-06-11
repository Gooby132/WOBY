using FluentResults;
using Microsoft.Extensions.Logging;
using System.Text;
using Woby.Core.Abstractions;
using Woby.Core.Network.Abstractions;

namespace Woby.Core.Network.Core
{

    /// <summary>
    /// InMemoryChannel used for testing porpuses
    /// </summary>
    public class InMemoryChannel : IChannel
    {

        #region Fields
        
        private readonly ILogger<InMemoryChannel> _logger;

        #endregion

        #region Constructor

        public InMemoryChannel(ILogger<InMemoryChannel> logger)
        {
            _logger = logger;
        }

        public required string Message { get; init; }

        public List<string> MessagesSent { get; init; } = new();

        #endregion

        #region Methods

        public Result Subscribe(IChannelListener listener)
        {
            if (string.IsNullOrEmpty(Message))
                return Result.Fail("Message was not loaded"); // this error is valid for testing porpuses

            _logger.LogTrace("{this} new channel listener request to subscribe. transmiting message.", 
                this);

            listener.ReceiveMessage(new MemoryStream(Encoding.UTF8.GetBytes(Message))).Wait(5_000);

            return Result.Ok();
        }

        public async Task<Result> Transmit(Stream message)
        {
            using StreamReader stream = new StreamReader(message);
            
            MessagesSent.Add(stream.ReadToEnd());
            
            _logger.LogTrace("{this} client tried to transmit the message - '{message}'", this, MessagesSent.LastOrDefault());

            return Result.Ok();
        }

        public Result Unsubscribe(IChannelListener defaultSaga)
        {
            return Result.Ok();
        }

        public override string ToString() => nameof(InMemoryChannel);

        #endregion

    }
}
