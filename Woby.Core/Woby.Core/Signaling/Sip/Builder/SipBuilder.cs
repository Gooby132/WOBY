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

        public string Name { get; set; } = nameof(SipBuilder);

        public SipBuilder(ILogger<SipBuilder> logger)
        {
            _logger = logger;
        }

        public Result<Stream> Build(MessageBase message)
        {

            var signaling = message.Signaling;
            StringBuilder builder = new StringBuilder();

            // add proxies
            foreach (var proxy in signaling.Proxies)
            {
                builder
                    .Append(SipMethods.Via.Key)
                    .Append(": ")
                    .Append(proxy.Protocol)
                    .Append(' ')
                    .AppendRoute(proxy);

                if (proxy.HasAdditinalMetadata())
                    builder.AppendMetadata(proxy.AdditinalMetadata);

                builder.Append(SyntaxHelper.Primitives.Crlf);
            }

            // add forwards
            builder
                .Append(SipMethods.MaxForwards.Key)
                .Append(": ")
                .Append(signaling.MaxForwardings.MaxForwards)
                .Append(SyntaxHelper.Primitives.Crlf);

            // add from
            builder
                .Append(SipMethods.From.Key)
                .Append(": ")
                .AppendRoute(signaling.From);

            if (signaling.From.HasAdditinalMetadata())
                builder.AppendMetadata(signaling.From.AdditinalMetadata);

            builder
                .Append(SyntaxHelper.Primitives.Crlf);

            // add to
            builder
                .Append(SipMethods.To.Key)
                .Append(": ")
                .AppendRoute(signaling.To);

            if (signaling.To.HasAdditinalMetadata())
                builder.AppendMetadata(signaling.To.AdditinalMetadata);

            builder
                .Append(SyntaxHelper.Primitives.Crlf);

            // add call-id
            builder
                .Append(SipMethods.CallId.Key)
                .Append(": ")
                .Append(signaling.DialogId.Id)
                .Append(SyntaxHelper.Primitives.Crlf);

            // add sequence
            builder
                .Append(SipMethods.CSeq.Key)
                .Append(": ")
                .Append(signaling.Sequence.Sequence)
                .Append(' ')
                .Append(signaling.Sequence.Method)
                .Append(SyntaxHelper.Primitives.Crlf);

            // add content type
            builder
                .Append(SipMethods.ContentType.Key)
                .Append(": ")
                .Append(signaling.ContentType.Content.ToString())
                .Append(SyntaxHelper.Primitives.Crlf);

            // add content length
            builder
                .Append(SipMethods.ContentLength.Key)
                .Append(": ")
                .Append(signaling.ContentLength.Length)
                .Append(SyntaxHelper.Primitives.Crlf);

            return new MemoryStream(
                Encoding.UTF8.GetBytes(builder.ToString()));
        }
    }
}
