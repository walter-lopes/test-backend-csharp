using Easynvest.Infohub.Parse.Domain.Interfaces;
using Easynvest.Infohub.Parse.Infra.CrossCutting.Sections;
using Microsoft.Extensions.Options;
using ServiceStack.Redis;
using System.Collections.Generic;

namespace Easynvest.Infohub.Parse.Infra.Data.Redis
{
    public class RedisCache : ICache
    {
        private readonly RedisClient redisClient;

        public RedisCache(IOptions<RedisSection> options)
        {
            var section = options.Value;
            redisClient = new RedisClient(section.Connection);
        }

        public void DeleteByKey<T>(string key)
        {
            using (redisClient)
            {
                redisClient.Del(key);
            }

        }

        public T Get<T>(string key)
        {
            using (redisClient)
            {
                return redisClient.Get<T>(key);
            }
        }

        public IList<T> GetAll<T>()
        {
            using (redisClient)
            {
                var keys = redisClient.GetAllKeys();

                IList<T> values = new List<T>(keys.Count);

                foreach (var key in keys)
                {
                    values.Add(redisClient.Get<T>(key));
                }

                return values;
            }
        }

        public void Set<T>(string key, T obj)
        {
            using (redisClient)
            {
                redisClient.Set(key, obj);
            }
        }
    }
}
