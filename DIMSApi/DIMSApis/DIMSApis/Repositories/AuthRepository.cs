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

        public AuthRepository(DIMSContext context, IOtherService otherservice)
        {
            _context = context;
            _otherservice = otherservice;
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
