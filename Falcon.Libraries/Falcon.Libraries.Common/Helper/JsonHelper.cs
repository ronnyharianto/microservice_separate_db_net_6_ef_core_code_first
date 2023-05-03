using Newtonsoft.Json;

namespace Falcon.Libraries.Common.Helper
{
    public class JsonHelper
    {
        private readonly JsonSerializerSettings settings;

        public JsonHelper()
        {
            settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        public string SerializeObject(object? data) => JsonConvert.SerializeObject(data, settings);

        public T? DeserializeObject<T>(string jsonString) => JsonConvert.DeserializeObject<T>(jsonString, settings);
    }
}
