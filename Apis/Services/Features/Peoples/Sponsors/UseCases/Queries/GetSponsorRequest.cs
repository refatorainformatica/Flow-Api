using MediatR;
using Services.Features.Peoples.Sponsors.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Sponsors.UseCases.Queries
{
    public class GetSponsorRequest : IRequest<Result<Response<IEnumerable<SponsorResponse>>>>
    {
        public BaseQuery Query { get; set; }
    }
}
