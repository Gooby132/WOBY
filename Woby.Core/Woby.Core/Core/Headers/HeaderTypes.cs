using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woby.Core.Core.Headers
{
    public class HeaderTypes : SmartEnum<HeaderTypes>
    {

        public static readonly HeaderTypes Unknown = new HeaderTypes("Unknown", 0);
        public static readonly HeaderTypes Route = new HeaderTypes("Route", 1);

        private HeaderTypes(string name, int value) : base(name, value)
        {
        }
    }
}
