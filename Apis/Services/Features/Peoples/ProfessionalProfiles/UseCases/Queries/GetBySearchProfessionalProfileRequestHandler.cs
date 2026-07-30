using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.ProfessionalProfiles.Exceptions;
using Services.Features.Peoples.ProfessionalProfiles.Models;
using Services.Features.Peoples.ProfessionalProfiles.Repositories;
using Services.Features.Peoples.ProfessionalProfiles.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.ProfessionalProfiles.UseCases.Queries
{
    public class GetBySearchProfessionalProfileRequestHandler(
        IMapper mapper,
        IMediator mediator,
        ProfessionalProfileDbContext professionalprofileDbContext
    )
        : CommandHandler(professionalprofileDbContext, mediator),
            IRequestHandler<
                GetBySearchProfessionalProfileRequest,
                Result<Response<IEnumerable<ProfessionalProfileResponse>>>
            >
    {
        private readonly ProfessionalProfileDbContext _professionalprofileDbContext =
            professionalprofileDbContext;

        public async Task<Result<Response<IEnumerable<ProfessionalProfileResponse>>>> Handle(
            GetBySearchProfessionalProfileRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetBySearchProfessionalProfileAsync(request)
                .BindAsync(professionalprofiles =>
                    Task.FromResult(GenerateResponse(professionalprofiles))
                );
        }

        private async Task<
            Result<Pagination<ProfessionalProfile>>
        > GetBySearchProfessionalProfileAsync(GetBySearchProfessionalProfileRequest request)
        {
            var professionalprofiles = await Task.Run(
                () =>
                    _professionalprofileDbContext
                        .ProfessionalProfiles.AsNoTracking()
                        .SearchBy(request.Query.SearchText)
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<ProfessionalProfile>()
            );

            return !professionalprofiles.Rows.Any()
                ? Result<Pagination<ProfessionalProfile>>.Failure(
                    ProfessionalProfileErrors.NotFound(request.Query.SearchText)
                )
                : Result<Pagination<ProfessionalProfile>>.Success(professionalprofiles);
        }

        private Result<Response<IEnumerable<ProfessionalProfileResponse>>> GenerateResponse(
            Pagination<ProfessionalProfile> paginationProfessionalProfile
        )
        {
            var professionalprofileResponse = mapper.Map<IEnumerable<ProfessionalProfileResponse>>(
                paginationProfessionalProfile.Rows
            );
            var response = new Response<IEnumerable<ProfessionalProfileResponse>>(
                professionalprofileResponse,
                paginationProfessionalProfile.Offset,
                paginationProfessionalProfile.Limit,
                paginationProfessionalProfile.PageCount,
                paginationProfessionalProfile.RowCount
            );
            return Result<Response<IEnumerable<ProfessionalProfileResponse>>>.Success(response);
        }
    }
}
