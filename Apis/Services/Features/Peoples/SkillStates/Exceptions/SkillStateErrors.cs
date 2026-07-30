using Shared.Domain.Abstractions.Enumerations;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.SkillStates.Exceptions
{
    public static class SkillStateErrors
    {
        public static Error IsEmpty() =>
            new(
                ErrorType.NoContent,
                ErrorType.NoContent.ToString(),
                "The skill state data was empty"
            );

        public static Error NotFound(int id) =>
            new(
                ErrorType.NotFound,
                ErrorType.NotFound.ToString(),
                $"The skill state with Id '{id}' was not found"
            );

        public static Error NotFound(string searchText) =>
            new(
                ErrorType.NotFound,
                ErrorType.NotFound.ToString(),
                $"The skill state with term '{searchText}' was not found"
            );

        public static Error PreConditionFailed(int id) =>
            new(
                ErrorType.PreConditionFailed,
                ErrorType.PreConditionFailed.ToString(),
                $"The request Id '{id}' was invalid"
            );
    }
}
