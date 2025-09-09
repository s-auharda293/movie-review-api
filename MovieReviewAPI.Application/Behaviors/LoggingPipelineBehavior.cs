using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;

namespace MovieReviewApi.Application.Behaviors

{
    //generic arguments specify our request and response as TRequest and TResponse
    public class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest:notnull
    {
        private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;
        private readonly IHttpContextAccessor? _httpContextAccessor;

        public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var stopWatch = Stopwatch.StartNew();
            var context = _httpContextAccessor?.HttpContext;
            var traceId = context?.TraceIdentifier ?? "NoHttpContext";
            var routeData = context?.Request.RouteValues.ToDictionary(r => r.Key, r => r.Value?.ToString());
            var queryParams = context?.Request.Query.ToDictionary(q => q.Key, q => q.Value.ToString());
            var method = context?.Request.Method;
            var path = context?.Request.Path;


            var requestBodyJson = JsonSerializer.Serialize(request);

            _logger.LogInformation(
               "REQUEST STARTED | Request: {@RequestName}, TraceId: {@TraceId}, Method: {@Method}, Path: {@Path}, RouteData: {@RouteData}, QueryParams: {@QueryParams}, RequestBody: {@RequestBody}, Request Time: {@DateTimeNow}",
               typeof(TRequest).Name,
               traceId,
               method,
               path,
               routeData,
               queryParams,
               requestBodyJson,
               DateTime.Now
             );

            var result = await next();

            stopWatch.Stop();
            _logger.LogInformation("REQUEST COMPLETED | Request: {@RequestName} | TraceId:{@TraceId} | Request Time: {@DateTimeNow} | Time Elapsed: {@TimePassed} | Response: {@Response}", typeof(TRequest).Name,traceId, DateTime.Now, stopWatch.ElapsedMilliseconds, JsonSerializer.Serialize(result));
            return result;
        }
    }
}
