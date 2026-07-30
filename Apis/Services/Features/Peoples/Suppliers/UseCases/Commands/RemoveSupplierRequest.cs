using MediatR;
using Services.Features.Peoples.Suppliers.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Suppliers.UseCases.Commands
{
    public class RemoveSupplierRequest : IRequest<Result<Response<SupplierResponse>>>
    {
        public int Id { get; set; }
    }
}
