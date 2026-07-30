using MediatR;
using Services.Features.Financials.Banks.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Banks.UseCases.Queries
{
    public class GetBySearchBankRequest : IRequest<Result<Response<IEnumerable<BankResponse>>>>
    {
        public BaseQuerySearch Query { get; set; } = new();
    }
}
