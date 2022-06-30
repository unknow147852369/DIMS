#nullable disable

using AutoMapper;
using DIMSApis.Interfaces;
using DIMSApis.Models.Helper;
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


        //[HttpPost("vertify-Qr")]
        //public async Task<IActionResult> VertifyQrContent(VertifyQrInput qrIn)
        //{
        //    var qrcheck = await _qrmanage.vertifyQrContent(qrIn);

        //    return Ok(qrcheck);
        //}

        [HttpPost("vertify-Main-Qr")]
        public async Task<IActionResult> vertifyMainQrCheckIn(VertifyMainQrInput qrIn)
        {
            var qrcheck = await _qrmanage.vertifyMainQrCheckIn(qrIn);
            if(qrcheck == null) { return BadRequest(new DataRespone { Message = "Wrong Main QR" }); }
            return Ok(qrcheck);
        }

        [HttpGet("check-Room-lock")]
        public async Task<IActionResult> getStringToCheckRoom(int hotelId, String roomName)
        {
            var check = await _qrmanage.getStringToCheckRoom(hotelId, roomName);
            return Ok(new DataRespone { Message = check });
        }

        [HttpPost("Check-in")]
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

        //[HttpPost("Check-out")]
        //public async Task<IActionResult> CheckOut(CheckOutInput ckOut)
        //{
        //    var checkOut = await _qrmanage.CheckOut(ckOut);

        //    if (checkOut.Equals("1"))
        //    {
        //        return Ok();
        //    }
        //    else if (checkOut.Equals("3"))
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