using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using RestfulQr.Domain;

namespace RestfulQr.Persistence.Local
{
    public class ApiKeyRepository : IApiKeyRepository
    {
        private readonly string connectionString;

        public ApiKeyRepository(IConfiguration config)
        {
            connectionString = config.GetConnectionString("QrCodeDb");
        }

        public async Task<ApiKey> CreateAsync(ApiKey toCreate)
        {
            using var connection = new NpgsqlConnection(connectionString);

            var id = await connection.ExecuteScalarAsync<Guid>(
                @"INSERT INTO identity.api_keys
                    (id, created, last_used) VALUES (@Id, @Created, @LastUsed) RETURNING id", toCreate
            );

            return await GetAsync(id);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using var connection = new NpgsqlConnection(connectionString);

            return await connection.ExecuteAsync(
               @"DELETE FROM identity.api_keys WHERE id = @Id", new { Id = id }) == 1;
        }

        public async Task<ApiKey> GetAsync(Guid id)
        {
            using var connection = new NpgsqlConnection(connectionString);

            DefaultTypeMap.MatchNamesWithUnderscores = true;

            return await connection.QueryFirstOrDefaultAsync<ApiKey>(
                @"SELECT * FROM identity.api_keys WHERE id = @Id", new { Id = id });
        }

        public async Task<ApiKey> UpdateAsync(ApiKey apiKey)
        {
           using var connection = new NpgsqlConnection(connectionString);

           await connection.ExecuteScalarAsync(
               @"UPDATE identity.api_keys SET last_used = @LastUsed WHERE id = @Id", apiKey
           );

           return apiKey;
        }
    }
}