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
    public class GetCareerRequestHandler(
        IMapper mapper,
        IMediator mediator,
        CareerDbContext careerDbContext
    )
        : CommandHandler(careerDbContext, mediator),
            IRequestHandler<GetCareerRequest, Result<Response<IEnumerable<CareerResponse>>>>
    {
        private readonly CareerDbContext _careerDbContext = careerDbContext;

        public async Task<Result<Response<IEnumerable<CareerResponse>>>> Handle(
            GetCareerRequest request,
            CancellationToken cancellationToken
        )
        {
            return await GetCareerAsync(request)
                .BindAsync(careers => Task.FromResult(GenerateResponse(careers)));
        }

        private async Task<Result<Pagination<Career>>> GetCareerAsync(GetCareerRequest request)
        {
            var careers = await Task.Run(
                () =>
                    _careerDbContext
                        .Careers.AsNoTracking()
                        .Where(x => !x.DeletedAt.HasValue)
                        .SortBy(request.Query.SortBy.ToString(), request.Query.SortOrderAscending)
                        .CreatePagination(request.Query.Offset, request.Query.Limit)
                    ?? new Pagination<Career>()
            );

            return !careers.Rows.Any()
                ? Result<Pagination<Career>>.Failure(CareerErrors.IsEmpty())
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
