using MediatR;
using Services.Features.Peoples.SkillCategories.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.SkillCategories.UseCases.Queries
{
    public class GetByIdSkillCategoryRequest : IRequest<Result<Response<SkillCategoryResponse>>>
    {
        public int Id { get; set; }
    }
}
