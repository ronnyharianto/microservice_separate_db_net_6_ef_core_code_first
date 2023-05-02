using Falcon.Libraries.Common.Object;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Falcon.Libraries.Microservice.HttpClients
{
    public class CustomHttpClient
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomHttpClient(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            _client = client;
            _httpContextAccessor = httpContextAccessor;
        }

        public T? Get<T>(string url)
        {
            var responseText = GetAsync(url).Result;

            var responseData = JsonConvert.DeserializeObject<T>(responseText);

            return responseData;
        }

        public ObjectResult<T>? GetObjectResult<T>(string url)
            where T : class
        {
            var responseText = GetAsync(url).Result;

            var responseData = JsonConvert.DeserializeObject<ObjectResult<T>>(responseText);

            return responseData;
        }

        private async Task<string> GetAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await _client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
