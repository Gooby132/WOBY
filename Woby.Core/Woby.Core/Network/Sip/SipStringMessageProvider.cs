using FluentResults;
using System.Text;

namespace Woby.Core.Network.Sip
{
    public class SipStringMessageProvider
    {

        private readonly string _sipMessage;

        public delegate void SipStringMessageDelegate(Result<Stream> stream);
        public event SipStringMessageDelegate? MessageDelegateEvent;

        public SipStringMessageProvider(string sipMessage)
        {
            _sipMessage = sipMessage;
        }

        public void Start()
        {
            MessageDelegateEvent?.Invoke(
                Result.Ok<Stream>(
                    new MemoryStream(Encoding.UTF8.GetBytes(_sipMessage))
                    ));
        }
    }
}
