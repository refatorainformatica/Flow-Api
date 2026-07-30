using MediatR;
using Services.Features.Peoples.EducationLevels.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.EducationLevels.UseCases.Commands
{
    public class RemoveEducationLevelRequest : IRequest<Result<Response<EducationLevelResponse>>>
    {
        public int Id { get; set; }
    }
}
