using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestfulQr.Domain;

namespace RestfulQr.Persistence.Local
{
    /// <summary>
    /// Describes functionality for CRUD operations for api keys.
    /// </summary>
    public interface IApiKeyRepository
    {
        /// <summary>
        /// Retrieve an api key from the backing data store
        /// </summary>
        /// <param name="key">The key to retrieve</param>
        /// <returns></returns>
        public Task<ApiKey> GetAsync(Guid id);

        /// <summary>
        /// Create an api key in the backing store.
        /// </summary>
        /// <returns></returns>
        public Task<ApiKey> CreateAsync(ApiKey toCreate);

        /// <summary>
        /// Delete an api key from the backing store.
        /// </summary>
        /// <param name="key">The key to delete</param>
        /// <returns></returns>
        public Task<bool> DeleteAsync(Guid id);


        /// <summary>
        /// Updates an api key in the backing store.
        /// </summary>
        /// <param name="apiKey">The key to update</param>
        /// <returns></returns>
        public Task<ApiKey> UpdateAsync(ApiKey toUpdate);
    }
}