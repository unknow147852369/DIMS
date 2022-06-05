using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DIMSApis.Models.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DIMSApis.Interfaces;

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

        [HttpPost("Admin_Acecpt_Host")]
        public async Task<IActionResult> AcecptHost(int userId)
        {
            int check = await _admin.AcpectHost(userId);
            if (check == 1)
            {
                return Ok();
            }
            else if (check == 3)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("Admin_Acecpt_Hotel")]
        public async Task<IActionResult> AcecptHotel(int HotelId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            int check = await _admin.AcpectHotel(HotelId);
            if (check == 1)
            {
                return Ok();
            }
            else if (check == 3)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
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

    }
}
