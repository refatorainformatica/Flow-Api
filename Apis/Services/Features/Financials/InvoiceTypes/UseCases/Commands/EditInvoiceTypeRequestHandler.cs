using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Financials.InvoiceTypes.Exceptions;
using Services.Features.Financials.InvoiceTypes.Models;
using Services.Features.Financials.InvoiceTypes.Models.Events;
using Services.Features.Financials.InvoiceTypes.Repositories;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.InvoiceTypes.UseCases.Commands
{
    public class EditInvoiceTypeRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        InvoiceTypeDbContext invoicetypeDbContext
    )
        : CommandHandler(invoicetypeDbContext, mediator),
            IRequestHandler<EditInvoiceTypeRequest, Result<Response<InvoiceTypeResponse>>>
    {
        private readonly InvoiceTypeDbContext _invoicetypeDbContext = invoicetypeDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<InvoiceTypeResponse>>> Handle(
            EditInvoiceTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentInvoiceTypeAsync(req.Id, cancellationToken))
                .BindAsync(currentInvoiceType =>
                    EditAndSaveInvoiceTypeAsync(currentInvoiceType, request, cancellationToken)
                )
                .MapAsync(currentInvoiceType =>
                {
                    return new Response<InvoiceTypeResponse>(null);
                });
        }

        private static Result<EditInvoiceTypeRequest> ValidateRequest(
            EditInvoiceTypeRequest request
        )
        {
            return request.Id != request.RequestId
                ? Result<EditInvoiceTypeRequest>.Failure(
                    InvoiceTypeErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditInvoiceTypeRequest>.Success(request);
        }

        private async Task<Result<InvoiceType>> GetCurrentInvoiceTypeAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var invoicetype = await _invoicetypeDbContext
                .InvoiceTypes.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return invoicetype is null
                ? Result<InvoiceType>.Failure(InvoiceTypeErrors.NotFound(id))
                : Result<InvoiceType>.Success(invoicetype);
        }

        private async Task<Result<InvoiceType>> EditAndSaveInvoiceTypeAsync(
            InvoiceType currentInvoiceType,
            EditInvoiceTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            var editInvoiceType = new InvoiceType(
                request.Id,
                request.Description,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                currentInvoiceType.CreatedAt.GetValueOrDefault(),
                currentInvoiceType.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            );

            editInvoiceType.AddEvent(new InvoiceTypeEditedEvent(editInvoiceType.Id));

            await ExecuteTransactionAsync(
                () => _invoicetypeDbContext.InvoiceTypes.Update(editInvoiceType),
                editInvoiceType.GetEvents(),
                cancellationToken
            );

            return Result<InvoiceType>.Success(editInvoiceType);
        }
    }
}
