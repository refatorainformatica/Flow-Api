using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.ExpenseTypes.Exceptions;
using Services.Features.Financials.ExpenseTypes.Models;
using Services.Features.Financials.ExpenseTypes.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.ExpenseTypes.UseCases.Queries
{
    public class GetExpenseTypeRequestHandler(
        IMapper mapper,
        IMediator mediator,
        ExpenseTypeDbContext expensetypeDbContext
    )
        : CommandHandler(expensetypeDbContext, mediator),
            IRequestHandler<
                GetExpenseTypeRequest,
                Result<Response<IEnumerable<ExpenseTypeResponse>>>
            >
    {
        private readonly ExpenseTypeDbContext _expensetypeDbContext = expensetypeDbContext;

        public async Task<Result<Response<IEnumerable<ExpenseTypeResponse>>>> Handle(
            GetExpenseTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetExpenseTypeAsync(request)
                .BindAsync(expensetypes => Task.FromResult(GenerateResponse(expensetypes)));
        }

        private async Task<Result<Pagination<ExpenseType>>> GetExpenseTypeAsync(
            GetExpenseTypeRequest request
        )
        {
            var expensetypes = await Task.Run(
                () =>
                    _expensetypeDbContext
                        .ExpenseTypes.AsNoTracking()
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<ExpenseType>()
            );

            return !expensetypes.Rows.Any()
                ? Result<Pagination<ExpenseType>>.Failure(ExpenseTypeErrors.IsEmpty())
                : Result<Pagination<ExpenseType>>.Success(expensetypes);
        }

        private Result<Response<IEnumerable<ExpenseTypeResponse>>> GenerateResponse(
            Pagination<ExpenseType> paginationExpenseType
        )
        {
            var expensetypeResponse = mapper.Map<IEnumerable<ExpenseTypeResponse>>(
                paginationExpenseType.Rows
            );
            var response = new Response<IEnumerable<ExpenseTypeResponse>>(
                expensetypeResponse,
                paginationExpenseType.Offset,
                paginationExpenseType.Limit,
                paginationExpenseType.PageCount,
                paginationExpenseType.RowCount
            );
            return Result<Response<IEnumerable<ExpenseTypeResponse>>>.Success(response);
        }
    }
}
