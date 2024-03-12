using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenDrive.Models;
using Microsoft.Extensions.Caching.Memory;

namespace GreenDrive.Components
{
    public class Cache
    {
        private readonly IMemoryCache store;

        public Cache(IMemoryCache _store)
        {
            store = _store ?? throw new ArgumentNullException(nameof(_store));

        }

        public List<DriveResult> CreateCache(string id, List<DriveResult> data)
        {
            return store.Set(id, data, DateTime.Now.AddHours(6));
        }

        public List<DriveResult> GetCache(string id)
        {
            try
            {
                var data = store.Get<List<DriveResult>>(id);
                if (data == null)
                {
                    return null;
                }

                return data;
            }
            catch (Exception e)
            {
                return null;
            }

        }
    }
}
