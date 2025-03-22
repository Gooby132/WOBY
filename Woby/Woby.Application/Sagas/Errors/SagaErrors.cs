using FluentResults;
using Woby.Core.Commons.Errors;

namespace Woby.Application.Sagas.Errors
{
    public static class SagaErrors
    {

        public const int BuilderGroupCode = 1;
        public const int ClientsGroupCode = 2;

        public static Error ClientWasNotProvided() => new ErrorBase(BuilderGroupCode, 1, "client was not provided");
        public static Error BuilderTypeWasNotProvided() => new ErrorBase(BuilderGroupCode, 2, "builder was not provided");
        public static Error ConverterTypeWasNotProvided() => new ErrorBase(BuilderGroupCode, 3, "converter was not provided");
        public static Error TransmitterWasNotProvided() => new ErrorBase(BuilderGroupCode, 4, "transmitter was not provided");
        public static Error ReceiverWasNotProvided() => new ErrorBase(BuilderGroupCode, 5, "receiver was not provided");
        public static Error ParserTypeWasNotProvided() => new ErrorBase(BuilderGroupCode, 6, "parser was not provided");

        public static Error ClientSagaTransmissionWasNotProvided() => new ErrorBase(ClientsGroupCode, 1, "client transmission was not provided");

    }
}
