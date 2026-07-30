using MediatR;
using Services.Features.Financials.Revenues.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Revenues.UseCases.Queries
{
    public class GetByIdRevenueRequest : IRequest<Result<Response<RevenueResponse>>>
    {
        public int Id { get; set; }
    }
}
