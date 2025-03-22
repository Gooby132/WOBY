using System.Collections.Immutable;
using System.Text;
using Woby.Core.CommonLanguage.Signaling.Routings;
using Woby.Core.Utils.Rfc;

namespace Woby.Core.Signaling.Sip.Builder
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendMetadata(this StringBuilder builder, IImmutableDictionary<string, string> additinalMetadata)
        {
            foreach (var keyPair in additinalMetadata)
            {
                builder
                    .Append(SyntaxHelper.Primitives.SipHeaderDelimiter)
                    .Append(keyPair.Key)
                    .Append('=')
                    .Append(keyPair.Value);
            }

            return builder;
        }

        public static StringBuilder AppendRoute(this StringBuilder builder, Route route)
        {

            if (route.HasDisplayName())
                builder
                    .Append(
                        SyntaxHelper.AddressSpecifications.CreateNameAddr(
                            route.Uri.ToString(),
                            route.DisplayName));
            else
                builder
                    .Append(
                    route.Uri.ToString()
                    );

            return builder;
        }

    }
}
