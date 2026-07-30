using MediatR;
using Services.Features.Financials.Banks.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Banks.UseCases.Commands
{
    public class RemoveBankRequest : IRequest<Result<Response<BankResponse>>>
    {
        public int Id { get; set; }
    }
}
