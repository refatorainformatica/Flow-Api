using MediatR;
using Services.Features.Financials.Revenues.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Revenues.UseCases.Commands
{
    public class CreateRevenueRequest
        : RevenueRequest,
            IRequest<Result<Response<RevenueResponse>>> { }
}
