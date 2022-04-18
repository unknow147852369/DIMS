#nullable disable
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

namespace DIMSApis.Controllers
{
    [Authorize(Roles = "CEO")]
    [Route("api/[controller]")]
    [ApiController]
    public class HotelManageController : ControllerBase
    {
        private readonly DIMSContext _context;

        public HotelManageController(DIMSContext context)
        {
            _context = context;
        }

        //[HttpPost("create")]
        //public async Task<IActionResult> CreateRoom(CreateRoomInput room)
        //{
        //    int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        //    int check = await _repo.AddRoom(room, userId);
        //    if (check == 2)
        //    {
        //        return Unauthorized();
        //    }
        //    else if (check == 1)
        //    {
        //        await _hubContext.Clients.All.BroadcastMessage();
        //        return Ok();
        //    }
        //    else if (check == 3)
        //    {
        //        return NoContent();
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}
    }
}
