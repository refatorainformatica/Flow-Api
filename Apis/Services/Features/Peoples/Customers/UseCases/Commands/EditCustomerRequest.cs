using MediatR;
using Services.Features.Peoples.Customers.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Customers.UseCases.Commands
{
    public class EditCustomerRequest : CustomerRequest, IRequest<Result<Response<CustomerResponse>>>
    {
        public int RequestId { get; set; }
    }
}
