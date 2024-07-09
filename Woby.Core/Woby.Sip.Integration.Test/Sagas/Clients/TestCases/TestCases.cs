using Woby.Core.CommonLanguage.Messages;

namespace Woby.Sip.Integration.Test.Sagas.Clients.TestCases;

public static class TestCases
{

    public static object[][] UserAgentNotFound =>
    [
        [
            "INVITE sip:bob1@example.com SIP/2.0\r\n" +
            "Via: SIP/2.0/UDP client.example.com;branch=z9hG4bK776asdhds\r\n" +
            "Max-Forwards: 70\r\n" +
            "From: Alice <sip:alice@example.com>;tag=1928301774\r\n" +
            "To: Bob <sip:bob1@example.com>\r\n" +
            "Call-ID: a84b4c76e66710@client.example.com\r\n" +
            "CSeq: 314159 INVITE\r\n" +
            "Contact: <sip:alice@client.example.com>\r\n" +
            "Content-Type: application/sdp\r\n" +
            "Content-Length: 142\r\n",
@"SIP/2.0 100 Trying
Via: SIP/2.0/UDP pc33.atlanta.com;branch=z9hG4bK776asdhds
To: Bob <sip:bob1@example.com>\r\n
Call-ID: a84b4c76e66710@client.example.com\r\n
Call-ID: a84b4c76e66710@pc33.atlanta.com
CSeq: 314159 INVITE
",
            "SIP/2.0 404 Not Found\r\n" +
            "Via: SIP/2.0/UDP client.example.com;branch=z9hG4bK776asdhds;received=192.0.2.4\r\n" +
            "From: \"Alice\" <sip:alice@example.com>;tag=1928301774\r\n" +
            "To: \"Bob\" <sip:bob1@example.com>\r\n" +
            "Call-ID: a84b4c76e66710@client.example.com\r\n" +
            "CSeq: 314159 INVITE\r\n" +
            "Content-Length: 0\r\n"
        ]
    ];

    public static object[][] UserAgentTryingAndRinging =>
[
    [
        // a -> b
@"INVITE sip:b@b.example.com SIP/2.0
Via: SIP/2.0/UDP pc33.atlanta.com;branch=z9hG4bK776asdhds
Max-Forwards: 70
To: Bob <sip:b@b.example.com>
From: Alice <sip:a@a.example.com>;tag=1928301774
Call-ID: a84b4c76e66710@pc33.atlanta.com
CSeq: 314159 INVITE
Contact: <sip:a@pc33.atlanta.com>
Content-Type: application/sdp
Content-Length: 142
"
,
        //b -> a
@"SIP/2.0 100 Trying
Via: SIP/2.0/UDP pc33.atlanta.com;branch=z9hG4bK776asdhds
To: Bob <sip:b@b.example.com>
From: Alice <sip:a@a.example.com>;tag=1928301774
Call-ID: a84b4c76e66710@pc33.atlanta.com
CSeq: 314159 INVITE
",
        //b -> a
@"SIP/2.0 180 Ringing
Via: SIP/2.0/UDP pc33.atlanta.com;branch=z9hG4bK776asdhds
To: Bob <sip:b@b.example.com>;tag=832123435
From: Alice <sip:a@a.example.com>;tag=1928301774
Call-ID: a84b4c76e66710@pc33.atlanta.com
CSeq: 314159 INVITE
Contact: <sip:b@b.example.com>
Content-Length: 0
"]
];
}
