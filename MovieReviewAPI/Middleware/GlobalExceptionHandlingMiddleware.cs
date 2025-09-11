using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MovieReviewApi.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next; //next middleware in the pipeline
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL Server exception occurred");

                await HandleException(context, 500, "Database error occurred. Please try again later.",_logger);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Entity Framework update exception occurred");

                await HandleException(context, 500, "Database operation failed. Please try again later.", _logger);
            }
            catch (NullReferenceException nullEx)
            {
                _logger.LogError(nullEx, "Null reference exception occurred");

                await HandleException(context, 500, "An unexpected server error occurred.", _logger);
            }
            catch (ArgumentException argEx)
            {
                _logger.LogError(argEx, "Argument exception occurred");
                await HandleException(context, 500, "An invalid argument was provided.", _logger);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                await HandleException(context, 500, "An unexpected server error occurred.", _logger);
            }
        }

        private static async Task HandleException(HttpContext context, int statusCode, string message, ILogger logger)
        {
            var request = context.Request;
            var requestInfo = new
            {
                Method = request.Method,
                Path = request.Path,
            };

            logger.LogError("Exception occurred for request {@Request}: {Message}", requestInfo, message);

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = "Server Error",
                Detail = message,
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsJsonAsync(problemDetails);
        }

    }
}
