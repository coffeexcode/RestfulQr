using RestfulQr.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulQr.Services.Impl
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly ICreatedQrCodeRepository repository;

        public AuthorizationService(
            ICreatedQrCodeRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> CanAccessAsync(string apiKey, string filename)
        {
            return await repository.CanAccessFileAsync(apiKey, filename);
        }
    }
}
