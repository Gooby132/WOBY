using FluentResults;
using Microsoft.Extensions.Logging;
using System.Text;
using Woby.Core.CommonLanguage.Messages;
using Woby.Core.Commons.Errors;
using Woby.Core.Utils.Rfc;
using Woby.Sip.Signaling.Sip.Commons;
using Woby.Sip.Signaling.Sip.Converters;

namespace Woby.Core.Signaling.Sip.Builder
{
    public class SipBuilder
    {
        public readonly string SipVersionAndProtocol = "SIP/2.0";

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

            // add request line
            var method = signaling.Role.ToSipMethods();

            if (method is null)
            {
                _logger.LogWarning("{this} message role - '{message}' is not supported by SIP",
                    this, signaling.Role);

                return Result.Fail(new NotImplementedErrorBase(1, string.Format("{0} message role - '{1}' is not supported by SIP", this, signaling.Role)));
            }

            builder
                .Append(method.Name)
                .Append(" ")
                .Append(signaling.To.Uri.ToString())
                .Append(" ")
                .Append(SipVersionAndProtocol)
                .Append(SyntaxHelper.Primitives.Crlf);

            // add proxies
            foreach (var proxy in signaling.Proxies)
            {
                builder
                    .Append(SipHeaderMethods.Via.Key)
                    .Append(": ")
                    .Append(proxy.Protocol)
                    .Append(' ')
                    .AppendRoute(proxy);

                if (proxy.HasAdditinalMetadata())
                    builder.AppendMetadata(proxy.AdditinalMetadata);

                builder.Append(SyntaxHelper.Primitives.Crlf);
            }

            // add forwards
            if (signaling.MaxForwardings is not null)
                builder
                    .Append(SipHeaderMethods.MaxForwards.Key)
                    .Append(": ")
                    .Append(signaling.MaxForwardings.MaxForwards)
                    .Append(SyntaxHelper.Primitives.Crlf);

            // add from
            builder
                .Append(SipHeaderMethods.From.Key)
                .Append(": ")
                .AppendRoute(signaling.From);

            if (signaling.From.HasAdditinalMetadata())
                builder.AppendMetadata(signaling.From.AdditinalMetadata);

            builder
                .Append(SyntaxHelper.Primitives.Crlf);

            // add to
            builder
                .Append(SipHeaderMethods.To.Key)
                .Append(": ")
                .AppendRoute(signaling.To);

            if (signaling.To.HasAdditinalMetadata())
                builder.AppendMetadata(signaling.To.AdditinalMetadata);

            builder
                .Append(SyntaxHelper.Primitives.Crlf);

            // add call-id
            builder
                .Append(SipHeaderMethods.CallId.Key)
                .Append(": ")
                .Append(signaling.NegitiationId.Id)
                .Append(SyntaxHelper.Primitives.Crlf);

            // add sequence
            builder
                .Append(SipHeaderMethods.CSeq.Key)
                .Append(": ")
                .Append(signaling.Sequence.Sequence)
                .Append(' ')
                .Append(signaling.Sequence.Method)
                .Append(SyntaxHelper.Primitives.Crlf);

            // add content type
            if (signaling.ContentType is not null)
                builder
                    .Append(SipHeaderMethods.ContentType.Key)
                    .Append(": ")
                    .Append(signaling.ContentType.Content.ToString())
                    .Append(SyntaxHelper.Primitives.Crlf);

            // add content length
            if (signaling.ContentLength is not null)
                builder
                    .Append(SipHeaderMethods.ContentLength.Key)
                    .Append(": ")
                    .Append(signaling.ContentLength.Length)
                    .Append(SyntaxHelper.Primitives.Crlf);

            return new MemoryStream(
                Encoding.UTF8.GetBytes(builder.ToString()));
        }
    }
}
