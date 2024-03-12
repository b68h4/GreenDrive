using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenDrive.Models;
using Microsoft.Extensions.Caching.Memory;

namespace GreenDrive.Components
{
    public class DownloadStorage
    {
        private readonly IMemoryCache _store;
        public DownloadStorage(IMemoryCache store)
        {

            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        public string CreateToken(string FileId)
        {
            var token = Guid.NewGuid().ToString("n");

            _store.Set<DownloadModel>(token, new DownloadModel
            {
                FileId = FileId,
                Id = token
            }, DateTime.Now.AddHours(2));

            return token;
        }

        public DownloadModel GetData(string Id)
        {
            var data = _store.Get<DownloadModel>(Id);

            if (data == null)
            {
                return null;
            }

            return data;
        }
    }
}
