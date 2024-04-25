using FluentResults;
using Microsoft.Extensions.Logging;
using Woby.Core.CommonLanguage.Signaling.Core;
using Woby.Core.CommonLanguage.Signaling.Routings;
using Woby.Core.Core.Headers.Identities;
using Woby.Core.Network.Core;
using Woby.Core.Signaling.Sip.Headers;
using Woby.Core.Signaling.Sip.Messages;
using static Woby.Core.Utils.Rfc.SyntaxHelper;

namespace Woby.Core.Signaling.Sip.Converters
{

    /// <summary>
    /// Sip Route Header is specified for use for the following headers ; 
    /// - When the header field value contains a display name , the URI including all URI parameters is enclosed in "<" and ">"
    /// - If no "<" and ">" are present, all parameters after the URI are header parameters, not URI parameters.
    /// - The display name can be tokens, or a quoted string, if a larger character set is desired.
    /// - Even if the "display-name" is empty, the "name-addr" form MUST be used if the "addr-spec" contains a comma, semicolon, or question mark.
    /// - There may or may not be LWS between the display-name and the "<".
    /// 
    /// These rules for parsing a display name, URI and URI parameters, and header parameters also apply for the header fields To and From.
    /// 
    /// </summary>
    public class SipConverter
    {

        public static readonly char[] Whitespaces = [' ', '\t', '\n', '\r'];
        private readonly ILogger<SipConverter> _logger;

        public SipConverter(ILogger<SipConverter> logger)
        {
            _logger = logger;
        }

        public Result<SignalingHeader> Parse(SipHeader sipHeader)
        {
            SignalingHeader? temp = null;
            var type = SipHeaderType.GetType(sipHeader.Key);

            type
                .When([SipHeaderType.From, SipHeaderType.To, SipHeaderType.Contact])
                .Then(() =>
                {
                    // Address Specification ; https://datatracker.ietf.org/doc/html/rfc2822#section-3.4
                    // using name-addr
                    if (!AddressSpecifications.TryParseNameAddr(sipHeader.Body, out var displayName, out var address))
                    {
                        _logger.LogWarning("{this} failed to parse method - '{method}' with name-addr rfc 2822 parsing method", this, sipHeader.Key);
                        return;
                    }

                    RouteRole role = RouteRole.NotSet;
                    type
                        .When(SipHeaderType.From)
                        .Then(() => role = RouteRole.Sender)
                        .When(SipHeaderType.To)
                        .Then(() => role = RouteRole.Recipient);

                    if (Uri.TryCreate(address, UriKind.RelativeOrAbsolute, out var uri))
                    {
                        temp = new Route(
                            uri,
                            role,
                            sipHeader.Key,
                            sipHeader.Body,
                            displayName: displayName);
                    }
                })
                .When([SipHeaderType.CallId])
                .Then(() =>
                {
                    // no parsing required
                    temp = new DialogId(sipHeader.Key, sipHeader.Body);
                })
                .When([SipHeaderType.CSeq])
                .Then(() =>
                {
                    string[] sections = sipHeader.Body.Split(Whitespaces, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                    if (sections.Length != 2)
                    {
                        _logger.LogWarning("{this} cSeq received does not follow the correct format: <sequence> <method>", this);
                        return;
                    }

                    if (!uint.TryParse(sections[0], out var sequence))
                        return;

                    temp = new SequenceHeader(sipHeader.Key, sequence, sections[1], sipHeader.Body);
                })
                .When([SipHeaderType.MaxForwards])
                .Then(() =>
                {

                    string[] sections = sipHeader.Body.Split(Whitespaces, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                    if (sections.Length != 1)
                    {
                        _logger.LogWarning("{this} cSeq received does not follow the correct format: <sequence> <method>", this);
                        return;
                    }

                    if (!uint.TryParse(sections[0], out var maxForwarding))
                        return;

                    // no parsing required
                    temp = new MaxForwardings(sipHeader.Key, maxForwarding, sipHeader.Body);
                })
                .When([SipHeaderType.Via])
                .Then(() =>
                {
                    int index;
                    string? protocolSection = null;
                    string? addressSection = null;

                    if ((index = sipHeader.Body.IndexOf(NetworkProtocol.Udp.Name)) != -1)
                    {
                        protocolSection = sipHeader.Body.Substring(0, index + NetworkProtocol.Udp.CharacterLength);
                        addressSection = sipHeader.Body.Substring(index + NetworkProtocol.Udp.CharacterLength).Trim();
                    }

                    if (index == -1 && (index = sipHeader.Body.IndexOf(NetworkProtocol.Tcp.Name)) != -1)
                    {
                        protocolSection = sipHeader.Body.Substring(0, index + NetworkProtocol.Tcp.CharacterLength);
                        addressSection = sipHeader.Body.Substring(index + NetworkProtocol.Tcp.CharacterLength).Trim();
                    }

                    if (index == -1 && (index = sipHeader.Body.IndexOf(NetworkProtocol.Tls.Name)) != -1)
                    {
                        protocolSection = sipHeader.Body.Substring(0, index + NetworkProtocol.Tls.CharacterLength);
                        addressSection = sipHeader.Body.Substring(index + NetworkProtocol.Tls.CharacterLength).Trim();
                    }

                    if (index == -1 && (index = sipHeader.Body.IndexOf(NetworkProtocol.Sctp.Name)) != -1)
                    {
                        protocolSection = sipHeader.Body.Substring(0, index + NetworkProtocol.Sctp.CharacterLength);
                        addressSection = sipHeader.Body.Substring(index + NetworkProtocol.Sctp.CharacterLength).Trim();
                    }

                    if (index <= 0)
                    {
                        _logger.LogWarning("{this} 'Via' received does not follow the correct format: <protocol> <uri>", this);
                        return;
                    }

                    if (string.IsNullOrEmpty(addressSection))
                    {
                        _logger.LogWarning("{this} 'Via' received does not follow the correct format: <protocol> <uri>", this);
                        return;
                    }

                    // uri as 172.0.0.0: 5060 - is valid by book (TODO: add support)

                    if (Uri.TryCreate(addressSection, UriKind.RelativeOrAbsolute, out var uri))
                    {
                        temp = new Route(
                            uri,
                            RouteRole.Proxy,
                            sipHeader.Key,
                            sipHeader.Body,
                            protocol: protocolSection);
                    }

                });

            return temp is not null ? Result.Ok(temp) : Result.Fail("Failed to parse header");
        }

    }
}
