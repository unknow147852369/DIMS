#nullable disable

using AutoMapper;
using DIMSApis.Interfaces;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;
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

        [HttpGet("get_Qr_Booking_list_CHEAT")]
        public async Task<IActionResult> GetQrListImage(int bookingID)
        {
            var qrList = await _qrmanage.getListQrString(bookingID);
            if (qrList == null)
            {
                return BadRequest();
            }
            else
            {
                var returnQrList = _mapper.Map<IEnumerable<QrOutput>>(qrList);
                return Ok(returnQrList);
            }
        }


        [HttpPost("vertify_Qr")]
        public async Task<IActionResult> VertifyQrContent(VertifyQrInput qrIn)
        {
            var qrcheck = await _qrmanage.vertifyQrContent(qrIn);

            return Ok(qrcheck);
        }

        [HttpGet("check_Room_lock")]
        public async Task<IActionResult> checkRoomlock(int hotelId, String roomName)
        {
            var check = await _qrmanage.getStringToCheckRoom(hotelId, roomName);
            return Ok(check);
        }

        [HttpPost("Check_in")]
        public async Task<IActionResult> CheckIn(checkInInput ckIn)
        {
            var checkIn = await _qrmanage.checkIn(ckIn);

            if (checkIn.Equals("1"))
            {
                return Ok();
            }
            else if (checkIn.Equals("3"))
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("Check_out")]
        public async Task<IActionResult> CheckOut(CheckOutInput ckOut)
        {
            var checkOut = await _qrmanage.CheckOut(ckOut);

            if (checkOut.Equals("1"))
            {
                return Ok();
            }
            else if (checkOut.Equals("3"))
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }


    }
}