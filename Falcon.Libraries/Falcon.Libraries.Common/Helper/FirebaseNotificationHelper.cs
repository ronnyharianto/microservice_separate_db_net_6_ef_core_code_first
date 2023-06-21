using Falcon.Libraries.Common.Constants;
using Falcon.Libraries.Common.Enums;
using Falcon.Libraries.Common.Object;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Falcon.Libraries.Common.Helper
{
    public class FirebaseNotificationHelper
    {
        private readonly JsonHelper _jsonHelper;
        public FirebaseNotificationHelper(JsonHelper jsonHelper)
        {
            _jsonHelper = jsonHelper;
        }

        public async Task<ServiceResult> SendNotif(string serverKey, string target, string contentBody, string contentTitle)
        {
            var retval = new ServiceResult(ServiceResultCode.BadRequest);

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"key={serverKey}");

            var data = new
            {
                to = target,
                notification = new
                {
                    body = contentBody,
                    title = contentTitle
                }
            };

            var json = _jsonHelper.SerializeObject(data);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var results = await client.PostAsync(ServiceConstants.FirebaseCloudMessagingService, httpContent);

            if (results.IsSuccessStatusCode)
            {
                retval.OK(null);
            }

            return retval;
        }

    }
}
