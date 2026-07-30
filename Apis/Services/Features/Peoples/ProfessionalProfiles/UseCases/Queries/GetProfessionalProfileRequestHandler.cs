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
    public class GetProfessionalProfileRequestHandler(
        IMapper mapper,
        IMediator mediator,
        ProfessionalProfileDbContext professionalprofileDbContext
    )
        : CommandHandler(professionalprofileDbContext, mediator),
            IRequestHandler<
                GetProfessionalProfileRequest,
                Result<Response<IEnumerable<ProfessionalProfileResponse>>>
            >
    {
        private readonly ProfessionalProfileDbContext _professionalprofileDbContext =
            professionalprofileDbContext;

        public async Task<Result<Response<IEnumerable<ProfessionalProfileResponse>>>> Handle(
            GetProfessionalProfileRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetProfessionalProfileAsync(request)
                .BindAsync(professionalprofiles =>
                    Task.FromResult(GenerateResponse(professionalprofiles))
                );
        }

        private async Task<Result<Pagination<ProfessionalProfile>>> GetProfessionalProfileAsync(
            GetProfessionalProfileRequest request
        )
        {
            var professionalprofiles = await Task.Run(
                () =>
                    _professionalprofileDbContext
                        .ProfessionalProfiles.AsNoTracking()
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<ProfessionalProfile>()
            );

            return !professionalprofiles.Rows.Any()
                ? Result<Pagination<ProfessionalProfile>>.Failure(
                    ProfessionalProfileErrors.IsEmpty()
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
