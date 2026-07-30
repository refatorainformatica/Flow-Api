using Shared.Domain.Abstractions.Enumerations;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.CurrencyTypes.Exceptions
{
    public static class CurrencyTypeErrors
    {
        public static Error IsEmpty() =>
            new(
                ErrorType.NoContent,
                ErrorType.NoContent.ToString(),
                "The cost center data was empty"
            );

        public static Error NotFound(int id) =>
            new(
                ErrorType.NotFound,
                ErrorType.NotFound.ToString(),
                $"The cost center with Id '{id}' was not found"
            );

        public static Error NotFound(string searchText) =>
            new(
                ErrorType.NotFound,
                ErrorType.NotFound.ToString(),
                $"The cost center with term '{searchText}' was not found"
            );

        public static Error PreConditionFailed(int id) =>
            new(
                ErrorType.PreConditionFailed,
                ErrorType.PreConditionFailed.ToString(),
                $"The cost center request Id '{id}' was invalid"
            );
    }
}
