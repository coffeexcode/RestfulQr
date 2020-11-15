using Microsoft.AspNetCore.Mvc;

namespace RestfulQr.Controllers.Images.V1
{
    [ApiController]
    [Route("images")]
    [Produces("application/json", "image/png")]
    public class ImagesController : ControllerBase
    {
    }
}
