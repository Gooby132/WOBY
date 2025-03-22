using System.Diagnostics.CodeAnalysis;
using System.Net;
using Woby.Core.Signaling.Sip.Headers;

namespace Woby.Core.Signaling.Sip.Parsers.Utils
{
    public static class HeaderFieldsUtils
    {

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

            var sections = headerCompleteValue.Split((char)0x32);

            if (sections.Length != 3)
                return false;

            method = sections[0]; // first section of request line

            if (!Uri.TryCreate(sections[1], UriKind.RelativeOrAbsolute, out uri))
                return false;

            protocol = sections[3];

            return true;
        }

    }
}
