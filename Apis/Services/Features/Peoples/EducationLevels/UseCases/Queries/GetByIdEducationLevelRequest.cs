using MediatR;
using Services.Features.Peoples.EducationLevels.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.EducationLevels.UseCases.Queries
{
    public class GetByIdEducationLevelRequest : IRequest<Result<Response<EducationLevelResponse>>>
    {
        public int Id { get; set; }
    }
}
