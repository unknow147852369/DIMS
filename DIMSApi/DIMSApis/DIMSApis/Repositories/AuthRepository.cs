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

        public  AuthRepository(DIMSContext context, IOtherService otherservice, IMail mail)
        {
            _context = context;
            _otherservice = otherservice;
            _mail = mail;
        }

        public async Task<bool> GetForgotCodeMailSend(ForgotCodeMailInput mail)
        {
            var user = await _context.Users.Where(u => u.Email == mail.Email.ToLower() && u.Status == true && u.Role != "ADMIN").SingleOrDefaultAsync();
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

        public async Task<bool> ForgotCodeVertify(ForgotCodeVertifyInput code)
        {
            var user = await _context.Users.Where(u => u.Email == code.Email.ToLower() && u.Status == true && u.Role != "ADMIN" ).SingleOrDefaultAsync();
            if (user == null)
                return false;
            user.UnlockKey = null;
            if (await _context.SaveChangesAsync() > 0)
                return true;
            return false;
        }

        public async Task<bool> UpdateNewPass(ForgotPassInput pass)
        {
            var user = await _context.Users.Where(u => u.Email == pass.Email.ToLower() && u.Status == true ).SingleOrDefaultAsync();
            if (user == null)
                return false;
            byte[] passwordHash, passwordSalt;
            _otherservice.CreatePasswordHash(pass.Password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
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
            User user = new()
            {
                Email = userinput.Email.ToLower(),
                CreateDate = DateTime.Now,
                Gender = "UNKNOW",
                Role = "USER",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true
            };
            //await _mail.SendEmailAsync(user.Email, user.UnlockKey);
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
