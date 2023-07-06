using Falcon.Libraries.Common.Constants;
using Falcon.Libraries.Common.Enums;
using Falcon.Libraries.Common.Object;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Dynamic;
using System.Net.Http;
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

        public async Task<ServiceResult> SendNotif(string serverKey, List<string> target, string contentBody, string contentTitle, bool IsSentToAll)
        {
            var retval = new ServiceResult(ServiceResultCode.BadRequest);

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"key={serverKey}");

            IList<string> strings = target;
            string joined = string.Join(",", strings);

            dynamic data = new ExpandoObject();

            if (!IsSentToAll)
            {
                data = new
                {
                    registration_ids = target,
                    notification = new
                    {
                        body = contentBody,
                        title = contentTitle
                    }
                };
            }
            else
            {
                data = new
                {
                    to = joined,
                    notification = new
                    {
                        body = contentBody,
                        title = contentTitle
                    }
                };
            }

            var json = _jsonHelper.SerializeObject(data);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(ServiceConstants.FirebaseCloudMessagingService, httpContent);
            if (response.IsSuccessStatusCode)
            {
                retval.OK(null);
            }

            return retval;
        }

    }
}
