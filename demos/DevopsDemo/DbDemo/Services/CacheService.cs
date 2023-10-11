using Furion.DistributedIDGenerator;
using DbDemo.Models;
using SqlSugar;
using DbDemo.DbContext;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace DbDemo.Services
{
    [DynamicApiController]
    public class CacheService
    {
        private readonly IDistributedCache _cache;

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }
        /// <summary>
        /// 操作
        /// </summary>
        /// <returns></returns>
        public async Task<string> Operate()
        {
            var cacheKey = "cachedTimeUTC";
            var encodedCachedTimeUTC = await _cache.GetAsync(cacheKey);
            if (encodedCachedTimeUTC != null)
            {
                return Encoding.UTF8.GetString(encodedCachedTimeUTC);
            }
            var currentTimeUTC = DateTime.UtcNow.ToString();
            byte[] encodedCurrentTimeUTC = Encoding.UTF8.GetBytes(currentTimeUTC);

            // 设置分布式缓存
            var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(20));

            await _cache.SetAsync(cacheKey, encodedCurrentTimeUTC, options);
            return currentTimeUTC;
        }
    }
}
