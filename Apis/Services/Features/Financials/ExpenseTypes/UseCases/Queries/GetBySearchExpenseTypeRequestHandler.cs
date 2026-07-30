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
    public class GetBySearchExpenseTypeRequestHandler(
        IMapper mapper,
        IMediator mediator,
        ExpenseTypeDbContext expensetypeDbContext
    )
        : CommandHandler(expensetypeDbContext, mediator),
            IRequestHandler<
                GetBySearchExpenseTypeRequest,
                Result<Response<IEnumerable<ExpenseTypeResponse>>>
            >
    {
        private readonly ExpenseTypeDbContext _expensetypeDbContext = expensetypeDbContext;

        public async Task<Result<Response<IEnumerable<ExpenseTypeResponse>>>> Handle(
            GetBySearchExpenseTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetBySearchExpenseTypeAsync(request)
                .BindAsync(expensetypes => Task.FromResult(GenerateResponse(expensetypes)));
        }

        private async Task<Result<Pagination<ExpenseType>>> GetBySearchExpenseTypeAsync(
            GetBySearchExpenseTypeRequest request
        )
        {
            var expensetypes = await Task.Run(
                () =>
                    _expensetypeDbContext
                        .ExpenseTypes.AsNoTracking()
                        .SearchBy(request.Query.SearchText)
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<ExpenseType>()
            );

            return !expensetypes.Rows.Any()
                ? Result<Pagination<ExpenseType>>.Failure(
                    ExpenseTypeErrors.NotFound(request.Query.SearchText)
                )
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
