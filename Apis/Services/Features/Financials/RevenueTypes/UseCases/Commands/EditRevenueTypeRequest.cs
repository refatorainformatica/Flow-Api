using MediatR;
using Services.Features.Financials.RevenueTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.RevenueTypes.UseCases.Commands
{
    public class EditRevenueTypeRequest
        : RevenueTypeRequest,
            IRequest<Result<Response<RevenueTypeResponse>>>
    {
        public int RequestId { get; set; }
    }
}
