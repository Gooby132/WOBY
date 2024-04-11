using FluentResults;
using Microsoft.Extensions.Logging;
using System.Text;
using Woby.Core.Core.Errors.Common;
using Woby.Core.Core.Headers;
using Woby.Core.Core.Messages;
using Woby.Core.Network;

namespace Woby.Core.Sip.Parsers.Core
{
    public class SipCoreParser
    {

        #region Fields

        private readonly ILogger<SipCoreParser> _logger;
        private readonly SipCoreHeaderParser _sipHeaderParser;

        #endregion

        #region Properties

        public const int RequestUriMethodFieldIndex = 0;

        public const int CSeqSequenceFieldIndex = 0;
        public const int CSeqMethodFieldIndex = 1;

        public string Name { get; set; } = nameof(SipCoreParser);

        #endregion

        #region Constructor
        public SipCoreParser(ILogger<SipCoreParser> logger, SipCoreHeaderParser sipHeaderParser)
        {
            _logger = logger;
            _sipHeaderParser = sipHeaderParser;
        }

        #endregion

        public Result<MessageBase> Parse(string message, NetworkProtocols protocol)
        {

            #region Headers Parse

            // it is importent to consider folding stracture headers
            var parts = message.Split("\r\n\r\n");
            _sipHeaderParser.Parse(parts[0]);

            #endregion

            //string? requestUri = reader.ReadLine();

            //if (string.IsNullOrEmpty(requestUri))
            //{
            //    errors.Add(SipCoreErrors.MissingRequestUri());
            //    return Result.Fail(errors);
            //}

            //string? via = reader.ReadLine();

            //if (string.IsNullOrEmpty(via))
            //{
            //    errors.Add(SipCoreErrors.MissingVia());
            //    return Result.Fail(errors);
            //}

            //string? maxForward = reader.ReadLine();
            //if (string.IsNullOrEmpty(maxForward))
            //{
            //    errors.Add(SipCoreErrors.MissingMaxForward());
            //    return Result.Fail(errors);
            //}

            //if(int.TryParse(maxForward, out var maxForwardValue))
            //{
            //    errors.Add(SipCoreErrors.CSeqCouldNotBeParsed());
            //    return Result.Fail(errors);
            //}

            //string? to = reader.ReadLine();
            //if (string.IsNullOrEmpty(to))
            //{
            //    errors.Add(SipCoreErrors.MissingTo());
            //    return Result.Fail(errors);
            //}

            //string? from = reader.ReadLine();
            //if (string.IsNullOrEmpty(from))
            //{
            //    errors.Add(SipCoreErrors.MissingFrom());
            //    return Result.Fail(errors);
            //}

            //string? callId = reader.ReadLine();
            //if (string.IsNullOrEmpty(callId))
            //{
            //    errors.Add(SipCoreErrors.MissingCallId());
            //    return Result.Fail(errors);
            //}

            //string? cSeq = reader.ReadLine();
            //if (string.IsNullOrEmpty(cSeq))
            //{
            //    errors.Add(SipCoreErrors.MissingCSeq());
            //    return Result.Fail(errors);
            //}

            //var cSeqFields = cSeq.Split(" ");

            //if (!int.TryParse(cSeqFields[0], out _))
            //{
            //    errors.Add(SipCoreErrors.CSeqCouldNotBeParsed());
            //    return Result.Fail(errors);
            //}
            return Result.Fail(SipCoreErrors.GeneralInvalidMessageError("Not yet implemented"));
        }

        public override string ToString() => Name;
    }
}
