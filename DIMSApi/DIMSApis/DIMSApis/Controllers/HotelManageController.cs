using DIMSApis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DIMSApis.Controllers
{
    [Authorize(Roles = "HOST")]
    [Route("api/[controller]")]
    [ApiController]
    public class HotelManageController : ControllerBase
    {
        private readonly IHotelManage _hotel;

        public HotelManageController(IHotelManage hotel)
        {
            _hotel = hotel;
        }
        [HttpGet("List-A-Hotel-Photo")]
        public async Task<IActionResult> GetListHotelPhotos(int hotelId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var HotelRoom = await _hotel.GetListHotelPhotos(hotelId, userId);
            if (HotelRoom == null) { return BadRequest("Not Found"); }
            return Ok(HotelRoom);
        }

        [HttpPut("Update-Hotel-MainPhoto")]
        public async Task<IActionResult> UpdateHotelMainPhoto(int hotelId, int photoId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _hotel.UpdateHotelMainPhoto(photoId, hotelId);
            if (check.Equals("1"))
            {
                return Ok("Update Success");
            }
            else if (check.Equals("3"))
            {
                return NoContent();
            }
            else
            {
                return BadRequest(check);
            }
        }

    }
}
