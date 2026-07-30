using MediatR;
using Services.Features.Peoples.Customers.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Customers.UseCases.Queries
{
    public class GetCustomerRequest : IRequest<Result<Response<IEnumerable<CustomerResponse>>>>
    {
        public BaseQuery Query { get; set; }
    }
}
