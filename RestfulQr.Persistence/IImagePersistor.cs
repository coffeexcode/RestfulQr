using RestfulQr.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulQr.Persistence
{
    public interface IImagePersistor
    {
        Task<bool> DeleteAsync(ApiKey key);
        Task<bool> DeleteAsync(ApiKey key, string filename);
        Task<byte[]?> GetImageAsync(long locationId, string filename);
        Task<bool> UploadAsync(ApiKey apiKey, string filename, byte[] content);
    }
}
