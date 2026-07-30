using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.SkillLevels.Exceptions;
using Services.Features.Peoples.SkillLevels.Models;
using Services.Features.Peoples.SkillLevels.Repositories;
using Services.Features.Peoples.SkillLevels.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.SkillLevels.UseCases.Queries
{
    public class GetByIdSkillLevelRequestHandler(
        IMapper mapper,
        IMediator mediator,
        SkillLevelDbContext skilllevelDbContext
    )
        : CommandHandler(skilllevelDbContext, mediator),
            IRequestHandler<GetByIdSkillLevelRequest, Result<Response<SkillLevelResponse>>>
    {
        private readonly SkillLevelDbContext _skilllevelDbContext = skilllevelDbContext;

        public async Task<Result<Response<SkillLevelResponse>>> Handle(
            GetByIdSkillLevelRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdSkillLevelAsync(request, cancellationToken)
                .BindAsync(skilllevels => Task.FromResult(GenerateResponse(skilllevels)));
        }

        private async Task<Result<SkillLevel>> GetByIdSkillLevelAsync(
            GetByIdSkillLevelRequest request,
            CancellationToken cancellationToken
        )
        {
            var skilllevel = await _skilllevelDbContext
                .SkillLevels.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return skilllevel is null
                ? Result<SkillLevel>.Failure(SkillLevelErrors.NotFound(request.Id))
                : Result<SkillLevel>.Success(skilllevel);
        }

        private Result<Response<SkillLevelResponse>> GenerateResponse(SkillLevel skilllevel)
        {
            var skilllevelResponse = mapper.Map<SkillLevelResponse>(skilllevel);
            var response = new Response<SkillLevelResponse>(skilllevelResponse);
            return Result<Response<SkillLevelResponse>>.Success(response);
        }
    }
}
