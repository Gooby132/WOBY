using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woby.Core.Core.Headers
{
    public class HeaderType : SmartEnum<HeaderType>
    {

        public static readonly HeaderType Unknown = new HeaderType("Unknown", 0);
        public static readonly HeaderType Route = new HeaderType("Route", 1);

        private HeaderType(string name, int value) : base(name, value)
        {
        }
    }
}
