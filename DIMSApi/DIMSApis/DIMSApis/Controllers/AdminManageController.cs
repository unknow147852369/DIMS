using DIMSApis.Interfaces;
using DIMSApis.Models.Helper;
using DIMSApis.Models.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DIMSApis.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminManageController : ControllerBase
    {
        private readonly IAdminManage _admin;

        public AdminManageController(IAdminManage admin)
        {
            _admin = admin;
        }

        [HttpPut("Acpect-A-Hotel-Add-Request")]
        public async Task<IActionResult> AcpectHotelAddRequest(int hotelRequestId, int pendingStatus)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _admin.AcpectHotelAddRequest(hotelRequestId, pendingStatus);
            if (check.Equals("1"))
            {
                return Ok(new DataRespone { Message = "Send request success" });
            }
            else if (check.Equals("3"))
            {
                return Ok(new DataRespone { Message = "Send request success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "Send request fail " + check });
            }
        }
        [HttpPut("Acpect-A-Hotel-Update-Request")]
        public async Task<IActionResult> AcpectHotelUpdateRequest(int hotelRequestId, int pendingStatus)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _admin.AcpectHotelUpdateRequest(hotelRequestId, pendingStatus);
            if (check.Equals("1"))
            {
                return Ok(new DataRespone { Message = "Send request success" });
            }
            else if (check.Equals("3"))
            {
                return Ok(new DataRespone { Message = "Send request success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "Send request fail " + check });
            }
        }

        [HttpGet("List-Hotel-Add-Requests")]
        public async Task<IActionResult> GetListHotelAddRequests()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _admin.GetLitHotelAddRequests();
            if (check == null) { return BadRequest(new DataRespone { Message = "Empty" }); }
            return Ok(check);
        }
        [HttpGet("List-Hotel-Update-Requests")]
        public async Task<IActionResult> GetListHotelUpdateRequests()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _admin.GetListHotelUpdateRequests();
            if (check == null) { return BadRequest(new DataRespone { Message = "Empty" }); }
            return Ok(check);
        }


        [HttpPut("Admin-Acecpt-Host")]
        public async Task<IActionResult> AcecptHost(int userId)
        {
            int check = await _admin.AcpectHost(userId);
            if (check == 1)
            {
                return Ok("Accpect complete!");
            }
            else if (check == 3)
            {
                return NoContent();
            }
            else
            {
                return BadRequest("User not exist!");
            }
        }

        [HttpPut("Admin-Acecpt-Hotel")]
        public async Task<IActionResult> AcecptHotel(int HotelId)
        {
            int check = await _admin.AcpectHotel(HotelId);
            if (check == 1)
            {
                return Ok("Accpect Success!");
            }
            else if (check == 3)
            {
                return NoContent();
            }
            else
            {
                return BadRequest("Hotel not exist!");
            }
        }

        [HttpGet("List-All-Hotel")]
        public async Task<IActionResult> ListAllHotel()
        {
            var Hotel = await _admin.ListAllHotel();
            if (Hotel.Count() == 0) { return NotFound("No request"); }
            return Ok(Hotel);
        }

        [HttpGet("List-All-User")]
        public async Task<IActionResult> ListAllHost()
        {
            var Host = await _admin.ListAllHost();
            if (Host.Count() == 0) { return NotFound("No Accpect"); }
            return Ok(Host);
        }

        [HttpPost("Admin-Create-User")]
        public async Task<IActionResult> AdminCreateUser(AdminRegisterInput userinput)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _admin.AdminCreateUser(userinput);
            if (check == "1")
            {
                return Ok("Create Success");
            }
            else if (check == "3")
            {
                return NoContent();
            }
            else
            {
                return BadRequest("Create failed");
            }
        }
    }
}