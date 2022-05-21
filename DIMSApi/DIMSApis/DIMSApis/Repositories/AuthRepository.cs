using AutoMapper;
using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using Microsoft.EntityFrameworkCore;


namespace DIMSApis.Repositories
{
    public class AuthRepository : IAuth
    {
        private readonly DIMSContext _context;
        private readonly IOtherService _otherservice;
        private readonly IMail _mail;
        private readonly IMapper _mapper;

        public  AuthRepository(DIMSContext context, IOtherService otherservice, IMail mail, IMapper mapper)
        {
            _context = context;
            _otherservice = otherservice;
            _mail = mail;
            _mapper = mapper;
        }

        public async Task<bool> GetForgotCodeMailSend(ForgotCodeMailInput mail)
        {
            var user = await _context.Users
                .Include(x => x.Otps)
                .Where(u => u.Email == mail.Email.ToLower() && u.Status == true ).SingleOrDefaultAsync();
            if (user == null)
                return false;
            user.UnlockKey = _otherservice.RandomString(6);
            if (await _context.SaveChangesAsync() > 0)
            {
                await _mail.SendEmailAsync(user.Email, user.UnlockKey);
                return true;
            }
            return false;
        }


        public async Task<bool> UpdateNewPass(ForgotPassInput pass)
        {
            var user = await _context.Users.Where(u => u.Email == pass.Email.ToLower() && u.Status == true ).SingleOrDefaultAsync();
            if (user == null)
                return false;
            if (user.UnlockKey == pass.UnlockKey)
            {
                byte[] passwordHash, passwordSalt;
                _otherservice.CreatePasswordHash(pass.Password, out passwordHash, out passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.UnlockKey = null;
            }
            if (await _context.SaveChangesAsync() > 0)
                return true;
            return false;
        }

        public async Task<User> Login(LoginInput userinput)
        {
            var user = await _context.Users.Where(u => u.Email == userinput.Email.ToLower() && u.Status == true && u.Role != "ADMIN").SingleOrDefaultAsync();
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

        public async Task<bool> Register(RegisterInput userinput)
        {
            byte[] passwordHash, passwordSalt;
            _otherservice.CreatePasswordHash(userinput.Password, out passwordHash, out passwordSalt);
            var checkcode = _otherservice.RandomString(6);
            var ltOtp = new List<NewOtpInput>();
            ltOtp.Add(new NewOtpInput
            {
                Purpose = "ACTIVE ACCOUNT",
                CodeOtp =checkcode,
                CreateDate = DateTime.Now,
                Status = 1,
            });
            User user = new()
            {
                Email = userinput.Email.ToLower(),
                CreateDate = DateTime.Now,
                Gender = "UNKNOW",
                Role = "USER",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true,
            };

            _mapper.Map(ltOtp,user.Otps);
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

    }
}
