using MediatR;
using Services.Features.Peoples.Customers.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Customers.UseCases.Queries
{
    public class GetBySearchCustomerRequest
        : IRequest<Result<Response<IEnumerable<CustomerResponse>>>>
    {
        public BaseQuerySearch Query { get; set; }
    }
}
