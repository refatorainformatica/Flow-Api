using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.SkillCategories.Models
{
    public class SkillCategoryResponse : BaseResponse
    {
        public string Description { get; set; }
    }
}
