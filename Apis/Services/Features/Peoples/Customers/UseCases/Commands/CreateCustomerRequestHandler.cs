using AutoMapper;
using MediatR;
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
    public class CreateCustomerRequestHandler(
        IAuthenticatedUserService authenticatedUserService,
        IDateTimeService dateTimeService,
        IMapper mapper,
        IMediator mediator,
        CustomerDbContext customerDbContext
    )
        : CommandHandler(customerDbContext, mediator),
            IRequestHandler<CreateCustomerRequest, Result<Response<CustomerResponse>>>
    {
        private readonly CustomerDbContext _customerDbContext = customerDbContext;

        private readonly IAuthenticatedUserService _authenticatedUserService =
            authenticatedUserService;

        private readonly IDateTimeService _dateTimeService = dateTimeService;

        public async Task<Result<Response<CustomerResponse>>> Handle(
            CreateCustomerRequest request,
            CancellationToken cancellationToken
        )
        {
            return await SaveCustomerAsync(request, cancellationToken)
                .BindAsync(customer => Task.FromResult(GenerateResponse(customer)));
        }

        private async Task<Result<Customer>> SaveCustomerAsync(
            CreateCustomerRequest request,
            CancellationToken cancellationToken
        )
        {
            var newCustomer = new Customer(
                0,
                request.Name,
                request.AddressLine1,
                request.AddressLine2,
                request.Email,
                request.PhoneNumber,
                request.Picture ?? Shared.Infrastructure.Resources.Images.PictureBase64Image,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId,
                _dateTimeService.UtcNow,
                _authenticatedUserService.UserId
            )
            {
                Documents = request
                    .Documents.Select(document => new CustomerDocument()
                    {
                        CustomerId = document.CustomerId,
                        DocumentTypeId = document.DocumentTypeId,
                        EnrollmentCode = document.EnrollmentCode,
                        EnrollmentDate = document.EnrollmentDate,
                        File = document.File,
                        Picture =
                            document.Picture
                            ?? Shared.Infrastructure.Resources.Images.DocumentBase64Image,
                        CreatedAt = _dateTimeService.UtcNow,
                        CreatedBy = _authenticatedUserService.UserId,
                        EditedAt = _dateTimeService.UtcNow,
                        EditedBy = _authenticatedUserService.UserId,
                    })
                    .ToList(),
            };

            newCustomer.AddEvent(new CustomerCreatedEvent(newCustomer.Id));

            await ExecuteTransactionAsync(
                async () =>
                {
                    await _customerDbContext.Customers.AddAsync(
                        newCustomer,
                        cancellationToken: cancellationToken
                    );
                },
                newCustomer.GetEvents(),
                cancellationToken: cancellationToken
            );

            return Result<Customer>.Success(newCustomer);
        }

        private Result<Response<CustomerResponse>> GenerateResponse(Customer customer)
        {
            var customerResponse = mapper.Map<CustomerResponse>(customer);
            var response = new Response<CustomerResponse>(customerResponse);

            return Result<Response<CustomerResponse>>.Success(response);
        }
    }
}
