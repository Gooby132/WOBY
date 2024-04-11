using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woby.Core.Core.Headers.Core;

namespace Woby.Core.Core.Headers.ContentMeta
{
    public class ContentLength : HeaderBase
    {

        public uint Length { get; }

        public ContentLength(string key, uint body) : base(key, body.ToString(), HeaderType.ContentMetadata)
        {
            Length = body;
        }
    }
}
