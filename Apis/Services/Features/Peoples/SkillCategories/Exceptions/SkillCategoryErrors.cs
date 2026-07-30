using Shared.Domain.Abstractions.Enumerations;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.SkillCategories.Exceptions
{
    public static class SkillCategoryErrors
    {
        public static Error IsEmpty() =>
            new(
                ErrorType.NoContent,
                ErrorType.NoContent.ToString(),
                "The skill category data was empty"
            );

        public static Error NotFound(int id) =>
            new(
                ErrorType.NotFound,
                ErrorType.NotFound.ToString(),
                $"The skill category with Id '{id}' was not found"
            );

        public static Error NotFound(string searchText) =>
            new(
                ErrorType.NotFound,
                ErrorType.NotFound.ToString(),
                $"The skill category with term '{searchText}' was not found"
            );

        public static Error PreConditionFailed(int id) =>
            new(
                ErrorType.PreConditionFailed,
                ErrorType.PreConditionFailed.ToString(),
                $"The request Id '{id}' was invalid"
            );
    }
}
