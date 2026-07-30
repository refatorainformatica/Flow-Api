using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.Customers.Exceptions;
using Services.Features.Peoples.Customers.Models;
using Services.Features.Peoples.Customers.Repositories;
using Services.Features.Peoples.Customers.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Customers.UseCases.Queries
{
    public class GetByIdCustomerRequestHandler(
        IMapper mapper,
        IMediator mediator,
        CustomerDbContext customerDbContext
    )
        : CommandHandler(customerDbContext, mediator),
            IRequestHandler<GetByIdCustomerRequest, Result<Response<CustomerResponse>>>
    {
        private readonly CustomerDbContext _customerDbContext = customerDbContext;

        public async Task<Result<Response<CustomerResponse>>> Handle(
            GetByIdCustomerRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdCustomerAsync(request, cancellationToken)
                .BindAsync(customers => Task.FromResult(GenerateResponse(customers)));
        }

        private async Task<Result<Customer>> GetByIdCustomerAsync(
            GetByIdCustomerRequest request,
            CancellationToken cancellationToken
        )
        {
            var customer = await _customerDbContext
                .Customers.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return customer is null
                ? Result<Customer>.Failure(CustomerErrors.NotFound(request.Id))
                : Result<Customer>.Success(customer);
        }

        private Result<Response<CustomerResponse>> GenerateResponse(Customer customer)
        {
            var customerResponse = mapper.Map<CustomerResponse>(customer);
            var response = new Response<CustomerResponse>(customerResponse);
            return Result<Response<CustomerResponse>>.Success(response);
        }
    }
}
