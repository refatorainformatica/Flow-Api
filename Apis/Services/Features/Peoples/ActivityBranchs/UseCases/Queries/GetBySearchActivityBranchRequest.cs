using MediatR;
using Services.Features.Peoples.ActivityBranchs.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.ActivityBranchs.UseCases.Queries
{
    public class GetBySearchActivityBranchRequest
        : IRequest<Result<Response<IEnumerable<ActivityBranchResponse>>>>
    {
        public BaseQuerySearch Query { get; set; } = new();
    }
}
