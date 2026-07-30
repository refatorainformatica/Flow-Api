using MediatR;
using Services.Features.Peoples.Careers.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Careers.UseCases.Commands
{
    public class RemoveCareerRequest : IRequest<Result<Response<CareerResponse>>>
    {
        public int Id { get; set; }
    }
}
