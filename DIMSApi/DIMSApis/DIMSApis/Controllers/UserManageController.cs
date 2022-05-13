#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DIMSApis.Models.Data;
using Microsoft.AspNetCore.Authorization;
using DIMSApis.Models.Input;
using DIMSApis.Interfaces;
using System.Security.Claims;
using AutoMapper;
using DIMSApis.Models.Output;

namespace DIMSApis.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserManageController : ControllerBase
    {
        private readonly IUserManage _usermanage;
        private readonly IMapper _mapper;
        public UserManageController(IUserManage usermanage, IMapper mapper)
        {
            _usermanage = usermanage;
            _mapper = mapper;
        }

        [HttpPut("update-info")]
        public async Task<IActionResult> UpdateUserInfo(UserUpdateInput user)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            int check = await _usermanage.UpdateUserInfo(userId, user);
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

        [HttpGet("self-info")]
        public async Task<IActionResult> GetSelfUser()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = await _usermanage.GetUserDetail(userId);
            if (user == null)
                return NotFound();
            var returnUser = _mapper.Map<UserInfoOutput>(user);
            return Ok(returnUser);
        }

        [HttpGet("Avaiable_Hotel")]
        public async Task<IActionResult> GetAllHotel()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var Hotel = await _usermanage.GetListAvaiableHotel();
            return Ok(Hotel);
        }

        //[HttpGet("Host_All_Hotel_Room")]
        //public async Task<IActionResult> GetAllHotelRoom(int hotelId)
        //{
        //    int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        //    var HotelRoom = await _host.GetListAllHotelRoom(hotelId, userId);
        //    return Ok(HotelRoom);
        //}
    }
}
