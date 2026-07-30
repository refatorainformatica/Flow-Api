using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Abstractions.Primitives;

namespace Application.Controllers
{
    /// <summary>
    /// MetaController provides endpoints for retrieving metadata about the application.
    /// </summary>
    [ApiVersion("1.0")]
    [Tags("Meta Endpoints")]
    [ApiExplorerSettings(GroupName = "Meta")]
    [Route("api/v{version:apiVersion}/meta-infos")]
    public class MetaController : BaseApiController
    {
        /// <summary>
        /// Retrieves information about the application's version, last update time, and environment.
        /// </summary>
        /// <returns>A string containing the application's version, last update time, and environment.</returns>
        [HttpGet("info")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public IActionResult Info()
        {
            var assembly = typeof(Program).Assembly;
            var lastUpdate = System.IO.File.GetLastWriteTime(assembly.Location);
            var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

            return BuildResponse(
                Result<Response<dynamic>>.Success(
                    new Response<dynamic>(
                        new
                        {
                            Version = version,
                            LastUpdate = lastUpdate,
                            Environment = Environment.GetEnvironmentVariable(
                                "ASPNETCORE_ENVIRONMENT"
                            ),
                        }
                    )
                )
            );
        }
    }
}
