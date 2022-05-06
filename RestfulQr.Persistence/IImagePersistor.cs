using RestfulQr.Domain;
using System.Threading.Tasks;

namespace RestfulQr.Persistence
{
    public interface IImagePersistor
    {
        Task<bool> DeleteAsync(ApiKey apiKey);
        Task<bool> DeleteAsync(ApiKey apiKey, string filename);
        Task<byte[]?> GetImageAsync(ApiKey apiKey, string filename);
        Task<byte[]?> GetImageAsync(long locationId, string filename) => GetImageAsync(new ApiKey { LocationId = locationId }, filename);
        Task<bool> UploadAsync(ApiKey apiKey, string filename, byte[] content);
    }
}
