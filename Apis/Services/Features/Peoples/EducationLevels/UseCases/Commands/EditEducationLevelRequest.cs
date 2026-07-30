using MediatR;
using Services.Features.Peoples.EducationLevels.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.EducationLevels.UseCases.Commands
{
    public class EditEducationLevelRequest
        : EducationLevelRequest,
            IRequest<Result<Response<EducationLevelResponse>>>
    {
        public int RequestId { get; set; }
    }
}
