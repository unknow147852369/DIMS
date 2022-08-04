using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
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
       
        [HttpPut("Add-inbound-user-id")]
        public async Task<IActionResult> AddInboundUser(checkInInput ckIn)
        {
            var checkIn = await _host.AddInboundUser(ckIn);

            if (checkIn.Equals("1"))
            {
                return Ok(new DataRespone { Message = "Update success" });
            }
            else if (checkIn.Equals("3"))
            {
                return Ok(new DataRespone { Message = "nothing change" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "Wrong inform" });
            }
        }

        [HttpPut("Checkout-Local")]
        public async Task<IActionResult> CheckOut(int hotelId,int bookingID)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (bookingID == null) return BadRequest(new DataRespone { Message = "some feild is empty" });
            var check = await _host.CheckOutLocal(hotelId,bookingID);
            if (check.Equals("1"))
            {
                return Ok("CheckOut Success!");
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
        [HttpPut("Update-Clean-Status")]
        public async Task<IActionResult> CheckOut(int roomID)
        {
            if (roomID == null) return BadRequest(new DataRespone { Message = "some feild is empty" });
            var check = await _host.UpdateCleanStatus(roomID);
            if (check.Equals("1"))
            {
                return Ok("Update Success!");
            }
            else if (check.Equals("3"))
            {
                return NoContent();
            }
            else
            {
                return BadRequest("Not found");
            }
        }
        [HttpPut("Update-Item-Menu")]
        public async Task<IActionResult> UpdateItemMenu(int MenuID, ItemMenuInput item)
        {
            var check = await _host.UpdateItemMenu(MenuID,item);
            if (check != "1" && check != "3") { return BadRequest(new DataRespone { Message = "Update fail" }); }
            return Ok(new DataRespone { Message = "update Suceess" });
        }
        [HttpPost("Add-Item-Menu")]
        public async Task<IActionResult> AddItemMenu(ICollection<ItemMenuInput> item)
        {
            var check = await _host.AddItemMenu(item);
            if (check != "1" && check != "3") { return BadRequest(new DataRespone { Message = "Add fail" }); }
            return Ok(new DataRespone { Message = "Add Suceess" });
        }
        [HttpPost("Add-Problem-Extra-Fee")]
        public async Task<IActionResult> AddProblemForExtraFee(ICollection<ProblemExtraFeeInput> prEX)
        {
            var check = await _host.AddProblemForExtraFee(prEX);
            if (check != "1" && check != "3") { return BadRequest(new DataRespone { Message = "Add fail" }); }
            return Ok(new DataRespone { Message = "Add Suceess" });
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

        [HttpGet("Get-User-Menu")]
        public async Task<IActionResult> GetUserMenu(int BookingDetailID)
        {
            if (BookingDetailID == null) return BadRequest(new DataRespone { Message = "some feild is emptyw" });
            var check = await _host.GetUserMenu(BookingDetailID);
            if (check == null) { return BadRequest(new DataRespone { Message = "Your user do not use any thing" }); }
            return Ok(check);
        }

        [HttpGet("Get-All-Inbound-User-Booking-info")]
        public async Task<IActionResult> GetAllInboundUserBookingInfo(int hotelID)
        {
            var check = await _host.GetAllInboundUserBookingInfo(hotelID);
            if (check == null) { return BadRequest(new DataRespone { Message = "Your user do not use any thing" }); }
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
        [HttpPost("Add-Item-For-Extrafee")]
        public async Task<IActionResult> AddItemForExtraFee(ICollection<ExtraFeeMenuDetailInput> chek)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _host.AddItemForExtraFee(chek);
            if (check != "1" && check != "3") { return BadRequest(new DataRespone { Message = "Add fail" }); }
            return Ok(new DataRespone { Message = "Add Suceess" });
        }

        [HttpDelete("Delete-Item-For-Extrafee")]
        public async Task<IActionResult> DeleteItemForExtraFee(int BookingDetailId,int BookingDetailMenuId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _host.DeleteItemForExtraFee(BookingDetailId, BookingDetailMenuId);
            if (check != "1" && check != "3") { return BadRequest(new DataRespone { Message = "remove fail" }); }
            return Ok(new DataRespone { Message = "remove Suceess" });
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

        [HttpGet("Host-A-Hotel-Only-Room-Status-13-Search")]
        public async Task<IActionResult> GetListAHotelOnlyRoomStatus13Search(int hotelId, DateTime ArrivalDate, int totalnight)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var Hotel = await _host.GetListAHotelOnlyRoomStatus13Search(userId, hotelId, ArrivalDate, totalnight);
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

        [HttpGet("Get-Money-Checkout-info-By-Filter")]
        public async Task<IActionResult> GetFullRoomMoneyDetailByDate(int hotelId, DateTime startDate ,DateTime endDate)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _host.GetFullRoomMoneyDetailByFilter(hotelId, startDate,endDate);
            if (check == null && check.Bookings.Count() == 0) { return BadRequest(new DataRespone { Message = "No date to calculate!" }); }
            return Ok(check);
        }

        [HttpGet("Get-Money-not-Checkout-info-By-Filter")]
        public async Task<IActionResult> GetFullRoomMoneyNotCheckOutDetailByDate(int hotelId, DateTime startDate, DateTime endDate)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _host.GetFullRoomMoneyNotCheckOutDetailByDate(hotelId, startDate, endDate);
            if (check == null && check.Bookings.Count() == 0) { return BadRequest(new DataRespone { Message = "No date to calculate!" }); }
            return Ok(check);
        }

        [HttpGet("Get-All-Book-By-Page")]
        public async Task<IActionResult> HostgetListBookingByPage(int hotelId,int currentPage,int PageSize)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _host.HostgetListBookingByPage<Booking>(hotelId, currentPage, PageSize);
            if (check == null && check.Result.Count() == 0) { return BadRequest(new DataRespone { Message = "No Data!" }); }
            return Ok(check);
        }

        [HttpGet("Get-A-Book-Full-Detail")]
        public async Task<IActionResult> HostGetABookingFullDetail(int  bookingID)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _host.HostGetABookingFullDetail(bookingID);
            if (check == null) { return BadRequest(new DataRespone { Message = "No Data!" }); }
            return Ok(check);
        }
    }
}