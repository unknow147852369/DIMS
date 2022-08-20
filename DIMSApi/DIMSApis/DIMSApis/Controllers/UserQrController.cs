using DIMSApis.Interfaces;
using DIMSApis.Models.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpPut("User-get-new-Qr-room")]
        public async Task<IActionResult> UserGetNewQrRoom(int bookingID, int bookingdetailID)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _userqr.UserGetNewQrRoom(bookingID, bookingdetailID);
            if (check.Equals("1"))
            {
                return Ok(new DataRespone { Message = "renew qr success" });
            }
            else if (check.Equals("3"))
            {
                return Ok(new DataRespone { Message = "renew qr success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "renew qr fail " + check });
            }
        }
    }
}