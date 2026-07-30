using MediatR;
using Services.Features.Peoples.Careers.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Careers.UseCases.Queries
{
    public class GetBySearchCareerRequest : IRequest<Result<Response<IEnumerable<CareerResponse>>>>
    {
        public BaseQuerySearch Query { get; set; } = new();
    }
}
