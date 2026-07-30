using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.Banks.Exceptions;
using Services.Features.Financials.Banks.Models;
using Services.Features.Financials.Banks.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.Banks.UseCases.Queries
{
    public class GetBySearchBankRequestHandler(
        IMapper mapper,
        IMediator mediator,
        BankDbContext bankDbContext
    )
        : CommandHandler(bankDbContext, mediator),
            IRequestHandler<GetBySearchBankRequest, Result<Response<IEnumerable<BankResponse>>>>
    {
        private readonly BankDbContext _bankDbContext = bankDbContext;

        public async Task<Result<Response<IEnumerable<BankResponse>>>> Handle(
            GetBySearchBankRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetBySearchBankAsync(request)
                .BindAsync(banks => Task.FromResult(GenerateResponse(banks)));
        }

        private async Task<Result<Pagination<Bank>>> GetBySearchBankAsync(
            GetBySearchBankRequest request
        )
        {
            var banks = await Task.Run(
                () =>
                    _bankDbContext
                        .Banks.AsNoTracking()
                        .SearchBy(request.Query.SearchText)
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<Bank>()
            );

            return !banks.Rows.Any()
                ? Result<Pagination<Bank>>.Failure(BankErrors.NotFound(request.Query.SearchText))
                : Result<Pagination<Bank>>.Success(banks);
        }

        private Result<Response<IEnumerable<BankResponse>>> GenerateResponse(
            Pagination<Bank> paginationBank
        )
        {
            var bankResponse = mapper.Map<IEnumerable<BankResponse>>(paginationBank.Rows);
            var response = new Response<IEnumerable<BankResponse>>(
                bankResponse,
                paginationBank.Offset,
                paginationBank.Limit,
                paginationBank.PageCount,
                paginationBank.RowCount
            );
            return Result<Response<IEnumerable<BankResponse>>>.Success(response);
        }
    }
}
