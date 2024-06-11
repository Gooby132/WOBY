using System.Collections.Immutable;
using System.Text;
using Woby.Core.CommonLanguage.Signaling.Routings;
using Woby.Core.Utils.Rfc;
using Woby.Sip.Signaling.Sip.Converters;

namespace Woby.Core.Signaling.Sip.Builder
{
    public static class StringBuilderExtensions
    {

        public static StringBuilder AppendUri(this StringBuilder builder, Route route)
        {

            if (route.HasUser())
                builder
                    .Append(':')
                    .Append(route.User);

            if (route.HasPassword())
                builder
                    .Append(":")
                    .Append(route.Password);

            if (route.ContainsUserInfo())
                builder.Append('@');

            builder
                .Append(route.Host);

            if (route.HasPort())
                builder
                    .Append(":")
                    .Append(route.Port);

            return builder;
        }

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

        public static StringBuilder AppendNameAddr(this StringBuilder builder, Route route)
        {

            if (route.HasDisplayName())
            {
                builder
                    .Append('"')
                    .Append(route.DisplayName)
                    .Append('"')
                    .Append(' ')
                    .Append('<');

                if (route.NetworkProtocol.IsSecured)
                    builder.Append("sips");
                else
                    builder.Append("sip");

                builder
                    .AppendUri(route)
                    .Append(">");
            }
            else
            {

                builder
                    .Append('"')
                    .Append('"')
                    .Append(' ');

                if (route.NetworkProtocol.IsSecured)
                    builder.Append("sips");
                else
                    builder.Append("sip");
                
                builder
                    .AppendUri(route);
            }

            return builder;
        }

        public static StringBuilder AppendVia(this StringBuilder builder, Route route)
        {
            builder
                .Append(SipHeaderMethod.Via.Key)
                .Append(": ")
                .Append(route.Protocol)
                .Append('/')
                .Append(route.NetworkProtocol.Name)
                .Append(' ')
                .AppendUri(route);

            if (route.HasAdditinalMetadata())
                builder.AppendMetadata(route.AdditinalMetadata);

            return builder.Append(SyntaxHelper.Primitives.Crlf);
        }

        public static StringBuilder AppendContact(this StringBuilder builder, Route route, SipHeaderMethod method)
        {
            builder
                .Append(method.Key)
                .Append(": ")
                .AppendNameAddr(route);

            if (route.HasAdditinalMetadata())
                builder.AppendMetadata(route.AdditinalMetadata);

            return builder
                .Append(SyntaxHelper.Primitives.Crlf);
        }

    }
}
