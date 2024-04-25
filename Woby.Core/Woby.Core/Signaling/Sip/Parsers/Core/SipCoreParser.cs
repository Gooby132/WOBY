using FluentResults;
using Microsoft.Extensions.Logging;
using System.Text;
using Woby.Core.CommonLanguage.Messages;
using Woby.Core.CommonLanguage.Signaling.Core;
using Woby.Core.CommonLanguage.Signaling.Identities;
using Woby.Core.CommonLanguage.Signaling.Routings;
using Woby.Core.Core.Errors.Common;
using Woby.Core.Core.Headers;
using Woby.Core.Network.Core;
using Woby.Core.Signaling.Sip.Converters;

namespace Woby.Core.Signaling.Sip.Parsers.Core
{
    public class SipCoreParser
    {

        #region Fields

        private readonly ILogger<SipCoreParser> _logger;
        private readonly SipSignalingHeaderParser _sipHeaderParser;
        private readonly SipConverter _sipSpecializedHeaderParser;

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
            _sipSpecializedHeaderParser = sipSpecializedHeaderParser;
        }

        #endregion

        public Result<MessageBase> Parse(string message, NetworkProtocol protocol)
        {

            #region Headers Parse

            // it is importent to consider folding stracture headers
            var parts = message.Split("\r\n\r\n");
            var sipHeaders = _sipHeaderParser.Parse(parts[0]);

            if (sipHeaders.IsFailed)
            {
                // add log

                return Result.Fail(sipHeaders.Errors);
            }

            List<SignalingHeader> coreHeaders = new List<SignalingHeader>();
            DialogId? id = null;
            SequenceHeader? sequence = null;
            Route? to = null;
            Route? from = null;
            List<Route> proxies = new List<Route>();

            foreach (var header in sipHeaders.Value)
            {
                var coreHeader = _sipSpecializedHeaderParser.Parse(header.Value);

                if (coreHeader.IsFailed)
                {
                    _logger.LogWarning("{this} failed to parse sip header. errors(s) - '{errors}'", this, header.Reasons.Select(r => r.Message));
                    continue;
                }

                if (id is null && coreHeader.Value is DialogId dialogId)
                    id = dialogId;

                if (to is null && coreHeader.Value is Route toRoute)
                    to = toRoute.Role == RouteRole.Recipient ? toRoute : null;

                if (from is null && coreHeader.Value is Route fromRoute)
                    from = fromRoute.Role == RouteRole.Sender ? fromRoute : null;

                if (coreHeader.Value is Route proxyRoute && proxyRoute.Role == RouteRole.Proxy)
                    proxies.Add(proxyRoute);

                if (sequence is null && coreHeader.Value is Route)

                    if (coreHeader.IsSuccess)
                        coreHeaders.Add(coreHeader.Value);
                    else
                        _logger.LogWarning("{this} failed to parse header - '{header}'", this, coreHeader.Errors.Select(r => r.Message));
            }

            #endregion

            return Result.Fail(SipCoreErrors.GeneralInvalidMessageError("Not yet implemented"));
        }

        public override string ToString() => Name;
    }
}
