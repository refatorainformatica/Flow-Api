using MediatR;
using Services.Features.Peoples.Customers.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Customers.UseCases.Commands
{
    public class CreateCustomerRequest
        : CustomerRequest,
            IRequest<Result<Response<CustomerResponse>>> { }
}
