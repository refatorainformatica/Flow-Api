using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.Careers.Exceptions;
using Services.Features.Peoples.Careers.Models;
using Services.Features.Peoples.Careers.Repositories;
using Services.Features.Peoples.Careers.UseCases.Queries;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Extensions;

namespace Services.Features.Financials.Careers.UseCases.Queries
{
    public class GetBySearchCareerRequestHandler(
        IMapper mapper,
        IMediator mediator,
        CareerDbContext careerDbContext
    )
        : CommandHandler(careerDbContext, mediator),
            IRequestHandler<GetBySearchCareerRequest, Result<Response<IEnumerable<CareerResponse>>>>
    {
        private readonly CareerDbContext _careerDbContext = careerDbContext;

        public async Task<Result<Response<IEnumerable<CareerResponse>>>> Handle(
            GetBySearchCareerRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetBySearchCareerAsync(request)
                .BindAsync(careers => Task.FromResult(GenerateResponse(careers)));
        }

        private async Task<Result<Pagination<Career>>> GetBySearchCareerAsync(
            GetBySearchCareerRequest request
        )
        {
            var careers = await Task.Run(
                () =>
                    _careerDbContext
                        .Careers.AsNoTracking()
                        .SearchBy(request.Query.SearchText)
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<Career>()
            );

            return !careers.Rows.Any()
                ? Result<Pagination<Career>>.Failure(
                    CareerErrors.NotFound(request.Query.SearchText)
                )
                : Result<Pagination<Career>>.Success(careers);
        }

        private Result<Response<IEnumerable<CareerResponse>>> GenerateResponse(
            Pagination<Career> paginationCareer
        )
        {
            var careerResponse = mapper.Map<IEnumerable<CareerResponse>>(paginationCareer.Rows);
            var response = new Response<IEnumerable<CareerResponse>>(
                careerResponse,
                paginationCareer.Offset,
                paginationCareer.Limit,
                paginationCareer.PageCount,
                paginationCareer.RowCount
            );
            return Result<Response<IEnumerable<CareerResponse>>>.Success(response);
        }
    }
}
