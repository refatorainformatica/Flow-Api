using MediatR;
using Services.Features.Peoples.Sellers.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Sellers.UseCases.Commands
{
    public class CreateSellerRequest : SellerRequest, IRequest<Result<Response<SellerResponse>>> { }
}
