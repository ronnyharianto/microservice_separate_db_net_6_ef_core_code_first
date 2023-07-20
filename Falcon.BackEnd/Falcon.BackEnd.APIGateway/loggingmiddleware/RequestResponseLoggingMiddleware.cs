using Falcon.Libraries.Common.Helper;
using System.Diagnostics;
using System.Text;
using Falcon.BackEnd.APIGateway.Service.APIGateway;
using Falcon.Libraries.Common.Enums;
using Falcon.Libraries.Common.Object;
using Newtonsoft.Json;
using Falcon.BackEnd.APIGateway.Controllers.APIGateway.CustomModels;
using System.IO;

namespace Falcon.BackEnd.APIGateway.loggingmiddleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
        //private readonly JsonHelper _jsonHelper;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger/*, JsonHelper jsonHelper*/)
        {
            _next = next;
            _logger = logger;
            //_jsonHelper = jsonHelper;
        }

        private bool TargetRoute(PathString path)
        {
            // Add Route
            return path.StartsWithSegments("/notification");
        }

        public async Task Invoke(HttpContext context)
        {
            var accessToken = context.Request.Headers["Authorization"];

            if(context.Request.Path.StartsWithSegments("/apigateway") || TargetRoute(context.Request.Path))
            {
                var ResultValidateToken = await ValidateToken(accessToken);

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
                        if (!(ResultValidateToken.Succeeded))
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            await context.Response.WriteAsync($"Token Expired : {accessToken}");
                            //return;
                            // Read the response body and log it
                            string responseBodyContentValidateToken = await ReadResponseBodyAsync(context.Response);
                            _logger.LogInformation("Response {StatusCode} {ResponseBody}", context.Response.StatusCode, responseBodyContentValidateToken);
                        }
                        else
                        {
                            // Set the response body to the memory stream
                            context.Response.Body = responseBody;

                            // Call the next middleware in the pipeline
                            await _next(context);

                            // Read the response body and log it
                            string responseBodyContent = await ReadResponseBodyAsync(context.Response);
                            _logger.LogInformation("Response {StatusCode} {ResponseBody}", context.Response.StatusCode, responseBodyContent);
                            
                        }
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

        private async Task<ObjectResult<object>> ValidateToken(string? input)
        {
            var retVal = new ObjectResult<object>(ServiceResultCode.BadRequest);

            HttpClient client = new HttpClient();

            var data = new
            {
                objRequestData = input
            };

            var json = JsonConvert.SerializeObject(data);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://apigw.kalbenutritionals.com/t/kalbenutritionals.com/wso/v1/WsoAPI/ValidateToken", httpContent);

            // Read the responses
            var responseContent = await response.Content.ReadAsStringAsync();

            GetResponValidateTokenDto? responseObject = JsonConvert.DeserializeObject<GetResponValidateTokenDto>(responseContent);

            if (responseObject != null && responseObject.objData != null)
            {
                if (responseObject.objData.active == true)
                {
                    retVal.OK("Active");
                }
            }
            return retVal;
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
