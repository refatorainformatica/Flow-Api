using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.JuridicalNatures.Exceptions;
using Services.Features.Peoples.JuridicalNatures.Models;
using Services.Features.Peoples.JuridicalNatures.Repositories;
using Services.Features.Peoples.JuridicalNatures.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.JuridicalNatures.UseCases.Queries
{
    public class GetByIdJuridicalNatureRequestHandler(
        IMapper mapper,
        IMediator mediator,
        JuridicalNatureDbContext juridicalnatureDbContext
    )
        : CommandHandler(juridicalnatureDbContext, mediator),
            IRequestHandler<
                GetByIdJuridicalNatureRequest,
                Result<Response<JuridicalNatureResponse>>
            >
    {
        private readonly JuridicalNatureDbContext _juridicalnatureDbContext =
            juridicalnatureDbContext;

        public async Task<Result<Response<JuridicalNatureResponse>>> Handle(
            GetByIdJuridicalNatureRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdJuridicalNatureAsync(request, cancellationToken)
                .BindAsync(juridicalnatures => Task.FromResult(GenerateResponse(juridicalnatures)));
        }

        private async Task<Result<JuridicalNature>> GetByIdJuridicalNatureAsync(
            GetByIdJuridicalNatureRequest request,
            CancellationToken cancellationToken
        )
        {
            var juridicalnature = await _juridicalnatureDbContext
                .JuridicalNatures.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return juridicalnature is null
                ? Result<JuridicalNature>.Failure(JuridicalNatureErrors.NotFound(request.Id))
                : Result<JuridicalNature>.Success(juridicalnature);
        }

        private Result<Response<JuridicalNatureResponse>> GenerateResponse(
            JuridicalNature juridicalnature
        )
        {
            var juridicalnatureResponse = mapper.Map<JuridicalNatureResponse>(juridicalnature);
            var response = new Response<JuridicalNatureResponse>(juridicalnatureResponse);
            return Result<Response<JuridicalNatureResponse>>.Success(response);
        }
    }
}
