using MediatR;
using Services.Features.Financials.Banks.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Banks.UseCases.Queries
{
    public class GetBankRequest : IRequest<Result<Response<IEnumerable<BankResponse>>>>
    {
        public BaseQuery Query { get; set; } = new();
    }
}
