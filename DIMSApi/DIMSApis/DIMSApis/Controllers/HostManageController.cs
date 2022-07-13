using DIMSApis.Interfaces;
using DIMSApis.Models.Helper;
using DIMSApis.Models.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DIMSApis.Controllers
{
    [Authorize(Roles = "HOST")]
    [Route("api/[controller]")]
    [ApiController]
    public class HostManageController : ControllerBase
    {
        private readonly IHostManage _host;

        public HostManageController(IHostManage host)
        {
            _host = host;
        }


        [HttpPut("Checkout")]
        public async Task<IActionResult> CheckOut(int hotelId,int bookingID)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (bookingID == null) return BadRequest(new DataRespone { Message = "some feild is empty" });
            var check = await _host.CheckOut(hotelId,bookingID);
            if (check == null) { return BadRequest(new DataRespone { Message = "Booking not found" }); }
            return Ok(check);
        }

        [HttpGet("Get-list-Menu")]
        public async Task<IActionResult> GetListMenus(int hotelID)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if(hotelID == null) return BadRequest(new DataRespone { Message = "some feild is emptyw" });
            var check = await _host.GetListMenu(hotelID);
            if (check == null) { return BadRequest(new DataRespone { Message = "Your hotel do not have any menu" }); }
            return Ok(check);
        }

        [HttpPost("Check-Room-Date-Booking")]
        public async Task<IActionResult> CheckRoomDateBooking(CheckRoomDateInput chek)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _host.CheckRoomDateBooking(chek);
            if (check == null) { return BadRequest(new DataRespone { Message = check }); }
            return Ok(check);
        }

        [HttpPost("Host-Local-Payment-final")]
        public async Task<IActionResult> LocalPaymentFinal(LocalPaymentInput ppi)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            string check = await _host.LocalPaymentFinal(ppi, userId);
            if (check.Equals("1"))
            {
                return Ok(new DataRespone { Message = "Paid Success" });
            }
            else if (check.Equals("3"))
            {
                return Ok(new DataRespone { Message = "" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = check });
            }
        }
      

        [HttpGet("Host-A-Hotel-All-Room-Status-CheckOut")]
        public async Task<IActionResult> GetListAHotelAllRoomStatusCheckOut(int hotelId, DateTime today)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var Hotel = await _host.GetListAHotelAllRoomStatusCheckOut(userId, hotelId, today);
            if (Hotel == null) { return BadRequest(new DataRespone { Message = "No Rooms Create!" }); }
            return Ok(Hotel);
        }

        [HttpGet("Host-A-Hotel-All-Room-Status-Today")]
        public async Task<IActionResult> GetListAHotelAllRoomStatusToday(int hotelId, DateTime today)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var Hotel = await _host.GetListAHotelAllRoomStatusToday(userId, hotelId, today);
            if (Hotel == null) { return BadRequest(new DataRespone { Message = "No Rooms Create!" }); }
            return Ok(Hotel);
        }

        [HttpGet("Host-A-Hotel-All-Room-Status-Search")]
        public async Task<IActionResult> GetListAHotelAllRoomStatusSearch(int hotelId, DateTime ArrivalDate, int totalnight)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var Hotel = await _host.GetListAHotelAllRoomStatusSearch(userId, hotelId, ArrivalDate, totalnight);
            if (Hotel == null) { return BadRequest(new DataRespone { Message = "No Rooms Create!" }); }
            return Ok(Hotel);
        }

        [HttpGet("Host-A-Detail-Room")]
        public async Task<IActionResult> GetADetailRoom(int RoomId, DateTime today)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var roomDetail = await _host.GetADetailRoom(userId, RoomId, today);
            if (roomDetail == null) { return BadRequest(new DataRespone { Message = "Room not Found!" }); }
            return Ok(roomDetail);
        }

        [HttpGet("Host-All-Hotel")]
        public async Task<IActionResult> GetListAllHotel()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var Hotel = await _host.GetListAllHotel(userId);
            return Ok(Hotel);
        }

        [HttpGet("Host-A-Hotel-All-Info")]
        public async Task<IActionResult> GetAHotelAllInfo(int hotelId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var HotelRoom = await _host.GetAHotelAllInfo(hotelId, userId);
            return Ok(HotelRoom);
        }

        [HttpGet("List-A-Hotel-Photo")]
        public async Task<IActionResult> GetListHotelPhotos(int hotelId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var HotelRoom = await _host.GetListHotelPhotos(hotelId, userId);
            if (HotelRoom == null) { return BadRequest("Not Found"); }
            return Ok(HotelRoom);
        }

        [HttpPut("Update-Hotel-MainPhoto")]
        public async Task<IActionResult> UpdateHotelMainPhoto(int hotelId, int photoId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _host.UpdateHotelMainPhoto(photoId, hotelId);
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