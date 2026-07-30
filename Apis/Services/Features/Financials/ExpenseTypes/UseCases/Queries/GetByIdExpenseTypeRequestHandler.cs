using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.ExpenseTypes.Exceptions;
using Services.Features.Financials.ExpenseTypes.Models;
using Services.Features.Financials.ExpenseTypes.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.ExpenseTypes.UseCases.Queries
{
    public class GetByIdExpenseTypeRequestHandler(
        IMapper mapper,
        IMediator mediator,
        ExpenseTypeDbContext expensetypeDbContext
    )
        : CommandHandler(expensetypeDbContext, mediator),
            IRequestHandler<GetByIdExpenseTypeRequest, Result<Response<ExpenseTypeResponse>>>
    {
        private readonly ExpenseTypeDbContext _expensetypeDbContext = expensetypeDbContext;

        public async Task<Result<Response<ExpenseTypeResponse>>> Handle(
            GetByIdExpenseTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdExpenseTypeAsync(request, cancellationToken)
                .BindAsync(expensetypes => Task.FromResult(GenerateResponse(expensetypes)));
        }

        private async Task<Result<ExpenseType>> GetByIdExpenseTypeAsync(
            GetByIdExpenseTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            var expensetype = await _expensetypeDbContext
                .ExpenseTypes.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return expensetype is null
                ? Result<ExpenseType>.Failure(ExpenseTypeErrors.NotFound(request.Id))
                : Result<ExpenseType>.Success(expensetype);
        }

        private Result<Response<ExpenseTypeResponse>> GenerateResponse(ExpenseType expensetype)
        {
            var expensetypeResponse = mapper.Map<ExpenseTypeResponse>(expensetype);
            var response = new Response<ExpenseTypeResponse>(expensetypeResponse);
            return Result<Response<ExpenseTypeResponse>>.Success(response);
        }
    }
}
