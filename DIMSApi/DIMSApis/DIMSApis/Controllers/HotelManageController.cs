using DIMSApis.Interfaces;
using DIMSApis.Models.Helper;
using DIMSApis.Models.Input;
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
            var HotelRoom = await _hotel.GetListHotelPhotos(userId,hotelId);
            if (HotelRoom == null) { return BadRequest(new DataRespone { Message = "Not found" }); }
            return Ok(HotelRoom);
        }

        [HttpPut("Update-Hotel-MainPhoto")]
        public async Task<IActionResult> UpdateHotelMainPhoto(int hotelId, int photoId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _hotel.UpdateHotelMainPhoto(photoId, hotelId);
            if (check.Equals("1"))
            {
                return Ok(new DataRespone { Message = "Update success" });
            }
            else if (check.Equals("3"))
            {
                return Ok(new DataRespone { Message = "Update success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "Update fail" });
            }
        }
        [HttpPost("Add-A-Hotel-Photos")]
        public async Task<IActionResult> AddAHotelPhotos(ICollection<NewHotelPhotosInput> newPhotos)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var HotelRoom = await _hotel.AddAHotelPhotos(userId, newPhotos);
            if (HotelRoom == null) { return BadRequest("Not Found"); }
            if (HotelRoom.Equals("1"))
            {
                return Ok(new DataRespone { Message = "Add success" });
            }
            else if (HotelRoom.Equals("3"))
            {
                return Ok(new DataRespone { Message = "Add success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "Add fail" });
            }
        }
        [HttpPost("Remove-A-Hotel-Photos")]
        public async Task<IActionResult> RemoveAHotelPhotos(int PhotoId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var HotelRoom = await _hotel.RemoveAHotelPhotos(PhotoId);
            if (HotelRoom == null) { return BadRequest("Not Found"); }
            if (HotelRoom.Equals("1"))
            {
                return Ok(new DataRespone { Message = "Remove success" });
            }
            else if (HotelRoom.Equals("3"))
            {
                return Ok(new DataRespone { Message = "Remove success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "Remove fail" });
            }
        }
    }
}
