using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Features.Peoples.Skills.Models;
using Services.Features.Peoples.Skills.UseCases.Commands;
using Services.Features.Peoples.Skills.UseCases.Queries;
using Shared.Domain.Abstractions.Enumerations;
using Shared.Domain.Abstractions.Primitives;

namespace Application.Controllers.v1.Peoples
{
    /// <summary>
    /// The SkillController class handles the operations related to skill transactions.
    /// </summary>
    [Authorize]
    [ApiVersion("1.0")]
    [Tags("Peoples - SKills Endpoints")]
    [ApiExplorerSettings(GroupName = "Skills")]
    [Route("api/v{version:apiVersion}/skills")]
    public class SkillController : BaseApiController
    {
        /// <summary>
        /// Retrieves all skills based on the specified request.
        /// </summary>
        /// <param name="offset">The number of items to skip before starting to collect the result set.</param>
        /// <param name="limit">The number of items to return.</param>
        /// <param name="sortBy">The field by which to sort the results.</param>
        /// <param name="sortOrderAscending">Indicates whether the sorting should be in ascending order.</param>
        /// <returns>A response containing a list of skills.</returns>
        [HttpGet]
        [ProducesResponseType(
            typeof(Response<IEnumerable<SkillResponse>>),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllSkillsAsync(
            [FromQuery] int offset,
            [FromQuery] int limit,
            [FromQuery] SortBy sortBy,
            [FromQuery] bool sortOrderAscending
        ) =>
            BuildResponse(
                await Mediator.Send(
                    new GetSkillRequest
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
        /// Gets a skill by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the skill.</param>
        /// <returns>An <see cref="IActionResult"/> containing the skill details.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SkillResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSkillByIdAsync([FromRoute] int id) =>
            BuildResponse(await Mediator.Send(new GetByIdSkillRequest { Id = id }));

        /// <summary>
        /// Searches for skills based on the specified search text.
        /// </summary>
        /// <param name="searchText">The text to search for skills.</param>
        /// <param name="offset">The number of items to skip before starting to collect the result set.</param>
        /// <param name="limit">The number of items to return.</param>
        /// <param name="sortBy">The field by which to sort the results.</param>
        /// <param name="sortOrderAscending">Indicates whether the sorting should be in ascending order.</param>
        /// <returns>A response containing a list of skills that match the search text.</returns>
        [HttpGet("search")]
        [ProducesResponseType(
            typeof(Response<IEnumerable<SkillResponse>>),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchSkillsAsync(
            [FromQuery] string searchText,
            [FromQuery] int offset,
            [FromQuery] int limit,
            [FromQuery] SortBy sortBy,
            [FromQuery] bool sortOrderAscending
        ) =>
            BuildResponse(
                await Mediator.Send(
                    new GetBySearchSkillRequest
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
        /// Creates a new skill.
        /// </summary>
        /// <param name="request">The request containing the details of the skill to create.</param>
        /// <returns>A response containing the created skill details.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(SkillResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateSkillAsync([FromBody] CreateSkillRequest request) =>
            BuildResponse(await Mediator.Send(request));

        /// <summary>
        /// Edits an existing skill by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the skill to be edited.</param>
        /// <param name="request">The request containing the skill details to be updated.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditSkillAsync(
            [FromRoute] int id,
            [FromBody] EditSkillRequest request
        )
        {
            request.RequestId = id;
            var response = await Mediator.Send(request);
            return BuildResponse(response);
        }

        /// <summary>
        /// Removes a skill by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the skill to be removed.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveSkillAsync([FromRoute] int id) =>
            BuildResponse(await Mediator.Send(new RemoveSkillRequest { Id = id }));
    }
}
