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

        [HttpGet("get_Qr_Booking_list")]
        public async Task<IActionResult> GetQrListImage(int bookingID)
        {
            //int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
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

        [HttpPost("QrAutoCreate")]
        public async Task<IActionResult> AutoCreateQR(int bookingID)
        {
            if (await _qrmanage.CreateBookingQrString(bookingID) == 1)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("vertify_Qr_Content")]
        public async Task<IActionResult> VertifyQrContent(VertifyQrInput qrIn)
        {
            //int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var qrList = await _qrmanage.vertifyQrContent(qrIn);
            if (qrList == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(qrList);
            }
        }
    }
}