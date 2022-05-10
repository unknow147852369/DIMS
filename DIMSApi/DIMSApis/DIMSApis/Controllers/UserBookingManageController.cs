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
    [Authorize]
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

        [HttpPost("user_Payment")]
        public async Task<IActionResult> PaySelectedBooking(StripeInput stripein)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            int check = await _book.PayBooking(stripein, userId);
            if (check == 1)
            {
                return Ok();
            }
            else if (check == 3)
            {
                return NoContent();
            }
            else if (check == 2)
            {
                return BadRequest("Payment Fail");
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("user_request")]
        public async Task<IActionResult> SendBookingRequest(BookingInput bookinginput)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            int check = await _book.SendBookingRequest(bookinginput, userId);
            if (check == 1)
            {
                return Ok();
            }
            else if (check == 3)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("user_Booking_list")]
        public async Task<IActionResult> GetBooking()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var Booking = await _book.GetListBookingInfo(userId);
            var returnBooking = _mapper.Map<IEnumerable<BookingInfoOutput>>(Booking);
            return Ok(returnBooking);
        }
    }
}
