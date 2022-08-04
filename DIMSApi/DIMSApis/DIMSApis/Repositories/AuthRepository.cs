using AutoMapper;
using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using Microsoft.EntityFrameworkCore;

namespace DIMSApis.Repositories
{
    public class AuthRepository : IAuth
    {
        private readonly fptdimsContext _context;
        private readonly IOtherService _otherservice;
        private readonly IMail _mail;
        private readonly IMapper _mapper;
        private string purpose1 = "ACTIVE ACCOUNT";
        private string purpose2 = "CHANGE PASS";

        public AuthRepository(fptdimsContext context, IOtherService otherservice, IMail mail, IMapper mapper)
        {
            _context = context;
            _otherservice = otherservice;
            _mail = mail;
            _mapper = mapper;
        }

        public async Task<bool> GetForgotCodeMailSend(ForgotCodeMailInput mail)
        {
            var user = await _context.Users
                .Where(u => u.Email == mail.Email.ToLower() && u.Status == true).SingleOrDefaultAsync();
            var otpCode = await _context.Otps
                .Where(a => a.Purpose.Equals(purpose2) && user.UserId == a.UserId).SingleOrDefaultAsync();
            if (user == null || otpCode == null)
                return false;
            otpCode.CodeOtp = _otherservice.RandomString(6);
            if (await _context.SaveChangesAsync() > 0)
            {
                await _mail.SendEmailAsync(user.Email, otpCode.CodeOtp);
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateNewPass(ForgotPassInput pass)
        {
            var user = await _context.Users
                .Where(u => u.Email == pass.Email.ToLower() && u.Status == true).SingleOrDefaultAsync();
            var otpCode = await _context.Otps
                .Where(a => a.Purpose.Equals(purpose2) && user.UserId == a.UserId).SingleOrDefaultAsync();
            if (user == null || otpCode == null)
                return false;
            if (otpCode.CodeOtp.Trim().Equals(pass.UnlockKey.Trim()))
            {
                byte[] passwordHash, passwordSalt;
                _otherservice.CreatePasswordHash(pass.Password, out passwordHash, out passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                otpCode.CodeOtp = null;
            }
            if (await _context.SaveChangesAsync() > 0)
                return true;
            return false;
        }

        public async Task<User> LoginUser(LoginInput userinput)
        {
            var user = await _context.Users.Where(u => u.Email == userinput.Email.ToLower() && (u.Status == true && u.Role == "USER" || u.Role == "WAIT_USER")).SingleOrDefaultAsync();
            if (user == null)
                return null;

            if (!_otherservice.VerifyPasswordHash(userinput.Password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        public async Task<User> LoginAdmin(LoginInput userinput)
        {
            var user = await _context.Users.Where(u => u.Email == userinput.Email.ToLower() && u.Status == true && u.Role == "ADMIN").SingleOrDefaultAsync();
            if (user == null)
                return null;

            if (!_otherservice.VerifyPasswordHash(userinput.Password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        public async Task<User> LoginHost(LoginInput userinput)
        {
            var user = await _context.Users.Where(u => u.Email == userinput.Email.ToLower() && (u.Status == true && u.Role == "HOST" || u.Role == "WAIT_HOST")).SingleOrDefaultAsync();
            if (user == null)
                return null;

            if (!_otherservice.VerifyPasswordHash(userinput.Password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        public async Task<bool> UserRegister(RegisterInput userinput)
        {
            byte[] passwordHash, passwordSalt;
            _otherservice.CreatePasswordHash(userinput.Password, out passwordHash, out passwordSalt);
            var checkcode = _otherservice.RandomString(6);
            var ltOtp = new List<NewOtpInput>();
            ltOtp.Add(new NewOtpInput
            {
                Purpose = purpose1,
                CodeOtp = checkcode,
                CreateDate = DateTime.Now,
                Status = 1,
            });
            ltOtp.Add(new NewOtpInput
            {
                Purpose = purpose2,
                CodeOtp = null,
                CreateDate = DateTime.Now,
                Status = 1,
            });
            User user = new()
            {
                Email = userinput.Email.ToLower(),
                CreateDate = DateTime.Now,
                Gender = "UNKNOW",
                Role = "WAIT_USER",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true,
            };

            _mapper.Map(ltOtp, user.Otps);
            await _mail.SendEmailAsync(user.Email, checkcode);
            await _context.Users.AddAsync(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UserExists(string email)
        {
            if (await _context.Users.AnyAsync(x => x.Email == email))
                return true;
            return false;
        }

        public async Task<string> RegisterHotelManagerRole(RegisterInput userinput)
        {
            byte[] passwordHash, passwordSalt;
            _otherservice.CreatePasswordHash(userinput.Password, out passwordHash, out passwordSalt);
            var checkcode = _otherservice.RandomString(6);
            var ltOtp = new List<NewOtpInput>();
            ltOtp.Add(new NewOtpInput
            {
                Purpose = purpose1,
                CodeOtp = checkcode,
                CreateDate = DateTime.Now,
                Status = 1,
            });
            ltOtp.Add(new NewOtpInput
            {
                Purpose = purpose2,
                CodeOtp = null,
                CreateDate = DateTime.Now,
                Status = 1,
            });
            User user = new()
            {
                Email = userinput.Email.ToLower(),
                CreateDate = DateTime.Now,
                Gender = "UNKNOW",
                Role = "WAIT_HOST",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true,
            };

            _mapper.Map(ltOtp, user.Otps);
            await _mail.SendEmailAsync(user.Email, checkcode);
            await _context.Users.AddAsync(user);
            if (await _context.SaveChangesAsync() > 0)
                return "1";
            return "3";
        }

        public async Task<bool> HotelManagerExists(string email)
        {
            if (await _context.Users.AnyAsync(x => x.Email == email && x.Role == "HOST" || x.Role == "WAIT_HOST"))
                return true;
            return false;
        }
    }
}