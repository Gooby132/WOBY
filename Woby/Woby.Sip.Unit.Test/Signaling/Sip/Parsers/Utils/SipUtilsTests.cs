using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Woby.Core.Signaling.Sip.Headers;
using Woby.Core.Signaling.Sip.Parsers.Utils;

namespace Woby.Sip.Unit.Test.Signaling.Sip.Parsers.Utils;

[TestClass]
public class SipUtilsTests
{

    [DataTestMethod]
    [DataRow(
        "sip:alice:password123@atlanta.com:5060",
        "sip",
        "alice",
        "password123",
        "atlanta.com",
        5060
        )]
    [DataRow(
        "sip:alice:password123@atlanta.com",
        "sip",
        "alice",
        "password123",
        "atlanta.com",
        null
        )]
    [DataRow(
        "sip:alice@atlanta.com",
        "sip",
        "alice",
        null,
        "atlanta.com",
        null
        )]
    public void TryParseUriNoHeadersNoParameters_ValidUri_Successful(
        string uri,
        string expectedScheme, 
        string expectedUser,
        string? expectedPassword, 
        string expectedHost, 
        int? expectedPort)
    {

        Assert.IsTrue(HeaderFieldsUtils.TryParseContactUri(
            uri,
            out var scheme,
            out var user,
            out var password,
            out var host,
            out var port,
            out var parameters,
            out var headers
            ), "Parsing URI was not successful");

        Assert.AreEqual(expectedScheme, scheme, "Scheme are not matched");
        Assert.AreEqual(expectedUser, user, "User are not matched");
        Assert.AreEqual(expectedPassword, password, "Password are not matched");
        Assert.AreEqual(expectedHost, host, "Host are not matched");
        Assert.AreEqual(expectedPort, port, "Port are not matched");
    }

    [DataTestMethod]
    [DataRow( "sip:alice:password123atlanta.com:5060")]
    [DataRow("alice:password123@atlanta.com")]
    [DataRow(":alice@atlanta.com")]
    public void TryParseUriNoHeadersNoParameters_InvalidUri_Successful(string uri)
    {

        Assert.IsFalse(HeaderFieldsUtils.TryParseContactUri(
            uri,
            out var scheme,
            out var user,
            out var password,
            out var host,
            out var port,
            out var parameters,
            out var headers
            ), "Parsing URI was successful");

        Assert.IsNull(scheme, "Scheme is not null");
        Assert.IsNull(user, "User is not null");
        Assert.IsNull(password, "Password is not null");
        Assert.IsNull(host, "Host is not null");
        Assert.IsNull(port, "Port is not null");
    }

}
