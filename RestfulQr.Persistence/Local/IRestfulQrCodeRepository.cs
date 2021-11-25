using RestfulQr.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulQr.Persistence.Local
{
    public interface IRestfulQrCodeRepository
    {
        public Task<IList<RestfulQrCode>> GetAllAsync(ApiKey apiKey);

        public Task<RestfulQrCode> CreateAsync(RestfulQrCode restfulQrCode);

        public Task<bool> DeleteAsync(Guid id);

        public Task<bool> DeleteAllAsync(ApiKey apiKey);
    }
}
