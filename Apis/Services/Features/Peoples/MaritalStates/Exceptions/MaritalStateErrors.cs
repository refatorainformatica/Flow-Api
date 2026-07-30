using Shared.Domain.Abstractions.Enumerations;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.MaritalStates.Exceptions
{
    public static class MaritalStateErrors
    {
        public static Error IsEmpty() =>
            new(
                ErrorType.NoContent,
                ErrorType.NoContent.ToString(),
                "The marital state data was empty"
            );

        public static Error NotFound(int id) =>
            new(
                ErrorType.NotFound,
                ErrorType.NotFound.ToString(),
                $"The marital state with Id '{id}' was not found"
            );

        public static Error NotFound(string searchText) =>
            new(
                ErrorType.NotFound,
                ErrorType.NotFound.ToString(),
                $"The marital state with term '{searchText}' was not found"
            );

        public static Error PreConditionFailed(int id) =>
            new(
                ErrorType.PreConditionFailed,
                ErrorType.PreConditionFailed.ToString(),
                $"The request Id '{id}' was invalid"
            );
    }
}
