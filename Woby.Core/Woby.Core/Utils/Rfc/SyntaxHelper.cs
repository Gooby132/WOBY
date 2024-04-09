using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Woby.Core.Core.Headers;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;

namespace Woby.Core.Utils.Rfc
{
    /// <summary>
    ///    The syntax as given in this section defines the legal syntax of
    ///    Internet messages.Messages that are conformant to this standard
    ///    MUST conform to the syntax in this section.If there are options in
    ///    this section where one option SHOULD be generated, that is indicated
    ///    either in the prose or in a comment next to the syntax.
    /// </summary>
    public static class SyntaxHelper
    {
        public static class Primitives
        {
            public static readonly char Quote = (char)34;
            public static readonly char LessThan = '<';
            public static readonly char GreaterThan = '>';
            public static readonly Regex Numbers = new Regex("[1-9]");
            public static readonly Regex Rfc2822TextPatten = new Regex("^[\x01-\x09\x0B\x0C\x0E-\x7F]*$");
            public static readonly Regex Rfc2822SpecialsPatten = new Regex("[\\(\\)\\<\\>\\[\\]\\:\\;\\@\\\\\\,\\.\\\"\"]");
        }

        public static class Atoms
        {
            public static readonly Regex AText = new Regex("[A-Za-z0-9!#$%&'*+\\-/=?^_`{|}~]");
        }

        public static class AddressSpecifications
        {
            public static bool TryParseAngleAddr(string body, [NotNullWhen(true)] out string? address)
            {
                address = null;

                var trimmed = body.Trim(); // remove [CFWS]

                // validate
                if (!trimmed.StartsWith(Primitives.LessThan) || !trimmed.EndsWith(Primitives.GreaterThan))
                    return false;

                address = trimmed.Substring(1, trimmed.Length - 2);

                return true;
            }

            public static bool TryParseNameAddr(string body, out string? displayName, [NotNullWhen(true)] out string? address)
            {
                displayName = null;
                address = null;

                string[] sections = body.Split(' ');

                if (sections.Length == 0)
                    return false;

                if (sections.Length == 1)
                    return TryParseAngleAddr(sections[0], out address);

                displayName = string.Join(' ', sections[0..(sections.Length - 1)]);
                return TryParseAngleAddr(sections[sections.Length - 1], out address);
            }
        }

    }
}
