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

namespace DIMSApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBookingManageController : ControllerBase
    {
        private readonly DIMSContext _context;

        public UserBookingManageController(DIMSContext context)
        {
            _context = context;
        }

        //[HttpPost("user_request")]
        //public async Task<IActionResult> SendRentRequest(RentRequest rent)
        //{
        //    int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        //    //var rooms = await _repo.SendRentRequest(rent, userId);
        //    //var returnRoom = _mapper.Map<IEnumerable<ListRoom>>(rooms);
        //    return Ok(returnRoom);
        //}
    }
}
