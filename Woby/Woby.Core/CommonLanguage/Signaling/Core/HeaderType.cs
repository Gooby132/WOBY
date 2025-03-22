using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woby.Core.CommonLanguage.Signaling.Core
{
    public class HeaderType : SmartEnum<HeaderType>
    {

        public static readonly HeaderType Unknown = new HeaderType("Unknown", 0);
        public static readonly HeaderType Routing = new HeaderType("Routing", 1);
        public static readonly HeaderType Linguistics = new HeaderType("Linguistics", 2);
        public static readonly HeaderType ContentMetadata = new HeaderType("ContentMetadata", 3);
        public static readonly HeaderType Identity = new HeaderType("Identity", 3);

        private HeaderType(string name, int value) : base(name, value)
        {
        }
    }
}
