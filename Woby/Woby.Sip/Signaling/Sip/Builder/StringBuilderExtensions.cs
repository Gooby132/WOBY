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

        public static StringBuilder AppendHeaderParameter(this StringBuilder builder, string key, string value) => 
            builder
                .Append(SyntaxHelper.Primitives.SipHeaderDelimiter)
                .Append(key)
                .Append('=')
                .Append(value);

        public static StringBuilder AppendHeaderParameters(this StringBuilder builder, IImmutableDictionary<string, string> additinalMetadata)
        {
            foreach (var keyPair in additinalMetadata)
                builder.AppendHeaderParameter(keyPair.Key, keyPair.Value);

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

        public static StringBuilder AppendHeader(this StringBuilder builder, string key, string value) =>
            builder
                .Append(key)
                .Append(": ")
                .Append(value)
                .Append(SyntaxHelper.Primitives.Crlf);

        public static StringBuilder AppendVia(this StringBuilder builder, Proxy proxy)
        {
            builder
                .Append(SipHeaderMethod.Via.Key)
                .Append(": ")
                .Append(proxy.Protocol)
                .Append('/')
                .Append(proxy.DeclaredTransport)
                .Append(' ')
                .Append(proxy.Host);

            if (proxy.Port.HasValue)
                builder
                    .Append(":")
                    .Append(proxy.Port);

            if (proxy.HasAdditinalMetadata())
                builder
                    .AppendHeaderParameters(proxy.AdditinalMetadata);

            if (proxy.HasNetworkMetadata())
                builder
                    .AppendHeaderParameter(
                        "received", 
                        proxy.Metadata.ReceviedOn.Port > 0 ?
                            proxy.Metadata.ReceviedOn.ToString() :
                            proxy.Metadata.ReceviedOn.Address.ToString());

            return builder.Append(SyntaxHelper.Primitives.Crlf);
        }

        public static StringBuilder AppendContact(this StringBuilder builder, Route route, SipHeaderMethod method)
        {
            builder
                .Append(method.Key)
                .Append(": ")
                .AppendNameAddr(route);

            if (route.HasAdditinalMetadata())
                builder.AppendHeaderParameters(route.AdditinalMetadata);

            return builder
                .Append(SyntaxHelper.Primitives.Crlf);
        }

    }
}
