using System.Diagnostics.CodeAnalysis;
using Woby.Core.Sip.Headers;

namespace Woby.Core.Sip.Parsers.Utils
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

    }
}
