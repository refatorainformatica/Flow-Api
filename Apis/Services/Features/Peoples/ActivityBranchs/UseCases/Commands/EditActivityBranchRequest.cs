using MediatR;
using Services.Features.Peoples.ActivityBranchs.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.ActivityBranchs.UseCases.Commands
{
    public class EditActivityBranchRequest
        : ActivityBranchRequest,
            IRequest<Result<Response<ActivityBranchResponse>>>
    {
        public int RequestId { get; set; }
    }
}
