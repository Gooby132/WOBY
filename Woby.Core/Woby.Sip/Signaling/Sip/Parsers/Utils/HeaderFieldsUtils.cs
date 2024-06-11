using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Security.Cryptography.Xml;
using System.Text.RegularExpressions;
using Woby.Core.Network.Core;
using Woby.Core.Signaling.Sip.Headers;

namespace Woby.Core.Signaling.Sip.Parsers.Utils
{
    public static class HeaderFieldsUtils
    {

        public static readonly Regex SipContactUriPattern =
            new Regex(
                @"^(?:(sip|sips):)?(?:([^:@]+)(?::([^@]*))?@)?([^:;?]+)(?::(\d+))?(;[^?]+)?(?:\?(.*))?$",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static readonly Regex SipViaUriPattern =
            new Regex(
                @"^SIP/2\.0/(?<transport>\w+)\s+(?<host>[^:;]+)(:(?<port>\d+))?(;(?<params>.*))?$",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static readonly string SipProtocol = "SIP/2";
        public static readonly int ParameterNameIndex = 0;
        public static readonly int ParameterValueIndex = 1;
        public static readonly char ParameterSeperator = ';';
        public static readonly char ParameterKeyValueSeperator = '=';

        public static bool TryParseParameterField(string value, [NotNullWhen(true)] out SipParameter? parameter)
        {
            parameter = null;

            // validate (super basic)
            if (!value.Contains(ParameterKeyValueSeperator))
                return false;

            var sections = value.Split(ParameterKeyValueSeperator, 2, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            parameter = new SipParameter(
                sections[ParameterNameIndex],
                sections[ParameterValueIndex]);

            return true;
        }

        public static bool TryParseHeaderParameters(string headerCompleteValue, out string headerValue, out IEnumerable<SipParameter>? sipParameters)
        {
            sipParameters = null;
            var temp = new List<SipParameter>();

            var sections = headerCompleteValue.Split(ParameterSeperator, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            foreach (var section in sections[1..])
            {
                if (!TryParseParameterField(section, out var parameter))
                    continue; // TODO : TBD

                temp.Add(parameter);
            }

            headerValue = sections[0];

            sipParameters = temp.Any() ? temp : null; // keeping sip parameters null if non found

            return true;
        }
        public static bool TryParseRequestLine(
            string headerCompleteValue,
            [NotNullWhen(true)] out string? method,
            [NotNullWhen(true)] out Uri? uri,
            [NotNullWhen(true)] out string? protocol)
        {
            method = null;
            uri = null;
            protocol = null;

            var sections = headerCompleteValue.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (sections.Length != 3)
                return false;

            method = sections[0]; // first section of request line

            if (!Uri.TryCreate(sections[1], UriKind.RelativeOrAbsolute, out uri))
                return false;

            protocol = sections[2];

            return true;
        }


        public static bool TryParseContactUri(
            string uri,
            [NotNullWhen(true)] out string? scheme,
            [NotNullWhen(true)] out string? user,
            out string? password,
            [NotNullWhen(true)] out string? host,
            out int? port,
            [NotNullWhen(true)] out IEnumerable<SipParameter>? parameters,
            [NotNullWhen(true)] out IEnumerable<SipParameter>? headers)
        {
            scheme = null;
            user = null;
            password = null;
            host = null;
            port = null;
            parameters = null;
            headers = null;

            var match = SipContactUriPattern.Match(uri);

            if (!match.Success)
            {
                return false;
            }

            scheme = !string.IsNullOrEmpty(match.Groups[1].Value) ? match.Groups[1].Value : null;

            user = !string.IsNullOrEmpty(match.Groups[2].Value) ? match.Groups[2].Value : null;
            password = !string.IsNullOrEmpty(match.Groups[3].Value) ? match.Groups[3].Value : null;
            host = match.Groups[4].Value;
            string portAsString = match.Groups[5].Value;
            string parametersAsString = match.Groups[6].Value;
            string headersAsString = match.Groups[7].Value;

            if (string.IsNullOrEmpty(scheme))
            {
                scheme = null;
                user = null;
                password = null;
                host = null;
                port = null;
                parameters = null;
                headers = null;
                return false;
            }

            if (string.IsNullOrEmpty(user))
            {
                scheme = null;
                user = null;
                password = null;
                host = null;
                port = null;
                parameters = null;
                headers = null;
                return false;
            }

            if (!string.IsNullOrEmpty(portAsString))
            {
                if (!int.TryParse(portAsString, out var possiblePort))
                {
                    scheme = null;
                    user = null;
                    password = null;
                    host = null;
                    port = null;
                    parameters = null;
                    headers = null;
                    return false;
                }

                port = possiblePort;
            }

            if (!string.IsNullOrEmpty(parametersAsString))
            {
                if (TryParseHeaderParameters(parametersAsString, out _, out parameters))
                {
                    scheme = null;
                    user = null;
                    password = null;
                    host = null;
                    port = null;
                    parameters = null;
                    headers = null;
                    return false;
                }
            }

            return true;
        }

        public static bool TryParseViaUri(
            string header,
            [NotNullWhen(true)] out string? host,
            out int? port,
            [NotNullWhen(true)] out string? protocol,
            out NetworkProtocol networkProtocol,
            out IEnumerable<SipParameter>? parameters)
        {
            host = null;
            port = null;
            protocol= null;
            networkProtocol = NetworkProtocol.Unknown;
            parameters = null;
            
            var match = SipViaUriPattern.Match(header);

            if (!match.Success)
                return false;

            host = match.Groups["host"].Value;
            protocol = SipProtocol;
            string transport = match.Groups["transport"].Value;
            string portAsString = match.Groups["port"].Value;
            string parametersAsString = match.Groups["parameters"].Value;

            parameters = null; // TODO: add parameters support

            if (string.IsNullOrEmpty(host))
                return false;

            if (string.IsNullOrEmpty(transport))
                return false;

            if (!NetworkProtocol.TryFromName(transport, out networkProtocol))
                networkProtocol = NetworkProtocol.Unknown;

            if (!string.IsNullOrEmpty(portAsString))
                if (int.TryParse(portAsString, out var parsedPort))
                    port = parsedPort;
                else return false;

            return true;
        }
    }
}
