using MediatR;
using Services.Features.Peoples.Suppliers.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Suppliers.UseCases.Queries
{
    public class GetBySearchSupplierRequest
        : IRequest<Result<Response<IEnumerable<SupplierResponse>>>>
    {
        public BaseQuerySearch Query { get; set; } = new();
    }
}
