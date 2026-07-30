using MediatR;
using Services.Features.Peoples.Sponsors.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Sponsors.UseCases.Queries
{
    public class GetByIdSponsorRequest : IRequest<Result<Response<SponsorResponse>>>
    {
        public int Id { get; set; }
    }
}
