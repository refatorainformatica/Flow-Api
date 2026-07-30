using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Features.Financials.RevenueTypes.Models;
using Services.Features.Financials.RevenueTypes.UseCases.Commands;
using Services.Features.Financials.RevenueTypes.UseCases.Queries;
using Shared.Domain.Abstractions.Enumerations;
using Shared.Domain.Abstractions.Primitives;

namespace Application.Controllers.v1.Financials
{
    /// <summary>
    /// The RevenueTypeController class handles the operations related to revenue type transactions.
    /// </summary>
    [Authorize]
    [ApiVersion("1.0")]
    [Tags("Financials - Revenues Endpoints")]
    [ApiExplorerSettings(GroupName = "Revenues")]
    [Route("api/v{version:apiVersion}/revenue-types")]
    public class RevenueTypeController : BaseApiController
    {
        /// <summary>
        /// Retrieves all revenue types based on the specified request.
        /// </summary>
        /// <param name="offset">The number of items to skip before starting to collect the result set.</param>
        /// <param name="limit">The number of items to return.</param>
        /// <param name="sortBy">The field by which to sort the results.</param>
        /// <param name="sortOrderAscending">Indicates whether the sorting should be in ascending order.</param>
        /// <returns>A response containing a list of revenue types.</returns>
        [HttpGet]
        [ProducesResponseType(
            typeof(Response<IEnumerable<RevenueTypeResponse>>),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllRevenueTypesAsync(
            [FromQuery] int offset,
            [FromQuery] int limit,
            [FromQuery] SortBy sortBy,
            [FromQuery] bool sortOrderAscending
        ) =>
            BuildResponse(
                await Mediator.Send(
                    new GetRevenueTypeRequest
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
        /// Gets a revenue type by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the revenue type.</param>
        /// <returns>An <see cref="IActionResult"/> containing the revenue type details.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RevenueTypeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRevenueTypeByIdAsync([FromRoute] int id) =>
            BuildResponse(await Mediator.Send(new GetByIdRevenueTypeRequest { Id = id }));

        /// <summary>
        /// Searches for revenue types based on the specified search text.
        /// </summary>
        /// <param name="searchText">The text to search for revenue types.</param>
        /// <param name="offset">The number of items to skip before starting to collect the result set.</param>
        /// <param name="limit">The number of items to return.</param>
        /// <param name="sortBy">The field by which to sort the results.</param>
        /// <param name="sortOrderAscending">Indicates whether the sorting should be in ascending order.</param>
        /// <returns>A response containing a list of revenue types that match the search text.</returns>
        [HttpGet("search")]
        [ProducesResponseType(
            typeof(Response<IEnumerable<RevenueTypeResponse>>),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchRevenueTypesAsync(
            [FromQuery] string searchText,
            [FromQuery] int offset,
            [FromQuery] int limit,
            [FromQuery] SortBy sortBy,
            [FromQuery] bool sortOrderAscending
        ) =>
            BuildResponse(
                await Mediator.Send(
                    new GetBySearchRevenueTypeRequest
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
        /// Creates a new revenue type.
        /// </summary>
        /// <param name="request">The request containing the details of the revenue type to create.</param>
        /// <returns>A response containing the created revenue type details.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(RevenueTypeResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateRevenueTypeAsync(
            [FromBody] CreateRevenueTypeRequest request
        ) => BuildResponse(await Mediator.Send(request));

        /// <summary>
        /// Edits an existing revenue type by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the revenue type to be edited.</param>
        /// <param name="request">The request containing the revenue type details to be updated.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditRevenueTypeAsync(
            [FromRoute] int id,
            [FromBody] EditRevenueTypeRequest request
        )
        {
            request.RequestId = id;
            var response = await Mediator.Send(request);
            return BuildResponse(response);
        }

        /// <summary>
        /// Removes a revenue type by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the revenue type to be removed.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveRevenueTypeAsync([FromRoute] int id) =>
            BuildResponse(await Mediator.Send(new RemoveRevenueTypeRequest { Id = id }));
    }
}
