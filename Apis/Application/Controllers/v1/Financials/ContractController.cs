using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Features.Financials.Contracts.Models;
using Services.Features.Financials.Contracts.UseCases.Commands;
using Services.Features.Financials.Contracts.UseCases.Queries;
using Shared.Domain.Abstractions.Enumerations;
using Shared.Domain.Abstractions.Primitives;

namespace Application.Controllers.v1.Financials
{
    /// <summary>
    /// The ContractController class handles the operations related to contract transactions.
    /// </summary>
    [Authorize]
    [ApiVersion("1.0")]
    [Tags("Financials - Contracts Endpoints")]
    [ApiExplorerSettings(GroupName = "Contracts")]
    [Route("api/v{version:apiVersion}/contracts")]
    public class ContractController : BaseApiController
    {
        /// <summary>
        /// Retrieves all contracts based on the specified request.
        /// </summary>
        /// <param name="offset">The number of items to skip before starting to collect the result set.</param>
        /// <param name="limit">The number of items to return.</param>
        /// <param name="sortBy">The field by which to sort the results.</param>
        /// <param name="sortOrderAscending">Indicates whether the sorting should be in ascending order.</param>
        /// <returns>A response containing a list of contracts.</returns>
        [HttpGet]
        [ProducesResponseType(
            typeof(Response<IEnumerable<ContractResponse>>),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllContractsAsync(
            [FromQuery] int offset,
            [FromQuery] int limit,
            [FromQuery] SortBy sortBy,
            [FromQuery] bool sortOrderAscending
        ) =>
            BuildResponse(
                await Mediator.Send(
                    new GetContractRequest
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
        /// Gets a contract by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the contract.</param>
        /// <returns>An <see cref="IActionResult"/> containing the contract details.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ContractResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetContractByIdAsync([FromRoute] int id) =>
            BuildResponse(await Mediator.Send(new GetByIdContractRequest { Id = id }));

        /// <summary>
        /// Searches for contracts based on the specified search text.
        /// </summary>
        /// <param name="searchText">The text to search for contracts.</param>
        /// <param name="offset">The number of items to skip before starting to collect the result set.</param>
        /// <param name="limit">The number of items to return.</param>
        /// <param name="sortBy">The field by which to sort the results.</param>
        /// <param name="sortOrderAscending">Indicates whether the sorting should be in ascending order.</param>
        /// <returns>A response containing a list of contracts that match the search text.</returns>
        [HttpGet("search")]
        [ProducesResponseType(
            typeof(Response<IEnumerable<ContractResponse>>),
            StatusCodes.Status200OK
        )]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchContractsAsync(
            [FromQuery] string searchText,
            [FromQuery] int offset,
            [FromQuery] int limit,
            [FromQuery] SortBy sortBy,
            [FromQuery] bool sortOrderAscending
        ) =>
            BuildResponse(
                await Mediator.Send(
                    new GetBySearchContractRequest
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
        /// Creates a new contract.
        /// </summary>
        /// <param name="request">The request containing the details of the contract to create.</param>
        /// <returns>A response containing the created contract details.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ContractResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateContractAsync(
            [FromBody] CreateContractRequest request
        ) => BuildResponse(await Mediator.Send(request));

        /// <summary>
        /// Edits an existing contract by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the contract to be edited.</param>
        /// <param name="request">The request containing the contract details to be updated.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditContractAsync(
            [FromRoute] int id,
            [FromBody] EditContractRequest request
        )
        {
            request.RequestId = id;
            var response = await Mediator.Send(request);
            return BuildResponse(response);
        }

        /// <summary>
        /// Removes a contract by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the contract to be removed.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveContractAsync([FromRoute] int id) =>
            BuildResponse(await Mediator.Send(new RemoveContractRequest { Id = id }));
    }
}
