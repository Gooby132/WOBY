using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woby.Core.CommonLanguage.Signaling.Core;

namespace Woby.Core.CommonLanguage.Signaling.ContentMeta
{
    public class ContentType : SignalingHeader
    {
        public string? CharacterSet { get; }

        public ContentType(string key, string body, string? characterSet, HeaderType type) : base(key, body, type)
        {
            CharacterSet = characterSet;
        }
    }
}
