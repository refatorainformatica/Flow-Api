using MediatR;
using Services.Features.Peoples.SkillCategories.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.SkillCategories.UseCases.Queries
{
    public class GetBySearchSkillCategoryRequest
        : IRequest<Result<Response<IEnumerable<SkillCategoryResponse>>>>
    {
        public BaseQuerySearch Query { get; set; } = new();
    }
}
