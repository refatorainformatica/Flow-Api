using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Features.Financials.InvoiceStates.Models;
using Services.Features.Financials.InvoiceStates.UseCases.Commands;
using Services.Features.Financials.InvoiceStates.UseCases.Queries;
using Shared.Domain.Abstractions.Enumerations;
using Shared.Domain.Abstractions.Primitives;

namespace Application.Controllers.v1.Financials
{
    /// <summary>
    /// The InvoiceStateController class handles the operations related to invoice state transactions.
    /// </summary>
    [Authorize]
    [ApiVersion("1.0")]
    [Tags("Financials - Invoices Endpoints")]
    [ApiExplorerSettings(GroupName = "Invoices")]
    [Route("api/v{version:apiVersion}/invoice-states")]
    public class InvoiceStateController : BaseApiController
    {
        /// <summary>
        /// Retrieves all invoice states based on the specified request.
        /// </summary>
        /// <param name="offset">The number of items to skip before starting to collect the result set.</param>
        /// <param name="limit">The number of items to return.</param>
        /// <param name="sortBy">The field by which to sort the results.</param>
        /// <param name="sortOrderAscending">Indicates whether the sorting should be in ascending order.</param>
        /// <returns>A response containing a list of invoice states.</returns>
        [HttpGet]
        [ProducesResponseType(
            typeof(Response<IEnumerable<InvoiceStateResponse>>),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllInvoiceStatesAsync(
            [FromQuery] int offset,
            [FromQuery] int limit,
            [FromQuery] SortBy sortBy,
            [FromQuery] bool sortOrderAscending
        ) =>
            BuildResponse(
                await Mediator.Send(
                    new GetInvoiceStateRequest
                    {
                        Query = new BaseQuery
                        {
                            Offset = offset,
                            Limit = limit,
                            SortBy = sortBy,
                            SortOrderAscending = sortOrderAscending,
                        },
                    }
                )
            );

        /// <summary>
        /// Gets a invoice state by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the invoice state.</param>
        /// <returns>An <see cref="IActionResult"/> containing the invoice state details.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(InvoiceStateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetInvoiceStateByIdAsync([FromRoute] int id) =>
            BuildResponse(await Mediator.Send(new GetByIdInvoiceStateRequest { Id = id }));

        /// <summary>
        /// Searches for invoice states based on the specified search text.
        /// </summary>
        /// <param name="searchText">The text to search for invoice states.</param>
        /// <param name="offset">The number of items to skip before starting to collect the result set.</param>
        /// <param name="limit">The number of items to return.</param>
        /// <param name="sortBy">The field by which to sort the results.</param>
        /// <param name="sortOrderAscending">Indicates whether the sorting should be in ascending order.</param>
        /// <returns>A response containing a list of invoice states that match the search text.</returns>
        [HttpGet("search")]
        [ProducesResponseType(
            typeof(Response<IEnumerable<InvoiceStateResponse>>),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchInvoiceStatesAsync(
            [FromQuery] string searchText,
            [FromQuery] int offset,
            [FromQuery] int limit,
            [FromQuery] SortBy sortBy,
            [FromQuery] bool sortOrderAscending
        ) =>
            BuildResponse(
                await Mediator.Send(
                    new GetBySearchInvoiceStateRequest
                    {
                        Query = new BaseQuerySearch
                        {
                            SearchText = searchText,
                            Limit = limit,
                            Offset = offset,
                            SortBy = sortBy,
                            SortOrderAscending = sortOrderAscending,
                        },
                    }
                )
            );

        /// <summary>
        /// Creates a new invoice state.
        /// </summary>
        /// <param name="request">The request containing the details of the invoice state to create.</param>
        /// <returns>A response containing the created invoice state details.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(InvoiceStateResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateInvoiceStateAsync(
            [FromBody] CreateInvoiceStateRequest request
        ) => BuildResponse(await Mediator.Send(request));

        /// <summary>
        /// Edits an existing invoice state by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the invoice state to be edited.</param>
        /// <param name="request">The request containing the invoice state details to be updated.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditInvoiceStateAsync(
            [FromRoute] int id,
            [FromBody] EditInvoiceStateRequest request
        )
        {
            request.RequestId = id;
            var response = await Mediator.Send(request);
            return BuildResponse(response);
        }

        /// <summary>
        /// Removes a invoice state by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the invoice state to be removed.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveInvoiceStateAsync([FromRoute] int id) =>
            BuildResponse(await Mediator.Send(new RemoveInvoiceStateRequest { Id = id }));
    }
}
