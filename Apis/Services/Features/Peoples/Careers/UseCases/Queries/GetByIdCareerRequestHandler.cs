using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.Careers.Exceptions;
using Services.Features.Peoples.Careers.Models;
using Services.Features.Peoples.Careers.Repositories;
using Services.Features.Peoples.Careers.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;

namespace Services.Features.Financials.Careers.UseCases.Queries
{
    public class GetByIdCareerRequestHandler(
        IMapper mapper,
        IMediator mediator,
        CareerDbContext careerDbContext
    )
        : CommandHandler(careerDbContext, mediator),
            IRequestHandler<GetByIdCareerRequest, Result<Response<CareerResponse>>>
    {
        private readonly CareerDbContext _careerDbContext = careerDbContext;

        public async Task<Result<Response<CareerResponse>>> Handle(
            GetByIdCareerRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetByIdCareerAsync(request, cancellationToken)
                .BindAsync(careers => Task.FromResult(GenerateResponse(careers)));
        }

        private async Task<Result<Career>> GetByIdCareerAsync(
            GetByIdCareerRequest request,
            CancellationToken cancellationToken
        )
        {
            var career = await _careerDbContext
                .Careers.AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && !x.DeletedAt.HasValue,
                    cancellationToken
                );

            return career is null
                ? Result<Career>.Failure(CareerErrors.NotFound(request.Id))
                : Result<Career>.Success(career);
        }

        private Result<Response<CareerResponse>> GenerateResponse(Career career)
        {
            var careerResponse = mapper.Map<CareerResponse>(career);
            var response = new Response<CareerResponse>(careerResponse);
            return Result<Response<CareerResponse>>.Success(response);
        }
    }
}
