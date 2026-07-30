using MediatR;
using Services.Features.Peoples.Suppliers.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Suppliers.UseCases.Queries
{
    public class GetSupplierRequest : IRequest<Result<Response<IEnumerable<SupplierResponse>>>>
    {
        public BaseQuery Query { get; set; }
    }
}
