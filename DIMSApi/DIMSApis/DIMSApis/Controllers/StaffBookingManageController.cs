#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DIMSApis.Models.Data;
using System.Security.Claims;
using DIMSApis.Interfaces;

namespace DIMSApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffBookingManageController : ControllerBase
    {
        private readonly IBookingManage _booking;

        public StaffBookingManageController(IBookingManage booking)
        { 
            _booking = booking;
        }

        [HttpPut("accept/{id}")]
        public async Task<IActionResult> AcceptRent(int BooingId, int[] roomId ,string condition)
        { 
            int check = await _booking.AccecptBookingRequest( BooingId,  roomId,  condition);
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

        //[HttpPut("deny/{id}")]
        //public async Task<IActionResult> DenyRent(int id)
        //{
        //    int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        //    bool check = await _repo.DenyRent(id, userId);
        //    if (check)
        //    {
        //        return Ok();
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}
    }
}
