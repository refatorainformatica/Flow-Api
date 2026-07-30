using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Features.Peoples.Customers.Exceptions;
using Services.Features.Peoples.Customers.Models;
using Services.Features.Peoples.Customers.Models.Events;
using Services.Features.Peoples.Customers.Repositories;
using Services.Features.Peoples.Customers.UseCases.Commands;
using Shared.Domain.Abstractions.Bus;
using Shared.Domain.Abstractions.DateTime;
using Shared.Domain.Abstractions.Primitives;
using Shared.Domain.Abstractions.Security;

namespace Services.Features.Financials.Customers.UseCases.Commands
{
    public class EditCustomerRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMediator mediator,
        CustomerDbContext customerDbContext
    )
        : CommandHandler(customerDbContext, mediator),
            IRequestHandler<EditCustomerRequest, Result<Response<CustomerResponse>>>
    {
        private readonly CustomerDbContext _customerDbContext = customerDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<CustomerResponse>>> Handle(
            EditCustomerRequest request,
            CancellationToken cancellationToken
        )
        {
            return await ValidateRequest(request)
                .BindAsync(req => GetCurrentCustomerAsync(req.Id, cancellationToken))
                .BindAsync(currentCustomer =>
                    EditAndSaveCustomerAsync(currentCustomer, request, cancellationToken)
                )
                .MapAsync(currentCustomer =>
                {
                    return new Response<CustomerResponse>(null);
                });
        }

        private static Result<EditCustomerRequest> ValidateRequest(EditCustomerRequest request)
        {
            return request.Id != request.RequestId
                ? Result<EditCustomerRequest>.Failure(
                    CustomerErrors.PreConditionFailed(request.RequestId)
                )
                : Result<EditCustomerRequest>.Success(request);
        }

        private async Task<Result<Customer>> GetCurrentCustomerAsync(
            int id,
            CancellationToken cancellationToken
        )
        {
            var customer = await _customerDbContext
                .Customers.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.DeletedAt.HasValue, cancellationToken);

            return customer is null
                ? Result<Customer>.Failure(CustomerErrors.NotFound(id))
                : Result<Customer>.Success(customer);
        }

        private async Task<Result<Customer>> EditAndSaveCustomerAsync(
            Customer currentCustomer,
            EditCustomerRequest request,
            CancellationToken cancellationToken
        )
        {
            var editCustomer = new Customer(
                request.Id,
                request.Name,
                request.AddressLine1,
                request.AddressLine2,
                request.Email,
                request.PhoneNumber,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                currentCustomer.CreatedAt.GetValueOrDefault(),
                currentCustomer.CreatedBy,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            )
            {
                Documents = request
                    .Documents.Select(document => new CustomerDocument()
                    {
                        Id =
                            currentCustomer
                                .Documents.FirstOrDefault(x =>
                                    x.DocumentTypeId == document.DocumentTypeId
                                )
                                ?.Id ?? 0,
                        CustomerId = document.CustomerId,
                        DocumentTypeId = document.DocumentTypeId,
                        EnrollmentCode = document.EnrollmentCode,
                        EnrollmentDate = document.EnrollmentDate,
                        File = document.File,
                        Picture =
                            document.Picture
                            ?? Shared.Infrastructure.Resources.Images.DocumentBase64Image,
                        CreatedAt =
                            currentCustomer
                                .Documents.FirstOrDefault(x =>
                                    x.DocumentTypeId == document.DocumentTypeId
                                )
                                ?.CreatedAt ?? _dateTimeService.UtcNow,
                        CreatedBy =
                            currentCustomer
                                .Documents.FirstOrDefault(x =>
                                    x.DocumentTypeId == document.DocumentTypeId
                                )
                                ?.CreatedBy ?? _authenticatedUserService.UserId,
                        EditedAt = _dateTimeService.UtcNow,
                        EditedBy = _authenticatedUserService.UserId,
                        DeletedAt = document.DeletedAt,
                    })
                    .ToList(),
            };

            editCustomer.AddEvent(new CustomerEditedEvent(editCustomer.Id));

            await ExecuteTransactionAsync(
                () => _customerDbContext.Customers.Update(editCustomer),
                editCustomer.GetEvents(),
                cancellationToken
            );

            return Result<Customer>.Success(editCustomer);
        }
    }
}
