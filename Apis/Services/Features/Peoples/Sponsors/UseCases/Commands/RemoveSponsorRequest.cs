using MediatR;
using Services.Features.Peoples.Sponsors.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Sponsors.UseCases.Commands
{
    public class RemoveSponsorRequest : IRequest<Result<Response<SponsorResponse>>>
    {
        public int Id { get; set; }
    }
}
