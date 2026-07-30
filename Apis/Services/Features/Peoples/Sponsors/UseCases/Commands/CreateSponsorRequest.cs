using MediatR;
using Services.Features.Peoples.Sponsors.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Sponsors.UseCases.Commands
{
    public class CreateSponsorRequest
        : SponsorRequest,
            IRequest<Result<Response<SponsorResponse>>> { }
}
