using System.Collections;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;
using Shared.Domain.Abstractions.Enumerations;
using Shared.Domain.Abstractions.Primitives;
using Shared.Infrastructure.Resources;

namespace Application.Controllers
{
    /// <summary>
    /// Base API controller providing common functionality for all API controllers.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        private IMediator _mediator;

        /// <summary>
        /// Gets the mediator instance.
        /// </summary>
        protected IMediator Mediator =>
            _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        /// <summary>
        /// Builds an IActionResult based on the provided Result object.
        /// </summary>
        /// <typeparam name="T">The type of the data in the response.</typeparam>
        /// <param name="result">The result object containing the response data and errors.</param>
        /// <returns>An IActionResult representing the HTTP response.</returns>
        protected IActionResult BuildResponse<T>(Result<Response<T>> result)
        {
            if (!result.IsSuccess)
            {
                var errors = result.Errors;
                var errorMappings = new Dictionary<ErrorType, Func<IActionResult>>
                {
                    { ErrorType.Validation, () => HandleValidationFailure(errors) },
                    { ErrorType.PreConditionFailed, () => HandleValidationFailure(errors) },
                    {
                        ErrorType.Unauthorized,
                        () =>
                            HandleFailure(
                                StatusCodes.Status401Unauthorized,
                                errors.FirstOrDefault().Code,
                                errors.FirstOrDefault().Description,
                                Config.HttpResponseErrorTypeStatus401Unauthorized
                            )
                    },
                    {
                        ErrorType.NotFound,
                        () =>
                            HandleFailure(
                                StatusCodes.Status404NotFound,
                                errors.FirstOrDefault().Code,
                                errors.FirstOrDefault().Description,
                                Config.HttpResponseErrorTypeStatus404NotFound
                            )
                    },
                    {
                        ErrorType.NoContent,
                        () =>
                            HandleFailure(
                                StatusCodes.Status404NotFound,
                                errors.FirstOrDefault().Code,
                                errors.FirstOrDefault().Description,
                                Config.HttpResponseErrorTypeStatus404NotFound
                            )
                    },
                };

                var matchingError = errors
                    .Select(e => e.Type)
                    .FirstOrDefault(errorMappings.ContainsKey);
                return matchingError != default
                    ? errorMappings[matchingError]()
                    : StatusCode(500, errors);
            }

            return BuildResponse(result.Value);
        }

        /// <summary>
        /// Handles failure responses.
        /// </summary>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="title">The title of the error.</param>
        /// <param name="detail">The detail of the error.</param>
        /// <param name="instance">The instance of the error.</param>
        /// <param name="type">The type of the error.</param>
        /// <returns>An IActionResult representing the failure response.</returns>
        protected IActionResult HandleFailure(
            int statusCode,
            string title,
            string detail = "",
            string instance = "",
            string type = ""
        )
        {
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = instance ?? HttpContext.Request.Path,
                Type = type,
            };

            return StatusCode(statusCode, problemDetails);
        }

        /// <summary>
        /// Handles validation failure responses.
        /// </summary>
        /// <param name="errors">The validation errors.</param>
        /// <returns>An IActionResult representing the validation failure response.</returns>
        protected IActionResult HandleValidationFailure(IEnumerable<Error> errors)
        {
            var problemDetails = new ValidationProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation failed",
                Detail = "One or more validation errors occurred.",
                Instance = HttpContext.Request.Path,
                Type = "https://developer.mozilla.org/en-US/docs/Web/HTTP/Reference/Status/400",
            };

            var errorsGrouped = errors
                .GroupBy(f => f.Code)
                .ToDictionary(g => g.Key, g => g.Select(f => f.Description).ToArray());

            problemDetails.Errors.AddRange(errorsGrouped);

            return BadRequest(problemDetails);
        }

        /// <summary>
        /// Builds an appropriate <see cref="IActionResult"/> based on the provided <see cref="Response{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the data in the response.</typeparam>
        /// <param name="response">The response object containing the response data.</param>
        /// <returns>An IActionResult representing the result response.</returns>
        private IActionResult BuildResponse<T>(Response<T> response)
        {
            if (
                HttpContext.Request.Method == HttpMethods.Delete
                || HttpContext.Request.Method == HttpMethods.Put
            )
            {
                return NoContent();
            }

            if (HttpContext.Request.Method == HttpMethods.Get)
            {
                return BuildGetResponse(response);
            }

            if (HttpContext.Request.Method == HttpMethods.Post)
            {
                return BuildPostResponse(response);
            }

            throw new InvalidOperationException(HttpContext.Request.Method);
        }

        /// <summary>
        /// Builds an appropriate <see cref="IActionResult"/> based on the provided <see cref="Response{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the data in the response.</typeparam>
        /// <param name="response">The response object containing the response data.</param>
        /// <returns>An IActionResult representing the result response.</returns>
        private IActionResult BuildGetResponse<T>(Response<T> response)
        {
            if (response.Data is IEnumerable<T> genericEnumerable)
            {
                if (!genericEnumerable.Any())
                {
                    return NoContent();
                }
                return Ok(response);
            }

            if (response.Data is IEnumerable enumerable)
            {
                if (!enumerable.Cast<object>().Any())
                {
                    return NoContent();
                }
                return Ok(response);
            }

            if (response.Data == null)
            {
                return NoContent();
            }

            return Ok(response.Data);
        }

        /// <summary>
        /// Builds an appropriate <see cref="IActionResult"/> based on the provided <see cref="Response{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the data in the response.</typeparam>
        /// <param name="response">The response object containing the response data.</param>
        /// <returns>An IActionResult representing the result response.</returns>
        private IActionResult BuildPostResponse<T>(Response<T> response)
        {
            var id = response.Data.GetType().GetProperty("Id")?.GetValue(response.Data);
            return Created($"{HttpContext.Request.Path}/{id ?? ""}", response.Data);
        }
    }
}
