using System.Collections;
using System.Collections.Generic;

namespace Easynvest.Infohub.Parse.Domain.Interfaces
{
    public interface ICache
    {
        void Set<T>(string key,  T obj);

        T Get<T>(string key);

        void DeleteByKey<T>(string key);

        IList<T> GetAll<T>();
    }
}
