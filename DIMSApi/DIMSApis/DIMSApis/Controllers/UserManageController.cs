#nullable disable

using AutoMapper;
using DIMSApis.Interfaces;
using DIMSApis.Models.Helper;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
                return Ok(new DataRespone { Message = "Update Success" });
            }
            else if (check == 3)
            {
                return Ok(new DataRespone { Message = "nothing change" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "Update Fail" });
            }
        }

        [HttpGet("self-info")]
        public async Task<IActionResult> GetSelfUser()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = await _usermanage.GetUserDetail(userId);
            if (user == null)
                return NotFound(new DataRespone { Message = "Something Wrong" });
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
                return Ok(new DataRespone { Message = "Active success" });
            }
            else if (check.Equals("2"))
            {
                return BadRequest(new DataRespone { Message = "Wrong code" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = check });
            }
        }

        [HttpPut("Send-mail-active")]
        public async Task<IActionResult> GetActiveCodeMailSend()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            int check = await _usermanage.GetActiveCodeMailSend(userId);
            if (check == 1)
            {
                return Ok(new DataRespone { Message = "send Success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "send Fail" });
            }
        }

        [HttpPut("Host-Active-Account")]
        public async Task<IActionResult> HostActiveAccount(string AcitveCode)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            string check = await _usermanage.HostActiveAccount(AcitveCode, userId);
            if (check.Equals("1"))
            {
                return Ok(new DataRespone { Message = "Active success" });
            }
            else if (check.Equals("2"))
            {
                return BadRequest(new DataRespone { Message = "Wrong code" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = check });
            }
        }
        [HttpPut("Host-Send-mail-active")]
        public async Task<IActionResult> HostGetActiveCodeMailSend()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _usermanage.HostGetActiveCodeMailSend(userId);
            if (check.Equals("1"))
            {
                return Ok(new DataRespone { Message = "send Success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "send Fail" });
            }
        }

        //    [HttpPost("user-feedback")]
        //    public async Task<IActionResult> userFeedback(int BookingId ,FeedBackInput fb)
        //    {
        //        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        //        int check = await _usermanage.userFeedback(userId,BookingId,fb);
        //        if (check == 1)
        //        {
        //            return Ok("send Success");
        //        }
        //        else if (check == 0)
        //        {
        //            return NotFound("booking not end yet");
        //        }
        //        else
        //        {
        //            return BadRequest("some feild wrong!");
        //        }
        //    }

        //    [HttpPut("user-feedback")]
        //    public async Task<IActionResult> userUpdateFeedback(int feedbackId,FeedBackInput fb)
        //    {
        //        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        //        int check = await _usermanage.userUpdateFeedback(userId,feedbackId, fb);
        //        if (check == 1)
        //        {
        //            return Ok("update Success");
        //        }
        //        else if (check == 0)
        //        {
        //            return NotFound("booking not end yet");
        //        }
        //        else
        //        {
        //            return BadRequest("some feild wrong!");
        //        }
        //    }
    }
}