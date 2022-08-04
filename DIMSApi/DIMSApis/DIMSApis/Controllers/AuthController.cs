#nullable disable

using DIMSApis.Interfaces;
using DIMSApis.Models.Helper;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;
using Microsoft.AspNetCore.Mvc;

namespace DIMSApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _auth;
        private readonly ITokenService _tokenService;

        public AuthController(IAuth auth, ITokenService tokenService)
        {
            _auth = auth;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> UserRegister(RegisterInput user)
        {
            if (await _auth.UserExists(user.Email))
                return BadRequest(new DataRespone { Message = "Email already exists" });
            if (await _auth.UserRegister(user))
            {
                return Ok(new DataRespone { Message = "create success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "some thing went wrong" });
            }
        }

        [HttpPost("login-user")]
        public async Task<IActionResult> LoginUser(LoginInput userIn)
        {
            var user = await _auth.LoginUser(userIn);

            if (user == null)
                return Unauthorized(new DataRespone { Message = "wrong email or password" });

            LoginOutput login = new()
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            };
            return Ok(login);
        }

        [HttpPost("login-admin")]
        public async Task<IActionResult> LoginAdmin(LoginInput userIn)
        {
            var user = await _auth.LoginAdmin(userIn);

            if (user == null)
                return Unauthorized(new DataRespone { Message = "wrong email or password" });

            LoginOutput login = new()
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            };
            return Ok(login);
        }
        [HttpPost("login-Host")]
        public async Task<IActionResult> LoginHost(LoginInput userIn)
        {
            var user = await _auth.LoginHost(userIn);

            if (user == null)
                return Unauthorized(new DataRespone { Message = "wrong email or password" });

            LoginOutput login = new()
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            };
            return Ok(login);
        }
        [HttpPost("forgot-code-mail")]
        public async Task<IActionResult> ForgotCodeMailSend(ForgotCodeMailInput mail)
        {
            if (await _auth.GetForgotCodeMailSend(mail))
            {
                return Ok(new DataRespone { Message = "send mail success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "send mail fail" });
            }
        }

        [HttpPost("forgot-pass-change")]
        public async Task<IActionResult> ForgoPassChange(ForgotPassInput pass)
        {
            if (await _auth.UpdateNewPass(pass))
            {
                return Ok(new DataRespone { Message = "change success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "Nothing change" });
            }
        }

        [HttpPost("Register-hotel-manager-role")]
        public async Task<IActionResult> RegisterHotelManagerRole(RegisterInput user)
        {

            if (await _auth.HotelManagerExists(user.Email))
                return BadRequest(new DataRespone { Message = "Email already exists" });
            var check = await _auth.RegisterHotelManagerRole(user); 
            if (check.Equals("1"))
            {
                return Ok(new DataRespone { Message = "create success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "some thing went wrong" });
            }
        }

    }
}