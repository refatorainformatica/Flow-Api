using MediatR;
using Services.Features.Peoples.ActivityBranchs.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.ActivityBranchs.UseCases.Commands
{
    public class CreateActivityBranchRequest
        : ActivityBranchRequest,
            IRequest<Result<Response<ActivityBranchResponse>>> { }
}
