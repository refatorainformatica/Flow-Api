using MediatR;
using Services.Features.Peoples.Suppliers.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Suppliers.UseCases.Commands
{
    public class CreateSupplierRequest
        : SupplierRequest,
            IRequest<Result<Response<SupplierResponse>>> { }
}
