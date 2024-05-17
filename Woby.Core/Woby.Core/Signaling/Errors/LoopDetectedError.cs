using Woby.Core.Commons.Errors;

namespace Woby.Core.Signaling.Errors
{
    public class LoopDetectedError : ErrorBase
    {
        public const int GroupErrorCode = 5;

        public LoopDetectedError() : base(GroupErrorCode, 1, "Loop Detected")
        {
        }
    }
}
