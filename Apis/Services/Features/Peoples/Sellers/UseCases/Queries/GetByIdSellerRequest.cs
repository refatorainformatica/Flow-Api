using MediatR;
using Services.Features.Peoples.Sellers.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Sellers.UseCases.Queries
{
    public class GetByIdSellerRequest : IRequest<Result<Response<SellerResponse>>>
    {
        public int Id { get; set; }
    }
}
