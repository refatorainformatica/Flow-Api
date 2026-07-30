using MediatR;
using Services.Features.Peoples.SkillCategories.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.SkillCategories.UseCases.Queries
{
    public class GetSkillCategoryRequest
        : IRequest<Result<Response<IEnumerable<SkillCategoryResponse>>>>
    {
        public BaseQuery Query { get; set; }
    }
}
