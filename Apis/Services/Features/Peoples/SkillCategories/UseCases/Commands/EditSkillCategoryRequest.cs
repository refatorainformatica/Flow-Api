using MediatR;
using Services.Features.Peoples.SkillCategories.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.SkillCategories.UseCases.Commands
{
    public class EditSkillCategoryRequest
        : SkillCategoryRequest,
            IRequest<Result<Response<SkillCategoryResponse>>>
    {
        public int RequestId { get; set; }
    }
}
