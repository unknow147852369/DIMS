using DIMSApis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DIMSApis.Controllers
{
    [Authorize(Roles = "USER")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserQrController : ControllerBase
    {
        private readonly IUserQr _userqr;

        public UserQrController(IUserQr userqr)
        {
            _userqr = userqr;
        }
    }
}
