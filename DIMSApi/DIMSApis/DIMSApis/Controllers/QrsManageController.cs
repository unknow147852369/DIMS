#nullable disable

using AutoMapper;
using DIMSApis.Interfaces;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DIMSApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QrsManageController : ControllerBase
    {
        private readonly IQrManage _qrmanage;
        private readonly IMapper _mapper;

        public QrsManageController(IQrManage qrmanage, IMapper mapper)
        {
            _qrmanage = qrmanage;
            _mapper = mapper;
        }

        [HttpGet("get_Qr_Booking_list")]
        public async Task<IActionResult> GetBooking(QrInput qrIn)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var qrList = await _qrmanage.getQrString(qrIn);
            var returnQrList = _mapper.Map<IEnumerable<QrOutput>>(qrList);
            return Ok(returnQrList);
        }

        [HttpGet("get_Qr_Booking_list")]
        public async Task<IActionResult> create(QrInput qrIn)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var qrList = await _qrmanage.getQrString(qrIn);
            var returnQrList = _mapper.Map<IEnumerable<QrOutput>>(qrList);
            return Ok(returnQrList);
        }
    }
}