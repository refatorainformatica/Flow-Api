using MediatR;
using Services.Features.Peoples.Sellers.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Sellers.UseCases.Queries
{
    public class GetBySearchSellerRequest : IRequest<Result<Response<IEnumerable<SellerResponse>>>>
    {
        public BaseQuerySearch Query { get; set; } = new();
    }
}
