using Microsoft.Extensions.Caching.Distributed;

namespace Falcon.Libraries.Common.Helper
{
    public class CacheHelper
    {
        private readonly IDistributedCache _distributedCache;
        private readonly JsonHelper _jsonHelper;

        public CacheHelper(IDistributedCache distributedCache, JsonHelper jsonHelper)
        {
            _distributedCache = distributedCache;
            _jsonHelper = jsonHelper;
        }

        public void SetCacheData<T>(string cacheKey, T data, TimeSpan? absoluteExpirationRelativeToNow)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow ?? TimeSpan.FromDays(1),
            };

            var json = _jsonHelper.SerializeObject(data);
            _distributedCache.SetString(cacheKey, json, options);
        }

        public T? GetCacheData<T>(string cacheKey)
        {
            var jsonData = _distributedCache.GetString(cacheKey);

            if (jsonData != null)
            {
                return _jsonHelper.DeserializeObject<T>(jsonData);
            }

            return default;
        }
    }
}
