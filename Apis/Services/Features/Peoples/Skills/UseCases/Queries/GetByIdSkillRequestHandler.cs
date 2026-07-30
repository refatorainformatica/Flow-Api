using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.Skills.Exceptions;
using Services.Features.Peoples.Skills.Models;
using Services.Features.Peoples.Skills.Repositories;
using Services.Features.Peoples.Skills.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Skills.UseCases.Queries
{
    public class GetByIdSkillRequestHandler(
        IMapper mapper,
        IMediator mediator,
        SkillDbContext skillDbContext
    )
        : CommandHandler(skillDbContext, mediator),
            IRequestHandler<GetByIdSkillRequest, Result<Response<SkillResponse>>>
    {
        private readonly SkillDbContext _skillDbContext = skillDbContext;

        public async Task<Result<Response<SkillResponse>>> Handle(
            GetByIdSkillRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdSkillAsync(request, cancellationToken)
                .BindAsync(skills => Task.FromResult(GenerateResponse(skills)));
        }

        private async Task<Result<Skill>> GetByIdSkillAsync(
            GetByIdSkillRequest request,
            CancellationToken cancellationToken
        )
        {
            var skill = await _skillDbContext
                .Skills.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return skill is null
                ? Result<Skill>.Failure(SkillErrors.NotFound(request.Id))
                : Result<Skill>.Success(skill);
        }

        private Result<Response<SkillResponse>> GenerateResponse(Skill skill)
        {
            var skillResponse = mapper.Map<SkillResponse>(skill);
            var response = new Response<SkillResponse>(skillResponse);
            return Result<Response<SkillResponse>>.Success(response);
        }
    }
}
