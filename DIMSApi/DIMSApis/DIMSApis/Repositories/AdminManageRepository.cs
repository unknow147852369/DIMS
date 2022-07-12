﻿using AutoMapper;
using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;
using Microsoft.EntityFrameworkCore;

namespace DIMSApis.Repositories
{
    public class AdminManageRepository : IAdminManage
    {
        private readonly fptdimsContext _context;
        private readonly IMapper _mapper;
        private readonly IOtherService _otherservice;
        private readonly IMail _mail;
        private string purpose1 = "ACTIVE ACCOUNT";
        private string purpose2 = "CHANGE PASS";
        private string role1 = "HOST";

        public AdminManageRepository(IMail mail, IOtherService otherservice, fptdimsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _otherservice = otherservice;
            _mail = mail;
        }

        public async Task<int> AcpectHost(int UserId)
        {
            var user = await _context.Users
                .Where(u => u.UserId == UserId && u.Status.Value)
                .FirstOrDefaultAsync();
            if (user != null)
            {
                user.Role = role1;
                if (await _context.SaveChangesAsync() > 0)
                    return 1;
                return 3;
            }
            return 0;
        }

        public async Task<int> AcpectHotel(int hotelId)
        {
            var hotel = await _context.Hotels
                .Where(u => u.HotelId == hotelId && u.Status.Value)
                .FirstOrDefaultAsync();
            if (hotel != null)
            {
                hotel.Status = true;
                if (await _context.SaveChangesAsync() > 0)
                    return 1;
                return 3;
            }
            return 0;
        }

        public async Task<IEnumerable<User>> ListAllHost()
        {
            var User = await _context.Users
                .Include(u => u.Hotels)
                .Where(r => r.Role.Equals("HOST"))
                .ToListAsync();
            return User.OrderBy(a => a.Status);
        }

        public async Task<IEnumerable<HotelOutput>> ListAllHotel()
        {
            var hotel = await _context.Hotels
                .Include(u => u.User)
                .Include(p => p.Photos)
                .Include(w => w.WardNavigation)
                .Include(pr => pr.ProvinceNavigation)
                .Include(d => d.DistrictNavigation)
                .ToListAsync();
            return _mapper.Map<IEnumerable<HotelOutput>>(hotel).OrderBy(a => a.Status);
        }

        public async Task<string> AdminCreateUser(AdminRegisterInput userinput)
        {
            byte[] passwordHash, passwordSalt;
            _otherservice.CreatePasswordHash(userinput.Password, out passwordHash, out passwordSalt);
            var ltOtp = new List<NewOtpInput>();
            ltOtp.Add(new NewOtpInput
            {
                Purpose = purpose1,
                CodeOtp = null,
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
                Role = userinput.role.Trim().ToUpper(),
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true,
            };

            _mapper.Map(ltOtp, user.Otps);
            await _context.Users.AddAsync(user);
            if (await _context.SaveChangesAsync() > 0)
                return "1";
            return "3";
        }
    }
}