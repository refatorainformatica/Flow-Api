using MediatR;
using Services.Features.Peoples.Careers.Models;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Peoples.Careers.UseCases.Queries
{
    public class GetByIdCareerRequest : IRequest<Result<Response<CareerResponse>>>
    {
        public int Id { get; set; }
    }
}
