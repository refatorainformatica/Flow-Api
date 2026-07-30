using MediatR;
using Services.Features.Financials.RevenueTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.RevenueTypes.UseCases.Commands
{
    public class CreateRevenueTypeRequest
        : RevenueTypeRequest,
            IRequest<Result<Response<RevenueTypeResponse>>> { }
}
