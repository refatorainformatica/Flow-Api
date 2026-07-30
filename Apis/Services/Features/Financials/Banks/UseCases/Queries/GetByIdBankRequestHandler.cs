using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.Banks.Exceptions;
using Services.Features.Financials.Banks.Models;
using Services.Features.Financials.Banks.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Banks.UseCases.Queries
{
    public class GetByIdBankRequestHandler(
        IMapper mapper,
        IMediator mediator,
        BankDbContext bankDbContext
    )
        : CommandHandler(bankDbContext, mediator),
            IRequestHandler<GetByIdBankRequest, Result<Response<BankResponse>>>
    {
        private readonly BankDbContext _bankDbContext = bankDbContext;

        public async Task<Result<Response<BankResponse>>> Handle(
            GetByIdBankRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdBankAsync(request, cancellationToken)
                .BindAsync(banks => Task.FromResult(GenerateResponse(banks)));
        }

        private async Task<Result<Bank>> GetByIdBankAsync(
            GetByIdBankRequest request,
            CancellationToken cancellationToken
        )
        {
            var bank = await _bankDbContext
                .Banks.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return bank is null
                ? Result<Bank>.Failure(BankErrors.NotFound(request.Id))
                : Result<Bank>.Success(bank);
        }

        private Result<Response<BankResponse>> GenerateResponse(Bank bank)
        {
            var bankResponse = mapper.Map<BankResponse>(bank);
            var response = new Response<BankResponse>(bankResponse);
            return Result<Response<BankResponse>>.Success(response);
        }
    }
}
