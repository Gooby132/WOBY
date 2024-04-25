using System.Text;
using Woby.Core.CommonLanguage.Signaling.Routings;
using Woby.Core.Utils.Rfc;

namespace Woby.Core.Signaling.Sip.Builder
{
    public static class StringBuilderExtensions
    {

        public static StringBuilder AppendRoutes(this StringBuilder builder, Route route)
        {
            return builder;
        }

        public static StringBuilder AppendRoute(this StringBuilder builder, Route route)
        {
            builder
                .Append(
                    SyntaxHelper.AddressSpecifications.CreateNameAddr(
                        route.Uri.ToString(),
                        route.DisplayName));

            if (route.AdditinalMetadata is not null)
                builder
                    .Append(" ")
                    .AppendJoin(";", route.AdditinalMetadata.Select(meta => string.Join(":", meta.Key, meta.Value)));

            return builder;
        }

    }
}
