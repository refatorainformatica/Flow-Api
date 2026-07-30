using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.Customers.Exceptions;
using Services.Features.Peoples.Customers.Models;
using Services.Features.Peoples.Customers.Repositories;
using Services.Features.Peoples.Customers.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.Customers.UseCases.Queries
{
    public class GetBySearchCustomerRequestHandler(
        IMapper mapper,
        IMediator mediator,
        CustomerDbContext customerDbContext
    )
        : CommandHandler(customerDbContext, mediator),
            IRequestHandler<
                GetBySearchCustomerRequest,
                Result<Response<IEnumerable<CustomerResponse>>>
            >
    {
        private readonly CustomerDbContext _customerDbContext = customerDbContext;

        public async Task<Result<Response<IEnumerable<CustomerResponse>>>> Handle(
            GetBySearchCustomerRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetBySearchCustomerAsync(request)
                .BindAsync(customers => Task.FromResult(GenerateResponse(customers)));
        }

        private async Task<Result<Pagination<Customer>>> GetBySearchCustomerAsync(
            GetBySearchCustomerRequest request
        )
        {
            var customers = await Task.Run(
                () =>
                    _customerDbContext
                        .Customers.AsNoTracking()
                        .SearchBy(request.Query.SearchText)
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<Customer>()
            );

            return !customers.Rows.Any()
                ? Result<Pagination<Customer>>.Failure(
                    CustomerErrors.NotFound(request.Query.SearchText)
                )
                : Result<Pagination<Customer>>.Success(customers);
        }

        private Result<Response<IEnumerable<CustomerResponse>>> GenerateResponse(
            Pagination<Customer> paginationCustomer
        )
        {
            var customerResponse = mapper.Map<IEnumerable<CustomerResponse>>(
                paginationCustomer.Rows
            );
            var response = new Response<IEnumerable<CustomerResponse>>(
                customerResponse,
                paginationCustomer.Offset,
                paginationCustomer.Limit,
                paginationCustomer.PageCount,
                paginationCustomer.RowCount
            );
            return Result<Response<IEnumerable<CustomerResponse>>>.Success(response);
        }
    }
}
