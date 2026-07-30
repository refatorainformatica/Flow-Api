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
    public class RemoveInvoiceTypeRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        InvoiceTypeDbContext invoicetypeDbContext
    )
        : CommandHandler(invoicetypeDbContext, mediator),
            IRequestHandler<RemoveInvoiceTypeRequest, Result<Response<InvoiceTypeResponse>>>
    {
        private readonly InvoiceTypeDbContext _invoicetypeDbContext = invoicetypeDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<InvoiceTypeResponse>>> Handle(
            RemoveInvoiceTypeRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentInvoiceTypeAsync(req.Id, cancellationToken))
                .BindAsync(currentInvoiceType =>
                    RemoveInvoiceTypeAsync(currentInvoiceType, cancellationToken)
                )
                .MapAsync(currentInvoiceType =>
                {
                    return new Response<InvoiceTypeResponse>(null);
                });
        }

        private static Result<RemoveInvoiceTypeRequest> ValidateRequest(
            RemoveInvoiceTypeRequest request
        )
        {
            return request.Id == default
                ? Result<RemoveInvoiceTypeRequest>.Failure(InvoiceTypeErrors.NotFound(request.Id))
                : Result<RemoveInvoiceTypeRequest>.Success(request);
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

        private async Task<Result<InvoiceType>> RemoveInvoiceTypeAsync(
            InvoiceType removeInvoiceType,
            CancellationToken cancellationToken
        )
        {
            removeInvoiceType.DeletedAt = _dateTimeService.UtcNow;
            removeInvoiceType.EditedAt = _dateTimeService.UtcNow;
            removeInvoiceType.EditedBy = _authenticatedUserService.UserId;

            removeInvoiceType.AddEvent(new InvoiceTypeRemovedEvent(removeInvoiceType.Id));

            await ExecuteTransactionAsync(
                () => _invoicetypeDbContext.Update(removeInvoiceType),
                removeInvoiceType.GetEvents(),
                cancellationToken
            );

            return Result<InvoiceType>.Success(removeInvoiceType);
        }
    }
}
