using MediatR;
using Services.Features.Peoples.Careers.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Careers.UseCases.Commands
{
    public class CreateCareerRequest : CareerRequest, IRequest<Result<Response<CareerResponse>>> { }
}
