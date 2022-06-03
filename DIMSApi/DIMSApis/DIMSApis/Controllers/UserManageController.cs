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
                return Ok("Update Success");
            }
            else if (check == 3)
            {
                return NoContent();
            }
            else
            {
                return BadRequest("Update Fail");
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

        [HttpGet("Active-Account")]
        public async Task<IActionResult> AcitveAccount(string AcitveCode)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            string check = await _usermanage.ActiveAccount(AcitveCode, userId);
            if (check.Equals("1"))
            {
                return Ok("Active success");
            }
            else
            {
                return BadRequest("Wrong code");
            }
        }

        [HttpPut("Send-mail-active")]
        public async Task<IActionResult> GetActiveCodeMailSend()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            int check = await _usermanage.GetActiveCodeMailSend(userId);
            if (check == 1)
            {
                return Ok("send Success");
            }
            else if (check == 0)
            {
                return NotFound("Empty fill");
            }
            else
            {
                return BadRequest("send Fail");
            }
        }
    }
}
