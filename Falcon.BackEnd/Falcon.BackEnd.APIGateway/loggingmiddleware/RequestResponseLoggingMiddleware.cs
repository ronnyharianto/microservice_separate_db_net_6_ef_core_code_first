using Serilog;

namespace Falcon.BackEnd.APIGateway.loggingmiddleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Log the request
            Log.Information("Request {RequestMethod} {RequestPath}", context.Request.Method, context.Request.Path);

            // Capture the original response stream
            var originalBodyStream = context.Response.Body;

            // Create a new memory stream to capture the response
            using (var responseBody = new MemoryStream())
            {
                // Set the response body to the new memory stream
                context.Response.Body = responseBody;

                await _next(context);

                // Log the response
                responseBody.Seek(0, SeekOrigin.Begin);
                var response = await new StreamReader(responseBody).ReadToEndAsync();
                Log.Information("Response {StatusCode} {ResponseBody}", context.Response.StatusCode, response);

                // Reset the response body to the original stream
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
    }
}
