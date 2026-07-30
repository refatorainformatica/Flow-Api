using MediatR;
using Services.Features.Peoples.ActivityBranchs.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.ActivityBranchs.UseCases.Queries
{
    public class GetActivityBranchRequest
        : IRequest<Result<Response<IEnumerable<ActivityBranchResponse>>>>
    {
        public BaseQuery Query { get; set; }
    }
}
