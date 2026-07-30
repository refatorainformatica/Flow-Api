using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.SkillTypes.Exceptions;
using Services.Features.Peoples.SkillTypes.Models;
using Services.Features.Peoples.SkillTypes.Repositories;
using Services.Features.Peoples.SkillTypes.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.SkillTypes.UseCases.Queries
{
    public class GetByIdSkillTypeRequestHandler(
        IMapper mapper,
        IMediator mediator,
        SkillTypeDbContext skilltypeDbContext
    )
        : CommandHandler(skilltypeDbContext, mediator),
            IRequestHandler<GetByIdSkillTypeRequest, Result<Response<SkillTypeResponse>>>
    {
        private readonly SkillTypeDbContext _skilltypeDbContext = skilltypeDbContext;

        public async Task<Result<Response<SkillTypeResponse>>> Handle(
            GetByIdSkillTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdSkillTypeAsync(request, cancellationToken)
                .BindAsync(skilltypes => Task.FromResult(GenerateResponse(skilltypes)));
        }

        private async Task<Result<SkillType>> GetByIdSkillTypeAsync(
            GetByIdSkillTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            var skilltype = await _skilltypeDbContext
                .SkillTypes.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return skilltype is null
                ? Result<SkillType>.Failure(SkillTypeErrors.NotFound(request.Id))
                : Result<SkillType>.Success(skilltype);
        }

        private Result<Response<SkillTypeResponse>> GenerateResponse(SkillType skilltype)
        {
            var skilltypeResponse = mapper.Map<SkillTypeResponse>(skilltype);
            var response = new Response<SkillTypeResponse>(skilltypeResponse);
            return Result<Response<SkillTypeResponse>>.Success(response);
        }
    }
}
