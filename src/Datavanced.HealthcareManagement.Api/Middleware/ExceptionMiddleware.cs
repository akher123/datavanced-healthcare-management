using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace Datavanced.HealthcareManagement.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred at {Time}", DateTime.UtcNow);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            int statusCode = (int)HttpStatusCode.InternalServerError;
            string message = "Internal server error. Please contact the administrator.";

            // Map known exception types
            switch (exception)
            {
                case DatavancedException dsiEx:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = dsiEx.Message;
                    break;

                case SqlException sqlEx when sqlEx.Number == 547:
                    statusCode = StatusCodes.Status409Conflict;
                    message = "The record cannot be deleted because it is associated with another record.";
                    break;

                case AggregateException aggEx:
                    var sqlInner = aggEx.InnerExceptions.OfType<SqlException>()
                        .FirstOrDefault(e => e.Number == 547);
                    if (sqlInner != null)
                    {
                        statusCode = StatusCodes.Status409Conflict;
                        message = "The record cannot be deleted because it is associated with another record.";
                    }
                    break;
                default:
                    break;
            }

            context.Response.StatusCode = statusCode;

            var response = new ResponseMessage<string>
            {
                StatusCode = statusCode,
                Message = message
            };

            var json = JsonConvert.SerializeObject(response, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            await context.Response.WriteAsync(json);
        }
    }
}
