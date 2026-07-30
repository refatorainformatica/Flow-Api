using MediatR;
using Services.Features.Peoples.ActivityBranchs.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.ActivityBranchs.UseCases.Commands
{
    public class RemoveActivityBranchRequest : IRequest<Result<Response<ActivityBranchResponse>>>
    {
        public int Id { get; set; }
    }
}
