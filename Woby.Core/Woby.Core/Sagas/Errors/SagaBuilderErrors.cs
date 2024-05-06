using FluentResults;
using Woby.Core.Commons.Errors;

namespace Woby.Core.Sagas.Errors
{
    public static class SagaBuilderErrors
    {

        public const int GroupCode = 1;

        public static Error ClientWasNotProvided() => new ErrorBase(GroupCode, 1, "client was not provided");
        public static Error BuilderWasNotProvided() => new ErrorBase(GroupCode, 2, "builder was not provided");
        public static Error ConverterWasNotProvided() => new ErrorBase(GroupCode, 3, "converter was not provided");
        public static Error TransmitterWasNotProvided() => new ErrorBase(GroupCode, 4, "transmitter was not provided");
        public static Error ReceiverWasNotProvided() => new ErrorBase(GroupCode, 5, "receiver was not provided");
        public static Error ParserWasNotProvided() => new ErrorBase(GroupCode, 6, "parser was not provided");
    }
}
