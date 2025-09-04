using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace MovieReviewApi.Api.Filters
{
    public class RequestResponseLoggingFilter:IAsyncActionFilter
    {
        private readonly ILogger<RequestResponseLoggingFilter> _logger;

        public RequestResponseLoggingFilter(ILogger<RequestResponseLoggingFilter> logger)
        {
            _logger = logger;
        }

        //we don't return Task here because compiler automatically wraps it in a Task when creating state maching
        //if the methods was not async we would have to return Task manually
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var stopWatch = Stopwatch.StartNew();
            var httpContext = context.HttpContext;
            var request = httpContext.Request;
                
            var controllerName = context.Controller.GetType().Name;
            var traceId = httpContext.TraceIdentifier;
            var actionName = context.ActionDescriptor.DisplayName;

            _logger.LogInformation(
              "REQUEST STARTED | Controller: {Controller} | Action: {Action} | Method: {HttpMethod} | Path: {Path} | TraceId: {TraceId}",
              controllerName, actionName, request.Method, request.Path, traceId);
            
            LogRouteParameters(context, traceId);

            LogActionParameters(context, traceId);

            // Log query parameters (like ?page=1&size=10)
            //LogQueryParameters(context, traceId);

            var executedContext = await next(); //actual controller action executes

            stopWatch.Stop();

            // Log the response details
            LogResponse(executedContext, controllerName, actionName ?? "Unknown Action", stopWatch.ElapsedMilliseconds, traceId);

        }

        private void LogRouteParameters(ActionExecutingContext context, string traceId)
        {
            var routeValues = context.RouteData.Values
             .Where(kvp => kvp.Key != "controller" && kvp.Key != "action")
             .ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.ToString() ?? "null");

            if (routeValues.Any())
            {
                _logger.LogInformation("Route Parameters: {@RouteParams} | TraceId: {TraceId}",
                    routeValues, traceId);
            }

        }

        private void LogActionParameters(ActionExecutingContext context, string traceId)
        {
            if (context.ActionArguments.Any())
            {
                var actionParams = new Dictionary<string, object>();

                foreach (var param in context.ActionArguments)
                {
                   
                        if (param.Value != null && IsDto(param.Value))
                        {
                            actionParams[param.Key] = param.Value; 
                        }
                        else
                        {
                            actionParams[param.Key] = param.Value?.ToString() ?? "null";
                        }
                }

            _logger.LogInformation("Action Parameters: {@ActionParams} | TraceId: {TraceId}", actionParams, traceId);
            }
        }

        private void LogQueryParameters(ActionExecutingContext context, string traceId)
        {
            var queryParams = context.HttpContext.Request.Query;
            if (queryParams.Any())
            {
               var queryParamsDict = queryParams.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());
                _logger.LogInformation("Query Parameters: {@QueryParams} | TraceId: {TraceId}", queryParamsDict, traceId);
            }
        }

        private void LogResponse(ActionExecutedContext context, string controllerName, string actionName,
         long durationMs, string traceId)
        {
            var statusCode = context.HttpContext.Response.StatusCode;

            if (context.Exception != null)
            {
                // Log exceptions with full details
                _logger.LogError(context.Exception,
                    "REQUEST FAILED | Controller: {Controller} | Action: {Action} | StatusCode: {StatusCode} | Duration: {Duration}ms | TraceId: {TraceId}",
                    controllerName, actionName, statusCode, durationMs, traceId);
            }
            else if (statusCode >= 400)
            {
                // Log client/server errors
                var logLevel = statusCode >= 500 ? LogLevel.Error : LogLevel.Warning;
                _logger.Log(logLevel,
                    "REQUEST COMPLETED WITH ERROR | Controller: {Controller} | Action: {Action} | StatusCode: {StatusCode} | Duration: {Duration}ms | TraceId: {TraceId}",
                    controllerName, actionName, statusCode, durationMs, traceId);
            }
            else
            {
                // Log successful requests, but warn if they're slow
                var logLevel = durationMs > 1000 ? LogLevel.Warning : LogLevel.Information;
                var message = durationMs > 1000
                    ? "REQUEST COMPLETED (SLOW)"
                    : "REQUEST COMPLETED";

                _logger.Log(logLevel,
                    "{Message} | Controller: {Controller} | Action: {Action} | StatusCode: {StatusCode} | Duration: {Duration}ms | TraceId: {TraceId}",
                    message, controllerName, actionName, statusCode, durationMs, traceId);
            }
        }

        private static bool IsDto(object value)
        {
            var typeName = value.GetType().Name;
            return typeName.EndsWith("Dto") || typeName.EndsWith("Request") || typeName.EndsWith("Command");
        }
    }
}

