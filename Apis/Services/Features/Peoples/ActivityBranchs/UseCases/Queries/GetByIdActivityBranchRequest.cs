using MediatR;
using Services.Features.Peoples.ActivityBranchs.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.ActivityBranchs.UseCases.Queries
{
    public class GetByIdActivityBranchRequest : IRequest<Result<Response<ActivityBranchResponse>>>
    {
        public int Id { get; set; }
    }
}
