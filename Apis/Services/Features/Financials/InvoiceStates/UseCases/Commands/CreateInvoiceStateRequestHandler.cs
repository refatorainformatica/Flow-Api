using AutoMapper;
using MediatR;
using Services.Features.Financials.InvoiceStates.Models;
using Services.Features.Financials.InvoiceStates.Models.Events;
using Services.Features.Financials.InvoiceStates.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.InvoiceStates.UseCases.Commands
{
    public class CreateInvoiceStateRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        InvoiceStateDbContext invoicestateDbContext
    )
        : CommandHandler(invoicestateDbContext, mediator),
            IRequestHandler<CreateInvoiceStateRequest, Result<Response<InvoiceStateResponse>>>
    {
        private readonly InvoiceStateDbContext _invoicestateDbContext = invoicestateDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<InvoiceStateResponse>>> Handle(
            CreateInvoiceStateRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveInvoiceStateAsync(request, cancellationToken)
                .BindAsync(invoicestate => Task.FromResult(GenerateResponse(invoicestate)));
        }

        private async Task<Result<InvoiceState>> SaveInvoiceStateAsync(
            CreateInvoiceStateRequest request,
            CancellationToken cancellationToken
        )
        {
            var newInvoiceState = new InvoiceState(
                0,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            newInvoiceState.AddEvent(new InvoiceStateCreatedEvent(newInvoiceState.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _invoicestateDbContext.InvoiceStates.AddAsync(
                        newInvoiceState,
                        cancellationToken: cancellationToken
                    );
                },
                newInvoiceState.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<InvoiceState>.Success(newInvoiceState);
        }

        private Result<Response<InvoiceStateResponse>> GenerateResponse(InvoiceState invoicestate)
        {
            var invoicestateResponse = mapper.Map<InvoiceStateResponse>(invoicestate);
            var response = new Response<InvoiceStateResponse>(invoicestateResponse);

            return Result<Response<InvoiceStateResponse>>.Success(response);
        }
    }
}
