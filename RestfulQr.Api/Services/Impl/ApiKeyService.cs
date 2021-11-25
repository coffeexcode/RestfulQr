using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestfulQr.Api.Core.Caching;
using RestfulQr.Domain;
using RestfulQr.Persistence.Local;
using Serilog;

namespace RestfulQr.Api.Services.Impl
{
    public class ApiKeyService : IApiKeyService
    {
        private readonly IApiKeyRepository repository;

        private readonly ICacheProvider<ApiKey> cache;

        public ApiKeyService(IApiKeyRepository repository, ICacheProvider<ApiKey> cache)
        {
            this.repository = repository;
            this.cache = cache;
        }

        public async Task<ApiKey?> CreateAsync()
        {
            var now = DateTime.Now.ToUniversalTime();

            var toCreate = new ApiKey
            {
                Id = Guid.NewGuid(),
                Created = now,
                LastUsed = now
            };

            var created = await repository.CreateAsync(toCreate);

            if (created == null)
            {
                return null;
            }

            await cache.SetAsync(created.Id.ToString(), created);
            
            return created;
        }

        public async Task<ApiKey?> GetAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key) || !Guid.TryParse(key, out Guid id)) {
                return null;
            }

            // Check the cache
            var cacheResult = await cache.GetAsync(key);

            if (cacheResult != null) 
            {
                return cacheResult;
            }

            // Check the db
            var dbResult = await repository.GetAsync(id);

            if (dbResult == null) {
                return null;
            }

            // Update the cache with the entry
            await cache.SetAsync(key, dbResult);

            return dbResult;
        }

        public async Task<ApiKey> UpdateLastUsedAsync(ApiKey key)
        {
            key.LastUsed = DateTime.Now.ToUniversalTime();

            // Send the key to the database and the store
            try {
                await Task.WhenAll(
                    cache.SetAsync(key.Id.ToString(), key),
                    repository.UpdateAsync(key)
                );
            } catch (Exception) {
                await cache.RemoveAsync(key.Id.ToString());
                throw;
            }

            return key;
        }
    
        public async Task<bool> DeleteAsync(string key) {
            if (string.IsNullOrWhiteSpace(key) || !Guid.TryParse(key, out Guid id)) {
                return true;
            }

            await Task.WhenAll(
                cache.RemoveAsync(key),
                repository.DeleteAsync(id)
            );

            return true;
        }
    }
}