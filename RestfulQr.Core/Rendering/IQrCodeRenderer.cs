using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestfulQr.Domain;

namespace RestfulQr.Core.Rendering
{
    public interface IQrCodeRenderer
    {
        public Task<byte[]?> RenderAsync(QrCodeType type, object model);
    }
}