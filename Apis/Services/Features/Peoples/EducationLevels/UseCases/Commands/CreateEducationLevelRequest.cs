using MediatR;
using Services.Features.Peoples.EducationLevels.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.EducationLevels.UseCases.Commands
{
    public class CreateEducationLevelRequest : IRequest<Result<Response<EducationLevelResponse>>>
    {
        public string ExternalCode { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
    }
}
