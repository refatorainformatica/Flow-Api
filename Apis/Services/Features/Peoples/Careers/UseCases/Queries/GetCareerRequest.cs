using MediatR;
using Services.Features.Peoples.Careers.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Careers.UseCases.Queries
{
    public class GetCareerRequest : IRequest<Result<Response<IEnumerable<CareerResponse>>>>
    {
        public BaseQuery Query { get; set; }
    }
}
