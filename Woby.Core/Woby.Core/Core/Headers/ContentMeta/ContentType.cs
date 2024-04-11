using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woby.Core.Core.Headers.Core;

namespace Woby.Core.Core.Headers.ContentMeta
{
    public class ContentType : HeaderBase
    {
        public string? CharacterSet { get; }

        public ContentType(string key, string body, string? characterSet, HeaderType type) : base(key, body, type)
        {
            CharacterSet = characterSet;
        }
    }
}
