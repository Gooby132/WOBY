using FluentResults;
using Microsoft.Extensions.Logging;
using System.Text;
using Woby.Core.Abstractions;
using Woby.Core.CommonLanguage.Messages;
using Woby.Core.Commons.Errors;
using Woby.Core.Utils.Rfc;
using Woby.Sip.Signaling.Sip.Commons;
using Woby.Sip.Signaling.Sip.Converters;

namespace Woby.Core.Signaling.Sip.Builder
{
    public class SipBuilder : IBuilder
    {
        public readonly string SipVersionAndProtocol = "SIP/2.0";

        private readonly ILogger<SipBuilder> _logger;

        public string Name { get; set; } = nameof(SipBuilder);

        public SipBuilder(ILogger<SipBuilder> logger)
        {
            _logger = logger;
        }

        public Task<Result<Stream>> Build(MessageBase message)
        {

            var signaling = message.Signaling;
            StringBuilder builder = new StringBuilder();

            // add request line
            var method = signaling.Role.ToSipMethods();

            if (method is null)
            {
                _logger.LogWarning("{this} message role - '{message}' is not supported by SIP",
                    this, signaling.Role);

                return Task.FromResult(
                    Result.Fail<Stream>(new NotImplementedErrorBase(1, string.Format("{0} message role - '{1}' is not supported by SIP", this, signaling.Role)))
                    );
            }

            builder
                .Append(method.Name)
                .Append(" ")
                .AppendNameAddr(signaling.To)
                .Append(" ")
                .Append(SipVersionAndProtocol)
                .Append(SyntaxHelper.Primitives.Crlf);

            // add proxies
            foreach (var proxy in signaling.Proxies)
                builder.AppendVia(proxy);

            // add forwards
            if (signaling.MaxForwardings is not null)
                builder
                    .Append(SipHeaderMethod.MaxForwards.Key)
                    .Append(": ")
                    .Append(signaling.MaxForwardings.MaxForwards)
                    .Append(SyntaxHelper.Primitives.Crlf);

            // add from
            builder
                .AppendContact(signaling.From, SipHeaderMethod.From);

            // add to
            builder
                .AppendContact(signaling.To, SipHeaderMethod.To);

            // add call-id
            builder
                .AppendHeader(SipHeaderMethod.CallId.Key, signaling.NegitiationId.Id);

            // add sequence
            builder
                .Append(SipHeaderMethod.CSeq.Key)
                .Append(": ")
                .Append(signaling.Sequence.Sequence)
                .Append(' ')
                .Append(signaling.Sequence.Method)
                .Append(SyntaxHelper.Primitives.Crlf);

            // add content type
            if (signaling.ContentType is not null)
                builder
                    .Append(SipHeaderMethod.ContentType.Key)
                    .Append(": ")
                    .Append(signaling.ContentType.Content.ToString())
                    .Append(SyntaxHelper.Primitives.Crlf);

            // add content length
            if (signaling.ContentLength is not null)
                builder
                    .Append(SipHeaderMethod.ContentLength.Key)
                    .Append(": ")
                    .Append(signaling.ContentLength.Length)
                    .Append(SyntaxHelper.Primitives.Crlf);

            return Task.FromResult(
               Result.Ok<Stream>(new MemoryStream(Encoding.UTF8.GetBytes(builder.ToString()))));
        }

        public Task<Result<Stream>> BuildResponse(ResponseBase message, SipMessageMethod method)
        {
            var signaling = message.Signaling;
            StringBuilder builder = new StringBuilder();
            builder
                .Append(SipVersionAndProtocol)
                .Append(' ')
                .Append(method.Value)
                .Append(' ')
                .Append(method.Name)
                .Append(SyntaxHelper.Primitives.Crlf);

            return Task.FromResult(
                Result.Ok<Stream>(new MemoryStream(Encoding.UTF8.GetBytes(builder.ToString()))));
        }

