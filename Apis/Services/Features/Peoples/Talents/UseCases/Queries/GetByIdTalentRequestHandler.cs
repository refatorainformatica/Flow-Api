using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.Talents.Exceptions;
using Services.Features.Peoples.Talents.Models;
using Services.Features.Peoples.Talents.Repositories;
using Services.Features.Peoples.Talents.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Talents.UseCases.Queries
{
    public class GetByIdTalentRequestHandler(
        IMapper mapper,
        IMediator mediator,
        TalentDbContext talentDbContext
    )
        : CommandHandler(talentDbContext, mediator),
            IRequestHandler<GetByIdTalentRequest, Result<Response<TalentResponse>>>
    {
        private readonly TalentDbContext _talentDbContext = talentDbContext;

        public async Task<Result<Response<TalentResponse>>> Handle(
            GetByIdTalentRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdTalentAsync(request, cancellationToken)
                .BindAsync(talents => Task.FromResult(GenerateResponse(talents)));
        }

        private async Task<Result<Talent>> GetByIdTalentAsync(
            GetByIdTalentRequest request,
            CancellationToken cancellationToken
        )
        {
            var talent = await _talentDbContext
                .Talents.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return talent is null
                ? Result<Talent>.Failure(TalentErrors.NotFound(request.Id))
                : Result<Talent>.Success(talent);
        }

        private Result<Response<TalentResponse>> GenerateResponse(Talent talent)
        {
            var talentResponse = mapper.Map<TalentResponse>(talent);
            var response = new Response<TalentResponse>(talentResponse);
            return Result<Response<TalentResponse>>.Success(response);
        }
    }
}
