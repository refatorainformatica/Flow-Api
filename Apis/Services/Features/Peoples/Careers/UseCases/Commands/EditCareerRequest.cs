using MediatR;
using Services.Features.Peoples.Careers.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Careers.UseCases.Commands
{
    public class EditCareerRequest : CareerRequest, IRequest<Result<Response<CareerResponse>>>
    {
        public int RequestId { get; set; }
    }
}