        public Task<Result<Stream>> BuildUserAgentWasNotFound(MessageBase message)
        {
            var signaling = message.Signaling;
            StringBuilder builder = new StringBuilder();

            builder
                .Append(SipVersionAndProtocol)
                .Append(' ')
                .Append(SipMessageMethod.NotFound.Value)
                .Append(' ')
                .Append(SipMessageMethod.NotFound.Name)
                .Append(SyntaxHelper.Primitives.Crlf);

            foreach (var proxy in signaling.Proxies)
                builder.AppendVia(proxy);

            // add from
            builder
                .AppendContact(message.Signaling.From, SipHeaderMethod.From);

            // add to
            builder
                .AppendContact(message.Signaling.To, SipHeaderMethod.To);

            // add call-id
            builder
                .AppendHeader(SipHeaderMethod.CallId.Key, signaling.NegitiationId.Id);

            // add sequence
            builder
                .Append(SipHeaderMethod.CSeq.Key)
                .Append(": ")
                .Append(signaling.Sequence.Sequence)
                .Append(' ')
                .Append(signaling.Sequence.Method)
                .Append(SyntaxHelper.Primitives.Crlf);

            // add content length
            if (signaling.ContentLength is not null)
                builder
                    .Append(SipHeaderMethod.ContentLength.Key)
                    .Append(": ")
                    .Append(0)
                    .Append(SyntaxHelper.Primitives.Crlf);

            return Task.FromResult(
                Result.Ok<Stream>(new MemoryStream(Encoding.UTF8.GetBytes(builder.ToString()))));
        }

        public Task<Result<Stream>> BuildTrying(MessageBase message)
        {
            var signaling = message.Signaling;
            StringBuilder builder = new StringBuilder();

            builder
                .Append(SipVersionAndProtocol)
                .Append(' ')
                .Append(SipMessageMethod.Trying.Value)
                .Append(' ')
                .Append(SipMessageMethod.Trying.Name)
                .Append(SyntaxHelper.Primitives.Crlf);

            foreach (var proxy in signaling.Proxies)
                builder.AppendVia(proxy);

            // add from
            builder
                .AppendContact(message.Signaling.From, SipHeaderMethod.From);

            // add to
            builder
                .AppendContact(message.Signaling.To, SipHeaderMethod.To);

            // add call-id
            builder
                .AppendHeader(SipHeaderMethod.CallId.Key, signaling.NegitiationId.Id);

            // add sequence
            builder
                .Append(SipHeaderMethod.CSeq.Key)
                .Append(": ")
                .Append(signaling.Sequence.Sequence)
                .Append(' ')
                .Append(signaling.Sequence.Method)
                .Append(SyntaxHelper.Primitives.Crlf);

            // add content length
            builder
                .Append(SipHeaderMethod.ContentLength.Key)
                .Append(": ")
                .Append(0)
                .Append(SyntaxHelper.Primitives.Crlf);

            return Task.FromResult(
                Result.Ok<Stream>(new MemoryStream(Encoding.UTF8.GetBytes(builder.ToString()))));
        }

        public Task<Result<Stream>> BuildRinging(MessageBase message)
        {
            var signaling = message.Signaling;
            StringBuilder builder = new StringBuilder();

            builder
                .Append(SipVersionAndProtocol)
                .Append(' ')
                .Append(SipMessageMethod.Ringing.Value)
                .Append(' ')
                .Append(SipMessageMethod.Ringing.Name)
                .Append(SyntaxHelper.Primitives.Crlf);

            foreach (var proxy in signaling.Proxies)
                builder.AppendVia(proxy);

            // add from
            builder
                .AppendContact(message.Signaling.From, SipHeaderMethod.From);

            // add to
            builder
                .AppendContact(message.Signaling.To, SipHeaderMethod.To);

            // add call-id
            builder
                .AppendHeader(SipHeaderMethod.CallId.Key, signaling.NegitiationId.Id);

            // add sequence
            builder
                .Append(SipHeaderMethod.CSeq.Key)
                .Append(": ")
                .Append(signaling.Sequence.Sequence)
                .Append(' ')
                .Append(signaling.Sequence.Method)
                .Append(SyntaxHelper.Primitives.Crlf);

            // add content length
            if (signaling.ContentLength is not null)
                builder
                    .Append(SipHeaderMethod.ContentLength.Key)
                    .Append(": ")
                    .Append(0)
                    .Append(SyntaxHelper.Primitives.Crlf);

            return Task.FromResult(
                Result.Ok<Stream>(new MemoryStream(Encoding.UTF8.GetBytes(builder.ToString()))));
        }
    }
}
