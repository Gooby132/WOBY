namespace Woby.Core.Core.Messages
{
    public abstract class MessageBase
    {

        /// <summary>
        /// The desired recipient request or the address-of-record of the user or resource that is the target request
        /// </summary>
        public string To { get; init; }
        public string From { get; init; }
        public string CSeq { get; init; }
        public string CallId { get; init; }
        public int MaxForward { get; init; }
        public string Via { get; init; }

        protected MessageBase(
            string to, 
            string from, 
            string cSeq, 
            string callId, 
            int maxForward, 
            string via)
        {
            To = to;
            From = from;
            CSeq = cSeq;
            CallId = callId;
            MaxForward = maxForward;
            Via = via;
        }
    }
}
