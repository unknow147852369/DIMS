using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DIMSApis.Models.Data;
using Microsoft.AspNetCore.Authorization;
using DIMSApis.Interfaces;
using DIMSApis.Models.Input;
using System.Security.Claims;
using DIMSApis.Models.Helper;

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
        public async Task<IActionResult> GetListAHotelAllRoomStatusSearch(int hotelId,DateTime ArrivalDate, int totalnight)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var Hotel = await _host.GetListAHotelAllRoomStatusSearch(userId, hotelId, ArrivalDate, totalnight);
            if(Hotel == null) { return BadRequest(new DataRespone { Message = "No Rooms Create!" }); }
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
            if(HotelRoom == null) { return BadRequest("Not Found"); }
            return Ok(HotelRoom);
        }

        [HttpPut("Update-Hotel-MainPhoto")]
        public async Task<IActionResult> UpdateHotelMainPhoto(int hotelId,int photoId)
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
