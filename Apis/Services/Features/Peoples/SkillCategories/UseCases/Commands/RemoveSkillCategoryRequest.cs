using MediatR;
using Services.Features.Peoples.SkillCategories.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.SkillCategories.UseCases.Commands
{
    public class RemoveSkillCategoryRequest : IRequest<Result<Response<SkillCategoryResponse>>>
    {
        public int Id { get; set; }
    }
}
