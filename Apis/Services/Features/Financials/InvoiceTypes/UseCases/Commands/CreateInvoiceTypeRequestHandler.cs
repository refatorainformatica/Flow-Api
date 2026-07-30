using AutoMapper;
using MediatR;
using Services.Features.Financials.InvoiceTypes.Models;
using Services.Features.Financials.InvoiceTypes.Models.Events;
using Services.Features.Financials.InvoiceTypes.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.InvoiceTypes.UseCases.Commands
{
    public class CreateInvoiceTypeRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        InvoiceTypeDbContext invoicetypeDbContext
    )
        : CommandHandler(invoicetypeDbContext, mediator),
            IRequestHandler<CreateInvoiceTypeRequest, Result<Response<InvoiceTypeResponse>>>
    {
        private readonly InvoiceTypeDbContext _invoicetypeDbContext = invoicetypeDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<InvoiceTypeResponse>>> Handle(
            CreateInvoiceTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveInvoiceTypeAsync(request, cancellationToken)
                .BindAsync(invoicetype => Task.FromResult(GenerateResponse(invoicetype)));
        }

        private async Task<Result<InvoiceType>> SaveInvoiceTypeAsync(
            CreateInvoiceTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            var newInvoiceType = new InvoiceType(
                0,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            newInvoiceType.AddEvent(new InvoiceTypeCreatedEvent(newInvoiceType.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _invoicetypeDbContext.InvoiceTypes.AddAsync(
                        newInvoiceType,
                        cancellationToken: cancellationToken
                    );
                },
                newInvoiceType.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<InvoiceType>.Success(newInvoiceType);
        }

        private Result<Response<InvoiceTypeResponse>> GenerateResponse(InvoiceType invoicetype)
        {
            var invoicetypeResponse = mapper.Map<InvoiceTypeResponse>(invoicetype);
            var response = new Response<InvoiceTypeResponse>(invoicetypeResponse);

            return Result<Response<InvoiceTypeResponse>>.Success(response);
        }
    }
}
