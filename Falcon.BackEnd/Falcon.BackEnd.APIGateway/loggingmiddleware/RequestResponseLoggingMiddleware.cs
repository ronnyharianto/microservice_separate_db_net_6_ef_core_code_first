using System.Diagnostics;
using System.Text;

namespace Falcon.BackEnd.APIGateway.loggingmiddleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;   
        }

        private bool TargetRoute(PathString path)
        {
            // Add Route
            return path.StartsWithSegments("/product") ||
                   path.StartsWithSegments("/notification");
        }

        public async Task Invoke(HttpContext context)
        {
            if (TargetRoute(context.Request.Path))
            {
                var stopwatch = Stopwatch.StartNew();

                // Capture the original request body and response body
                var originalRequestBody = context.Request.Body;
                var originalResponseBody = context.Response.Body;

                try
                {
                    // Read the request body and log it
                    string requestBody = await ReadRequestBodyAsync(context.Request);
                    _logger.LogInformation("Request {RequestMethod} {RequestPath} {RequestBody}", context.Request.Method, context.Request.Path, requestBody);

                    using (var responseBody = new MemoryStream())
                    {
                        // Set the response body to the memory stream
                        context.Response.Body = responseBody;

                        // Call the next middleware in the pipeline
                        await _next(context);

                        // Read the response body and log it
                        string responseBodyContent = await ReadResponseBodyAsync(context.Response);
                        _logger.LogInformation("Response {StatusCode} {ResponseBody}", context.Response.StatusCode, responseBodyContent);

                        // Reset the position of the response memory stream to the beginning
                        responseBody.Seek(0, SeekOrigin.Begin);

                        // Copy the response memory stream to the original response body stream
                        await responseBody.CopyToAsync(originalResponseBody);
                    }
                }
                finally
                {
                    stopwatch.Stop();

                    // Log the elapsed time
                    _logger.LogInformation("Request completed in {Elapsed} milliseconds", stopwatch.Elapsed.TotalMilliseconds);

                    // Reset the request body and response body to their original state
                    context.Request.Body = originalRequestBody;
                    context.Response.Body = originalResponseBody;
                }

            }
            else
            {
                await _next(context);
            }

        }

        private async Task<string> ReadRequestBodyAsync(HttpRequest request)
        {
            request.EnableBuffering();

            using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true))
            {
                string requestBody = await reader.ReadToEndAsync();

                // Reset the position of the request body stream to the beginning
                request.Body.Seek(0, SeekOrigin.Begin);

                return requestBody;
            }
        }

        private async Task<string> ReadResponseBodyAsync(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);

            string responseBody = await new StreamReader(response.Body, Encoding.UTF8).ReadToEndAsync();

            // Reset the position of the response body stream to the beginning
            response.Body.Seek(0, SeekOrigin.Begin);

            return responseBody;
        }
    }
}
