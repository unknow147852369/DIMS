using DIMSApis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DIMSApis.Controllers
{
    [Authorize(Roles = "HOST")]
    [Route("api/[controller]")]
    [ApiController]
    public class HostQrController : ControllerBase
    {
        private readonly IHostQr _hostqr;

        public HostQrController(IHostQr hostqr)
        {
            _hostqr = hostqr;
        }
    }
}