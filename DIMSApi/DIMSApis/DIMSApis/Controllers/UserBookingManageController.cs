#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DIMSApis.Models.Data;
using System.Security.Claims;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;
using DIMSApis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace DIMSApis.Controllers
{
    [Authorize(Roles = "USER")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserBookingManageController : ControllerBase
    {
        private readonly IBookingManage _book;
        private readonly IMapper _mapper;

        public UserBookingManageController(IBookingManage book, IMapper mapper)
        { 
            _book = book;
            _mapper = mapper;
        }

        [HttpPost("user-Payment-Processing")]
        public async Task<IActionResult> UserPaymentProcessing(PaymentProcessingInput ppi)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            string check = await _book.PaymentProcessing(ppi , userId);
            if (check.Equals("1"))
            {
                return Ok("Paid Success");
            }
            else if (check.Equals("3"))
            {
                return NoContent();
            }
            else
            {
                return BadRequest("Error");
            }
        }
        [HttpGet("user-Booking-list")]
        public async Task<IActionResult> GetBooking()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var Booking = await _book.GetListBookingInfo(userId);
            var returnBooking = _mapper.Map<IEnumerable<BookingInfoOutput>>(Booking);
            return Ok(returnBooking);
        }

        [HttpGet("Vertify-Voucher")]
        public async Task<IActionResult> VertifyVouvher(int hotelId,string VoucherCode)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var voucher = await _book.VertifyVouvher(hotelId, VoucherCode);
            if(voucher == null) { return BadRequest("Wrong code"); }
            return Ok(voucher);
        }
    }
}
