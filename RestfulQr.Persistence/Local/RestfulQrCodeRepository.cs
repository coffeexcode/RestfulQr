using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using RestfulQr.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RestfulQr.Persistence.Local
{
    public class RestfulQrCodeRepository : IRestfulQrCodeRepository
    {
        private readonly string connectionString;

        public RestfulQrCodeRepository(IConfiguration config)
        {
            connectionString = config.GetConnectionString("QrCodeDb");
        }

        public async Task<IList<RestfulQrCode>> GetAllAsync(ApiKey apiKey)
        {
            using var connection = new NpgsqlConnection(connectionString);

            var result = await connection.QueryAsync<RestfulQrCode>(
                @"SELECT * FROM qr_codes.qr_codes WHERE created_by = @CreatedBy", new { CreatedBy = apiKey.Id}
            );

            return result.ToList();
        }

        public async Task<RestfulQrCode> GetAsync(Guid id)
        {
            using var connection = new NpgsqlConnection(connectionString);

            return await connection.QueryFirstOrDefaultAsync<RestfulQrCode>(
                @"SELECT * FROM qr_codes.qr_codes WHERE id = @Id", new { Id = id });
        }

        public async Task<RestfulQrCode> CreateAsync(RestfulQrCode restfulQrCode)
        {
            using var connection = new NpgsqlConnection(connectionString);

            var id = await connection.ExecuteScalarAsync<Guid>(
                @"INSERT INTO qr_codes.qr_codes 
                    (id, created_by, created, type, render_type, filename, model, public_url) VALUES (@Id, @CreatedBy, @Created, @Type, @RenderType, @Filename, @Model, @PublicUrl) RETURNING id", new
                {
                    restfulQrCode.Id,
                    restfulQrCode.CreatedBy,
                    restfulQrCode.Created,
                    restfulQrCode.Type,
                    restfulQrCode.RenderType,
                    restfulQrCode.Filename,
                    restfulQrCode.PublicUrl,
                    Model = JsonSerializer.Serialize(restfulQrCode.Model)
                });

            return await GetAsync(id);
        }

        public async Task<bool> DeleteAllAsync(ApiKey apiKey)
        {
            using var connection = new NpgsqlConnection(connectionString);

            await connection.ExecuteAsync(
                @"DELETE FROM qr_codes.qr_codes WHERE created_by = @Id", new { Id = apiKey.Id });

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using var connection = new NpgsqlConnection(connectionString);

            await connection.ExecuteAsync(
                @"DELETE FROM qr_codes.qr_codes WHERE id = @Id", new { Id = id });

            return true;
        }
    }
}
