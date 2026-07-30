using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.EducationLevels.Exceptions;
using Services.Features.Peoples.EducationLevels.Models;
using Services.Features.Peoples.EducationLevels.Repositories;
using Services.Features.Peoples.EducationLevels.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.EducationLevels.UseCases.Queries
{
    public class GetByIdEducationLevelRequestHandler(
        IMapper mapper,
        IMediator mediator,
        EducationLevelDbContext educationlevelDbContext
    )
        : CommandHandler(educationlevelDbContext, mediator),
            IRequestHandler<GetByIdEducationLevelRequest, Result<Response<EducationLevelResponse>>>
    {
        private readonly EducationLevelDbContext _educationlevelDbContext = educationlevelDbContext;

        public async Task<Result<Response<EducationLevelResponse>>> Handle(
            GetByIdEducationLevelRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdEducationLevelAsync(request, cancellationToken)
                .BindAsync(educationlevels => Task.FromResult(GenerateResponse(educationlevels)));
        }

        private async Task<Result<EducationLevel>> GetByIdEducationLevelAsync(
            GetByIdEducationLevelRequest request,
            CancellationToken cancellationToken
        )
        {
            var educationlevel = await _educationlevelDbContext
                .EducationLevels.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return educationlevel is null
                ? Result<EducationLevel>.Failure(EducationLevelErrors.NotFound(request.Id))
                : Result<EducationLevel>.Success(educationlevel);
        }

        private Result<Response<EducationLevelResponse>> GenerateResponse(
            EducationLevel educationlevel
        )
        {
            var educationlevelResponse = mapper.Map<EducationLevelResponse>(educationlevel);
            var response = new Response<EducationLevelResponse>(educationlevelResponse);
            return Result<Response<EducationLevelResponse>>.Success(response);
        }
    }
}
