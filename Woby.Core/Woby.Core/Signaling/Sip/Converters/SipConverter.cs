using FluentResults;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Collections.Immutable;
using Woby.Core.CommonLanguage.Messages;
using Woby.Core.CommonLanguage.Signaling.ContentMeta;
using Woby.Core.CommonLanguage.Signaling.Core;
using Woby.Core.CommonLanguage.Signaling.Identities;
using Woby.Core.CommonLanguage.Signaling.Routings;
using Woby.Core.Network.Core;
using Woby.Core.Signaling.Sip.Headers;
using Woby.Core.Signaling.Sip.Messages;
using Woby.Core.Signaling.Sip.Parsers.Core;
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

        public Result<SignalingSection> Parse(IEnumerable<SipHeader> sipHeaders)
        {
            List<SignalingHeader> coreHeaders = new List<SignalingHeader>();
            DialogId? id = null;
            SequenceHeader? sequence = null;
            MaxForwardings? maxForwardings = null;
            Route? to = null;
            Route? from = null;
            List<Route> proxies = new List<Route>();
            ContentType? contentType = null;
            ContentLength? contentLength = null;

            foreach (var header in sipHeaders)
            {
                var coreHeader = Parse(header);

                if (coreHeader.IsFailed)
                {
                    _logger.LogWarning("{this} failed to parse sip header. errors(s) - '{errors}'", this, coreHeader.Reasons.Select(r => r.Message));
                    continue;
                }

                if (id is null && coreHeader.Value is DialogId dialogId)
                {
                    id = dialogId;
                    continue;
                }

                if (to is null && coreHeader.Value is Route toRoute)
                {
                    to = toRoute.Role == RouteRole.Recipient ? toRoute : null;
                }

                if (from is null && coreHeader.Value is Route fromRoute)
                {
                    from = fromRoute.Role == RouteRole.Sender ? fromRoute : null;
                }

                if (coreHeader.Value is Route proxyRoute && proxyRoute.Role == RouteRole.Proxy)
                {
                    proxies.Add(proxyRoute);
                    continue;
                }

                if (maxForwardings is null && coreHeader.Value is MaxForwardings maxForwardingsRoute)
                {
                    maxForwardings = maxForwardingsRoute;
                    continue;
                }

                if (sequence is null && coreHeader.Value is SequenceHeader sequenceHeader)
                {
                    sequence = sequenceHeader;
                    continue;
                }

                if (contentLength is null && coreHeader.Value is ContentLength contentLengthHeader)
                {
                    contentLength = contentLengthHeader;
                    continue;
                }

                if (contentType is null && coreHeader.Value is ContentType contentTypeHeader)
                {
                    contentType = contentTypeHeader;
                    continue;
                }

                _logger.LogWarning("{this} failed to parse header - '{header}'", this, coreHeader.Errors.Select(r => r.Message));
            }

            if (id is null)
                return Result.Fail(SipCoreErrors.MissingCallId());

            if (to is null)
                return Result.Fail(SipCoreErrors.MissingTo());

            if (from is null)
                return Result.Fail(SipCoreErrors.MissingFrom());

            if (sequence is null)
                return Result.Fail(SipCoreErrors.MissingCSeq());

            if (maxForwardings is null)
                return Result.Fail(SipCoreErrors.MissingMaxForward());

            if (contentType is null)
                return Result.Fail(SipCoreErrors.MissingContentType());

            if (contentLength is null)
                return Result.Fail(SipCoreErrors.MissingContentLength());

            return Result.Ok(
                new SignalingSection(
                    id,
                    sequence,
                    maxForwardings,
                    to,
                    from,
                    proxies.ToImmutableList(),
                    contentType,
                    contentLength));
        }

        public Result<SignalingHeader> Parse(SipHeader sipHeader)
        {
            SignalingHeader? temp = null;
            var type = SipMethods.GetType(sipHeader.Key);

            type
                .When([SipMethods.From, SipMethods.To, SipMethods.Contact])
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
                        .When(SipMethods.From)
                        .Then(() => role = RouteRole.Sender)
                        .When(SipMethods.To)
                        .Then(() => role = RouteRole.Recipient);

                    if (Uri.TryCreate(address, UriKind.RelativeOrAbsolute, out var uri))
                    {
                        Dictionary<string, string>? d = null;

                        if (sipHeader.HasParamerters())
                        {
                            d = new Dictionary<string, string>();

                            foreach (var p in sipHeader.Parameters)
                                d.Add(p.Name, p.Value);
                        }

                        temp = new Route(
                            uri,
                            role,
                            sipHeader.Key,
                            sipHeader.Body,
                            displayName: displayName,
                            additinalMetadata: d?.ToImmutableDictionary());
                    }
                })
                .When([SipMethods.CallId])
                .Then(() =>
                {
                    // no parsing required
                    temp = new DialogId(sipHeader.Key, sipHeader.Body);
                })
                .When([SipMethods.CSeq])
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
                .When([SipMethods.MaxForwards])
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
                .When([SipMethods.Via])
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
                        Dictionary<string, string>? d = null;

                        if (sipHeader.HasParamerters())
                        {
                            d = new Dictionary<string, string>();

                            foreach (var p in sipHeader.Parameters)
                                d.Add(p.Name, p.Value);
                        }

                        temp = new Route(
                            uri,
                            RouteRole.Proxy,
                            sipHeader.Key,
                            sipHeader.Body,
                            protocol: protocolSection,
                            additinalMetadata: d?.ToImmutableDictionary());
                    }

                })
                .When([SipMethods.ContentType])
                .Then(() =>
                {
                    System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType(sipHeader.Body);

                    temp = new ContentType(sipHeader.Key, sipHeader.Body, contentType);
                })
                .When([SipMethods.ContentLength])
                .Then(() =>
                {
                    if (!uint.TryParse(sipHeader.Body, out var length))
                        return;

                    temp = new ContentLength(sipHeader.Key, length);
                });

            return temp is not null ? Result.Ok(temp) : Result.Fail("Failed to parse header");
        }

    }
}
