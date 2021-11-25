using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestfulQr.Domain;

namespace RestfulQr.Api.Services
{
    public interface IApiKeyService
    {
        public Task<ApiKey?> GetAsync(string key);

        public Task<ApiKey> UpdateLastUsedAsync(ApiKey key);

        public Task<ApiKey?> CreateAsync();

        public Task<bool> DeleteAsync(string key);
    }
}