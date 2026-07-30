//using AutoMapper;
//using MediatR;
//using Services.Features.Financials.Revenues.Models;
//using Services.Features.Financials.Revenues.Models.Events;
//using Services.Features.Financials.Revenues.Repositories;
//using Shared.Domain.Abstractions.Bus;
//using Shared.Domain.Abstractions.DateTime;
//using Shared.Domain.Abstractions.Primitives;
//using Shared.Domain.Abstractions.Security;
//
//namespace Services.Features.Financials.Revenues.UseCases.Commands
//{
//    public class CreateRevenueRequestHandler(
//        IAuthenticatedUserService authenticatedUserService,
//        IDateTimeService dateTimeService,
//        IMapper mapper,
//        IMediator mediator,
//        RevenueDbContext revenueDbContext
//    )
//        : CommandHandler(revenueDbContext, mediator),
//            IRequestHandler<CreateRevenueRequest, Result<Response<RevenueResponse>>>
//    {
//        private readonly RevenueDbContext _revenueDbContext = revenueDbContext;
//
//        private readonly IAuthenticatedUserService _authenticatedUserService =
//            authenticatedUserService;
//
//        private readonly IDateTimeService _dateTimeService = dateTimeService;
//
//        public async Task<Result<Response<RevenueResponse>>> Handle(
//            CreateRevenueRequest request,
//            CancellationToken cancellationToken
//        )
//        {
//            return await SaveRevenueAsync(request, cancellationToken)
//                .BindAsync(revenue => Task.FromResult(GenerateResponse(revenue)));
//        }
//
//        private async Task<Result<Revenue>> SaveRevenueAsync(
//            CreateRevenueRequest request,
//            CancellationToken cancellationToken
//        )
//        {
//            var newRevenue = new Revenue(
//                0,
//                request.InvoiceId,
//                request.DateOfIssue,
//                request.DateOfDue,
//                request.DateOfPayment,
//                request.InstallmentNumber,
//                request.TotalNumberOfInstallments,
//                request.PaymentValue,
//                request.PaymentDiscountValue,
//                request.TotalPaymentValue,
//                request.BarCode,
//                request.Observation,
//                request.CostCenterId,
//                request.PaymentStateId,
//                request.RevenueTypeId,
//                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
//                _dateTimeService.UtcNow,
//                _authenticatedUserService.UserId,
//                _dateTimeService.UtcNow,
//                _authenticatedUserService.UserId
//            );
//
//            newRevenue.AddEvent(new RevenueCreatedEvent(newRevenue.Id));
//
//            await ExecuteTransactionAsync(
//                async () =>
//                {
//                    await _revenueDbContext.Revenues.AddAsync(
//                        newRevenue,
//                        cancellationToken: cancellationToken
//                    );
//                },
//                newRevenue.GetEvents(),
//                cancellationToken: cancellationToken
//            );
//
//            return Result<Revenue>.Success(newRevenue);
//        }
//
//        private Result<Response<RevenueResponse>> GenerateResponse(Revenue revenue)
//        {
//            var revenueResponse = mapper.Map<RevenueResponse>(revenue);
//            var response = new Response<RevenueResponse>(revenueResponse);
//
//            return Result<Response<RevenueResponse>>.Success(response);
//        }
//    }
//}
using AutoMapper;
using MediatR;
using Services.Features.Financials.Revenues.Models;
using Services.Features.Financials.Revenues.Models.Events;
using Services.Features.Financials.Revenues.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.Revenues.UseCases.Commands
{
    public class CreateRevenueRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        RevenueDbContext revenueDbContext
    )
        : CommandHandler(revenueDbContext, mediator),
            IRequestHandler<CreateRevenueRequest, Result<Response<RevenueResponse>>>
    {
        private readonly RevenueDbContext _revenueDbContext = revenueDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<RevenueResponse>>> Handle(
            CreateRevenueRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveRevenueAsync(request, cancellationToken)
                .BindAsync(revenue => Task.FromResult(GenerateResponse(revenue)));
        }

        private async Task<Result<Revenue>> SaveRevenueAsync(
            CreateRevenueRequest request,
            CancellationToken cancellationToken
        )
        {
            var newRevenue = new Revenue(
                0,
                request.InvoiceId,
                request.DateOfIssue,
                request.DateOfDue,
                request.DateOfPayment,
                request.InstallmentNumber,
                request.TotalNumberOfInstallments,
                request.PaymentValue,
                request.PaymentDiscountValue,
                request.TotalPaymentValue,
                request.BarCode,
                request.Observation,
                request.CostCenterId,
                request.PaymentStateId,
                request.RevenueTypeId,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            newRevenue.AddEvent(new RevenueCreatedEvent(newRevenue.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _revenueDbContext.Revenues.AddAsync(
                        newRevenue,
                        cancellationToken: cancellationToken
                    );
                },
                newRevenue.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<Revenue>.Success(newRevenue);
        }

        private Result<Response<RevenueResponse>> GenerateResponse(Revenue revenue)
        {
            var revenueResponse = mapper.Map<RevenueResponse>(revenue);
            var response = new Response<RevenueResponse>(revenueResponse);

            return Result<Response<RevenueResponse>>.Success(response);
        }
    }
}
