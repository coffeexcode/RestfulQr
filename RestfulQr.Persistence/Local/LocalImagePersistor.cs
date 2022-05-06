using Microsoft.Extensions.Configuration;
using RestfulQr.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulQr.Persistence.Local
{
    public class LocalImagePersistor : IImagePersistor
    {
        private readonly string baseFolderPath;

        public LocalImagePersistor(IConfiguration config)
        {
            baseFolderPath = config.GetSection("LocalStorage")["Path"];
        }

        public Task<bool> DeleteAsync(ApiKey apiKey)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(ApiKey apiKey, string filename)
        {
            var fileToDelete = Path.Combine(baseFolderPath, apiKey.LocationId.ToString(), filename);

            File.Delete(fileToDelete);

            return Task.FromResult(!File.Exists(fileToDelete));
        }

        public async Task<byte[]?> GetImageAsync(ApiKey apiKey, string filename)
        {
            var file = Path.Combine(Path.Combine(baseFolderPath, apiKey.LocationId.ToString(), filename));
            if (!File.Exists(file)) {
                return default;
            }

            var bytes = await File.ReadAllBytesAsync(file);

            return bytes;
        }

        public async Task<bool> UploadAsync(ApiKey apiKey, string filename, byte[] content)
        {
            var folder = Path.Combine(baseFolderPath, apiKey.LocationId.ToString());
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var file = Path.Combine(folder, filename);

            await File.WriteAllBytesAsync(file, content);

            return File.Exists(file);
        }
    }
}
