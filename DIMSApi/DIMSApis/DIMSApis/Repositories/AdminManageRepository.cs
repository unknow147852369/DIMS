using AutoMapper;
using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using DIMSApis.Models.Output;
using Microsoft.EntityFrameworkCore;

namespace DIMSApis.Repositories
{
    public class AdminManageRepository : IAdminManage
    {
        private readonly DIMSContext _context;
        private readonly IMapper _mapper;

        private string role1 = "HOST";

        public AdminManageRepository(DIMSContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> AcpectHost(int UserId)
        {
            var user = await _context.Users
                .Where(u => u.UserId == UserId).FirstOrDefaultAsync();
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
            var hotel = await _context.Hotels.Where(u => u.HotelId == hotelId ).FirstOrDefaultAsync();
            if (hotel != null)
            {
                hotel.Status = 1;
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
                .Where(r =>r.Role.Equals("HOST"))
                .ToListAsync();
            return User.OrderBy(a => a.Status);
        }

        public async Task<IEnumerable<HotelOutput>> ListAllHotel()
        {
            var hotel = await _context.Hotels
                .Include(u => u.User)
                .Include(p=>p.Photos)
                .Include(w=>w.WardNavigation)
                .Include(pr=>pr.ProvinceNavigation)
                .Include(d=>d.DistrictNavigation)
                .ToListAsync();
            return _mapper.Map<IEnumerable<HotelOutput>>(hotel).OrderBy(a=>a.Status);
        }
    }
}