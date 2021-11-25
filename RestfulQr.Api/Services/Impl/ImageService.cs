using RestfulQr.Api.Core.Caching;
using RestfulQr.Domain;
using RestfulQr.Persistence;
using RestfulQr.Persistence.Local;
using RestfulQr.Persistence.S3;

namespace RestfulQr.Api.Services.Impl
{
    public interface IImageService : IImagePersistor, IRestfulQrCodeRepository
    {
        public bool IsValid(long locationId, string filename);
    }

    public class ImageService : IImageService
    {
        private readonly ICacheProvider<byte[]> imageCache;
        private readonly IImagePersistor imagePersistor;
        private readonly IRestfulQrCodeRepository repository;

        public ImageService(ICacheProvider<byte[]> imageCache, IImagePersistor imagePersistor, IRestfulQrCodeRepository repository)
        {
            this.imageCache = imageCache;
            this.imagePersistor = imagePersistor;
            this.repository = repository;
        }

        public async Task<IList<RestfulQrCode>> GetAllAsync(ApiKey key)
        {
            return await repository.GetAllAsync(key);
        }

        public async Task<byte[]?> GetImageAsync(long locationId, string filename)
        {
            if (!IsValid(locationId, filename))
            {
                return null;
            }

            var imagePath = BuildImagePath(locationId, filename);

            // Check cache
            var cacheBytes = await imageCache.GetAsync(imagePath);

            if (cacheBytes != null)
            {
                if (cacheBytes.Length > 1)
                {
                    return cacheBytes;
                }
                // If the cache has a value that is length 1,
                // S3 doesn't have the file and we cached nothing (for a reason)
                else if (cacheBytes.Length == 1)
                {
                    return null;
                }
            }

            // Fetch image from AWS
            var imageBytes = await imagePersistor.GetImageAsync(locationId, filename);

            if (imageBytes == null)
            {
                // Image doesn't exist on S3
                // Cache a "null"
                await imageCache.SetAsync(imagePath, new byte[1] { 0 });
                return null;
            }

            // Store the image data
            await imageCache.SetAsync(imagePath, imageBytes);

            return imageBytes;
        }

        public bool IsValid(long locationId, string filename)
        {
            var parts = filename.Split('.');

            if (parts.Length != 2 || locationId < 0)
            {
                return false;
            }

            return Guid.TryParse(parts[0], out _) && IsValidExtension(parts[1]);
        }

        private static bool IsValidExtension(string extension)
        {
            return new string[] { "png", "bmp", "jpg", "jpeg", "svg"}.Contains(extension);
        }

        private static string BuildImagePath(long locationId, string filename)
        {
            return string.Join(@"/", locationId, filename);
        }

        public async Task<bool> DeleteAsync(ApiKey key)
        {
            var succeeded = await imagePersistor.DeleteAsync(key);

            if (!succeeded)
            {
                return succeeded;
            }

            // Delete all images belonging to the api key
            return true;
        }

        public async Task<bool> DeleteAsync(ApiKey key, string filename)
        {
            var succeeded = await imagePersistor.DeleteAsync(key, filename);

            if (!succeeded)
            {
                return succeeded;
            }

            await imageCache.RemoveAsync(BuildImagePath(key.LocationId, filename));

            return true;
        }

        public async Task<bool> UploadAsync(ApiKey apiKey, string filename, byte[] content)
        {
            var succeeded = await imagePersistor.UploadAsync(apiKey, filename, content);

            if (!succeeded)
            {
                return succeeded;
            }

            await imageCache.SetAsync(BuildImagePath(apiKey.LocationId, filename), content);

            return true;
        }

        public async Task<RestfulQrCode> CreateAsync(RestfulQrCode restfulQrCode)
        {
            return await repository.CreateAsync(restfulQrCode);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await repository.DeleteAsync(id);
        }

        public async Task<bool> DeleteAllAsync(ApiKey apiKey)
        {
            return await repository.DeleteAllAsync(apiKey);
        }
    }
}
