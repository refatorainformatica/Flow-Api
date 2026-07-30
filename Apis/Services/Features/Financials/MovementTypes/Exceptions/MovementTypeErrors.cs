using Shared.Domain.Abstractions.Enumerations;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.MovementTypes.Exceptions
{
    public static class MovementTypeErrors
    {
        public static Error IsEmpty() =>
            new(
                ErrorType.NoContent,
                ErrorType.NoContent.ToString(),
                "The movement type data was empty"
            );

        public static Error NotFound(int id) =>
            new(
                ErrorType.NotFound,
                ErrorType.NotFound.ToString(),
                $"The movement type with Id '{id}' was not found"
            );

        public static Error NotFound(string searchText) =>
            new(
                ErrorType.NotFound,
                ErrorType.NotFound.ToString(),
                $"The movement type with term '{searchText}' was not found"
            );

        public static Error PreConditionFailed(int id) =>
            new(
                ErrorType.PreConditionFailed,
                ErrorType.PreConditionFailed.ToString(),
                $"The request Id '{id}' was invalid"
            );
    }
}
