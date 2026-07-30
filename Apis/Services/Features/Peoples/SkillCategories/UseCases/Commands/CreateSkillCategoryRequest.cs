using MediatR;
using Services.Features.Peoples.SkillCategories.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.SkillCategories.UseCases.Commands
{
    public class CreateSkillCategoryRequest
        : SkillCategoryRequest,
            IRequest<Result<Response<SkillCategoryResponse>>> { }
}
