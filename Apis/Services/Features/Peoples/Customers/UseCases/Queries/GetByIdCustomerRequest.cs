using MediatR;
using Services.Features.Peoples.Customers.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Customers.UseCases.Queries
{
    public class GetByIdCustomerRequest : IRequest<Result<Response<CustomerResponse>>>
    {
        public int Id { get; set; }
    }
}
