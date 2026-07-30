using MediatR;
using Services.Features.Peoples.Sellers.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Sellers.UseCases.Queries
{
    public class GetSellerRequest : IRequest<Result<Response<IEnumerable<SellerResponse>>>>
    {
        public BaseQuery Query { get; set; }
    }
}
