using FluentResults;
using Woby.Core.Core.Errors.Common;
using Woby.Core.Core.Messages;
using Woby.Core.Network;

namespace Woby.Core.Sip.Parsers.Core
{
    public class SipCoreParser
    {
        public const int RequestUriMethodFieldIndex = 0;

        public const int CSeqSequenceFieldIndex = 0;
        public const int CSeqMethodFieldIndex = 1;

        public const string SimpleInviteHeaderSection = @"
INVITE sip:+14155552222@example.pstn.twilio.com SIP/2.0
Via: SIP/2.0/UDP 192.168.10.10:5060;branch=z9hG4bK776asdhds
Max-Forwards: 70
To: ""Bob"" <sip:+14155552222@example.pstn.twilio.com>
From: ""Alice"" <sip:+14155551111@example.pstn.twilio.com>;tag=1
Call-ID: a84b4c76e66710
CSeq: 1 INVITE
";

        public Result<MessageBase> Parse(string message, NetworkProtocols protocol)
        {
            var errors = new List<BaseError>();

            using var reader = new StringReader(message);

            string? requestUri = reader.ReadLine();

            if (string.IsNullOrEmpty(requestUri))
            {
                errors.Add(SipCoreErrors.MissingRequestUri());
                return Result.Fail(errors);
            }

            string? via = reader.ReadLine();

            if (string.IsNullOrEmpty(via))
            {
                errors.Add(SipCoreErrors.MissingVia());
                return Result.Fail(errors);
            }

            string? maxForward = reader.ReadLine();
            if (string.IsNullOrEmpty(maxForward))
            {
                errors.Add(SipCoreErrors.MissingMaxForward());
                return Result.Fail(errors);
            }

            if(int.TryParse(maxForward, out var maxForwardValue))
            {
                errors.Add(SipCoreErrors.CSeqCouldNotBeParsed());
                return Result.Fail(errors);
            }

            string? to = reader.ReadLine();
            if (string.IsNullOrEmpty(to))
            {
                errors.Add(SipCoreErrors.MissingTo());
                return Result.Fail(errors);
            }

            string? from = reader.ReadLine();
            if (string.IsNullOrEmpty(from))
            {
                errors.Add(SipCoreErrors.MissingFrom());
                return Result.Fail(errors);
            }

            string? callId = reader.ReadLine();
            if (string.IsNullOrEmpty(callId))
            {
                errors.Add(SipCoreErrors.MissingCallId());
                return Result.Fail(errors);
            }

            string? cSeq = reader.ReadLine();
            if (string.IsNullOrEmpty(cSeq))
            {
                errors.Add(SipCoreErrors.MissingCSeq());
                return Result.Fail(errors);
            }

            var cSeqFields = cSeq.Split(" ");

            if (!int.TryParse(cSeqFields[0], out _))
            {
                errors.Add(SipCoreErrors.CSeqCouldNotBeParsed());
                return Result.Fail(errors);
            }

            return new Invite(to, from, cSeq, callId, maxForwardValue, via);
        }
    }
}
