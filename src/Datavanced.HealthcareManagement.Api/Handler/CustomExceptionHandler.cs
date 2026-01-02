using Datavanced.HealthcareManagement.Shared.ExceptionHelper;
using Microsoft.AspNetCore.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace Datavanced.HealthcareManagement.Api.Handler
{
    public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError("Error Messge:{exceptionMessage}, Time of occurrence {time}", exception.Message, DateTime.UtcNow);

            (string Detail, string Title, int StatusCode) details = exception switch
            {
                InternalServerException => (
                    exception.Message,
                    exception.GetType().Name,
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError
                ),
                ValidationException => (
                    exception.Message,
                    exception.GetType().Name,
                    context.Response.StatusCode = StatusCodes.Status400BadRequest
                )
           ,
                BadRequestException => (
                    exception.Message,
                    exception.GetType().Name,
                    context.Response.StatusCode = StatusCodes.Status400BadRequest
                )
            ,
                NotFoundException => (
                    exception.Message,
                    exception.GetType().Name,
                    context.Response.StatusCode = StatusCodes.Status404NotFound
                ),
                DuplicateRequestException => (
                  exception.Message,
                  exception.GetType().Name,
                  context.Response.StatusCode = StatusCodes.Status409Conflict
              ),

                _ => (
                   exception.Message,
                    exception.GetType().Name,
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError
                )
            };
            var problemDetails = new ProblemDetails
            {
                Title = details.Title,
                Detail = details.Detail,
                Status = details.StatusCode,
                Instance = context.Request.Path
            };
            ErrorMessage errorMessage = new ErrorMessage { Message = problemDetails.Detail, StatusCode= problemDetails.Status };

            problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

            await context.Response.WriteAsJsonAsync(errorMessage, cancellationToken);
            return true;
        }
    }
}
