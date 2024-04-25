using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woby.Core.CommonLanguage.Signaling.Core;

namespace Woby.Core.CommonLanguage.Signaling.ContentMeta
{
    public class ContentLength : SignalingHeader
    {

        public uint Length { get; }

        public ContentLength(string key, uint body) : base(key, body.ToString(), HeaderType.ContentMetadata)
        {
            Length = body;
        }
    }
}
