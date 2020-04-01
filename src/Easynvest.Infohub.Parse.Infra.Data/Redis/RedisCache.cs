using Easynvest.Infohub.Parse.Domain.Interfaces;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Sections;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceStack.Redis;
using System.Collections.Generic;

namespace Easynvest.Infohub.Parse.Infra.Data.Redis
{
    public class RedisCache : ICache
    {
        private readonly RedisClient redisClient;
        private readonly ILogger<RedisCache> _logger;

        public RedisCache(IOptions<RedisSection> options, ILogger<RedisCache> logger)
        {
            var section = options.Value;
            redisClient = new RedisClient(section.Connection);
            _logger = logger;
        }

        public void DeleteByKey<T>(string key)
        {
            if (this.IsUnavailable()) return;
            redisClient.Del(key);
        }

        public virtual T Get<T>(string key)
        {
            if (this.IsUnavailable())
                return default;

            return redisClient.Get<T>(key);
        }

        public IList<T> GetAll<T>()
        {
            if (this.IsUnavailable()) return new List<T>();

            var keys = redisClient.GetAllKeys();

            IList<T> values = new List<T>(keys.Count);

            foreach (var key in keys) values.Add(redisClient.Get<T>(key));

            return values;
        }

        public void Set<T>(string key, T obj)
        {
            if (this.IsUnavailable()) return;

            redisClient.Set(key, obj);
        }

        private bool IsUnavailable()
        {
            try
            {
                return !this.redisClient.Ping();
            }
            catch (System.Exception ex)
            {
                _logger.LogError("Falha ao conectar com o Redis" + ex);
                return true;
            }
        }
    }
}
