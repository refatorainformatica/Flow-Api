using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.SkillStates.Exceptions;
using Services.Features.Peoples.SkillStates.Models;
using Services.Features.Peoples.SkillStates.Repositories;
using Services.Features.Peoples.SkillStates.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.SkillStates.UseCases.Queries
{
    public class GetByIdSkillStateRequestHandler(
        IMapper mapper,
        IMediator mediator,
        SkillStateDbContext skillstateDbContext
    )
        : CommandHandler(skillstateDbContext, mediator),
            IRequestHandler<GetByIdSkillStateRequest, Result<Response<SkillStateResponse>>>
    {
        private readonly SkillStateDbContext _skillstateDbContext = skillstateDbContext;

        public async Task<Result<Response<SkillStateResponse>>> Handle(
            GetByIdSkillStateRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdSkillStateAsync(request, cancellationToken)
                .BindAsync(skillstates => Task.FromResult(GenerateResponse(skillstates)));
        }

        private async Task<Result<SkillState>> GetByIdSkillStateAsync(
            GetByIdSkillStateRequest request,
            CancellationToken cancellationToken
        )
        {
            var skillstate = await _skillstateDbContext
                .SkillStates.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return skillstate is null
                ? Result<SkillState>.Failure(SkillStateErrors.NotFound(request.Id))
                : Result<SkillState>.Success(skillstate);
        }

        private Result<Response<SkillStateResponse>> GenerateResponse(SkillState skillstate)
        {
            var skillstateResponse = mapper.Map<SkillStateResponse>(skillstate);
            var response = new Response<SkillStateResponse>(skillstateResponse);
            return Result<Response<SkillStateResponse>>.Success(response);
        }
    }
}
