using MediatR;
using Services.Features.Financials.RevenueTypes.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.RevenueTypes.UseCases.Queries
{
    public class GetByIdRevenueTypeRequest : IRequest<Result<Response<RevenueTypeResponse>>>
    {
        public int Id { get; set; }
    }
}
