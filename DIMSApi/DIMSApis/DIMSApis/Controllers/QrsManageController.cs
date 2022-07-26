#nullable disable

using AutoMapper;
using DIMSApis.Interfaces;
using DIMSApis.Models.Helper;
using DIMSApis.Models.Input;
using Microsoft.AspNetCore.Mvc;

namespace DIMSApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QrsManageController : ControllerBase
    {
        private readonly IQrManage _qrmanage;
        private readonly IMapper _mapper;
        private readonly IGenerateQr _generateqr;

        public QrsManageController(IQrManage qrmanage, IMapper mapper, IGenerateQr generateqr)
        {
            _qrmanage = qrmanage;
            _mapper = mapper;
            _generateqr = generateqr;
        }

        [HttpGet("vertify-Qr-Room")]
        public async Task<IActionResult> VertifyQrContent(int HotelId, string RoomName, string QrContent)
        {
            var qrcheck = await _qrmanage.vertifyQrContent(HotelId,RoomName,QrContent);

            return Ok(qrcheck);
        }

        [HttpPost("Check-in-Main-Qr")]
        public async Task<IActionResult> vertifyMainQrCheckIn(VertifyMainQrInput qrIn)
        {
            var qrcheck = await _qrmanage.vertifyMainQrCheckIn(qrIn);
            if (qrcheck == null) { return BadRequest(new DataRespone { Message = "Wrong Main QR" }); }
            return Ok(qrcheck);
        }

        [HttpGet("check-Room-lock")]
        public async Task<IActionResult> getStringToCheckRoom(int hotelId, String roomName)
        {
            var check = await _qrmanage.getStringToCheckRoom(hotelId, roomName);
            if (check == "") { return BadRequest("null"); }
            return Ok(check);
        }

 

        [HttpPut("Checkin-Online")]
        public async Task<IActionResult> CheckInOnline(int hotelId, int bookingID)
        {
            var checkOut = await _qrmanage.CheckInOnline(hotelId,bookingID);

            if (checkOut.Equals("1"))
            {
                return Ok("CheckIn Success!");
            }
            else if (checkOut.Equals("3"))
            {
                return NoContent();
            }
            else
            {
                return BadRequest("Not found");
            }
        }

        [HttpPut("Checkout-Online")]
        public async Task<IActionResult> CheckOutOnline(int hotelId, int bookingID)
        {
            if (bookingID == null) return BadRequest(new DataRespone { Message = "some feild is empty" });
            var check = await _qrmanage.CheckOutOnline(hotelId, bookingID);
            if (check.Equals("1"))
            {
                return Ok("CheckIn Success!");
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
    }
}