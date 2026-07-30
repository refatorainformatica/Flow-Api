using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Features.Peoples.ActivityBranchs.Models;
using Services.Features.Peoples.ActivityBranchs.UseCases.Commands;
using Services.Features.Peoples.ActivityBranchs.UseCases.Queries;
using Shared.Domain.Abstractions.Enumerations;
using Shared.Domain.Abstractions.Primitives;

namespace Application.Controllers.v1.Peoples
{
    /// <summary>
    /// The ActivityBranchController class handles the operations related to activity branch transactions.
    /// </summary>
    [Authorize]
    [ApiVersion("1.0")]
    [Tags("Peoples - Suppliers Endpoints")]
    [ApiExplorerSettings(GroupName = "Suppliers")]
    [Route("api/v{version:apiVersion}/activity-branchs")]
    public class ActivityBranchController : BaseApiController
    {
        /// <summary>
        /// Retrieves all activity branchs based on the specified request.
        /// </summary>
        /// <param name="offset">The number of items to skip before starting to collect the result set.</param>
        /// <param name="limit">The number of items to return.</param>
        /// <param name="sortBy">The field by which to sort the results.</param>
        /// <param name="sortOrderAscending">Indicates whether the sorting should be in ascending order.</param>
        /// <returns>A response containing a list of activity branchs.</returns>
        [HttpGet]
        [ProducesResponseType(
            typeof(Response<IEnumerable<ActivityBranchResponse>>),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllActivityBranchsAsync(
            [FromQuery] int offset,
            [FromQuery] int limit,
            [FromQuery] SortBy sortBy,
            [FromQuery] bool sortOrderAscending
        ) =>
            BuildResponse(
                await Mediator.Send(
                    new GetActivityBranchRequest
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
        /// Gets a activity branch by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the activity branch.</param>
        /// <returns>An <see cref="IActionResult"/> containing the activity branch details.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ActivityBranchResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetActivityBranchByIdAsync([FromRoute] int id) =>
            BuildResponse(await Mediator.Send(new GetByIdActivityBranchRequest { Id = id }));

        /// <summary>
        /// Searches for activity branchs based on the specified search text.
        /// </summary>
        /// <param name="searchText">The text to search for activity branchs.</param>
        /// <param name="offset">The number of items to skip before starting to collect the result set.</param>
        /// <param name="limit">The number of items to return.</param>
        /// <param name="sortBy">The field by which to sort the results.</param>
        /// <param name="sortOrderAscending">Indicates whether the sorting should be in ascending order.</param>
        /// <returns>A response containing a list of activity branchs that match the search text.</returns>
        [HttpGet("search")]
        [ProducesResponseType(
            typeof(Response<IEnumerable<ActivityBranchResponse>>),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchActivityBranchsAsync(
            [FromQuery] string searchText,
            [FromQuery] int offset,
            [FromQuery] int limit,
            [FromQuery] SortBy sortBy,
            [FromQuery] bool sortOrderAscending
        ) =>
            BuildResponse(
                await Mediator.Send(
                    new GetBySearchActivityBranchRequest
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
        /// Creates a new activity branch.
        /// </summary>
        /// <param name="request">The request containing the details of the activity branch to create.</param>
        /// <returns>A response containing the created activity branch details.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ActivityBranchResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateActivityBranchAsync(
            [FromBody] CreateActivityBranchRequest request
        ) => BuildResponse(await Mediator.Send(request));

        /// <summary>
        /// Edits an existing activity branch by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the activity branch to be edited.</param>
        /// <param name="request">The request containing the activity branch details to be updated.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditActivityBranchAsync(
            [FromRoute] int id,
            [FromBody] EditActivityBranchRequest request
        )
        {
            request.RequestId = id;
            var response = await Mediator.Send(request);
            return BuildResponse(response);
        }

        /// <summary>
        /// Removes a activity branch by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the activity branch to be removed.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveActivityBranchAsync([FromRoute] int id) =>
            BuildResponse(await Mediator.Send(new RemoveActivityBranchRequest { Id = id }));
    }
}
