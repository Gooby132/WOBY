using FluentResults;
using Microsoft.Extensions.Logging;
using System.Reflection.PortableExecutable;
using Woby.Core.CommonLanguage.Messages;
using Woby.Core.CommonLanguage.Signaling.Core;
using Woby.Core.CommonLanguage.Signaling.Identities;
using Woby.Core.CommonLanguage.Signaling.Routings;
using Woby.Core.Network.Core;
using Woby.Core.Signaling.Sip.Converters;

namespace Woby.Core.Signaling.Sip.Parsers.Core
{
    public class SipCoreParser
    {

        #region Fields

        private readonly ILogger<SipCoreParser> _logger;
        private readonly SipSignalingHeaderParser _sipHeaderParser;
        private readonly SipConverter _sipConverter;

        #endregion

        #region Properties

        public const int RequestUriMethodFieldIndex = 0;

        public const int CSeqSequenceFieldIndex = 0;
        public const int CSeqMethodFieldIndex = 1;

        public string Name { get; set; } = nameof(SipCoreParser);

        #endregion

        #region Constructor
        public SipCoreParser(
            ILogger<SipCoreParser> logger,
            SipSignalingHeaderParser sipHeaderParser,
            SipConverter sipSpecializedHeaderParser)
        {
            _logger = logger;
            _sipHeaderParser = sipHeaderParser;
            _sipConverter = sipSpecializedHeaderParser;
        }

        #endregion

        public Result<MessageBase> Parse(string message, NetworkProtocol protocol)
        {

            // it is importent to consider folding stracture headers
            var parts = message.Split("\r\n\r\n");
            var sipHeaders = _sipHeaderParser.Parse(parts[0]);

            if (sipHeaders.IsFailed)
            {
                // add log

                return Result.Fail(sipHeaders.Errors);
            }

            // log header warnings
            foreach (var header in sipHeaders.Value)
            {
                if (header.IsFailed)
                {
                    _logger.LogWarning("{this} failed to convert header. error - '{errors}'",
                        this, string.Join(", ", header.Reasons.Select(r => r.Message)));
                }
            }

            var section = _sipConverter.Parse(
                sipHeaders
                    .Value
                    .Where(header => header.IsSuccess)
                    .Select(header => header.Value));

            if (section.IsFailed)
            {
                _logger.LogWarning("{this} failed to convert section. error - '{errors}'",
                    this, string.Join(", ", section.Reasons.Select(r => r.Message)));
            }

            return Result.Ok(new MessageBase(section.Value));
        }

        public override string ToString() => Name;
    }
}
