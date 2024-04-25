using FluentResults;
using Microsoft.Extensions.Logging;
using System.Text;
using Woby.Core.CommonLanguage.Messages;
using Woby.Core.Signaling.Sip.Messages;
using Woby.Core.Utils.Rfc;

namespace Woby.Core.Signaling.Sip.Builder
{
    public class SipBuilder
    {
        private readonly ILogger<SipBuilder> _logger;
        private readonly bool _compact;

        public string Name { get; set; } = nameof(SipBuilder);

        public SipBuilder(bool compact, ILogger<SipBuilder> logger)
        {
            _logger = logger;
            _compact = compact;
        }

        public Result<Stream> Build(MessageBase message)
        {

            var signaling = message.Signaling;
            StringBuilder builder = new StringBuilder();

            builder
                .Append(SipMethods.To.Key)
                .AppendRoute(signaling.To)
                .Append(SyntaxHelper.Primitives.Crlf)

                .Append(SipMethods.From.Key)
                .AppendRoute(signaling.From)
                .Append(SyntaxHelper.Primitives.Crlf);

            foreach (var proxy in signaling.Proxies)
            {
                builder
                    .Append(SipMethods.Via.Key)
                    .AppendRoute(proxy)
                    .Append(SyntaxHelper.Primitives.Crlf);
            }

            return new MemoryStream(
                Encoding.UTF8.GetBytes(builder.ToString()));
        }
    }
}
