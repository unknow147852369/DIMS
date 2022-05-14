using AutoMapper;
using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;
using Microsoft.EntityFrameworkCore;

namespace DIMSApis.Repositories
{
    public class UserManageRepository : IUserManage
    {
        private readonly DIMSContext _context;
        private readonly IMapper _mapper;

        public UserManageRepository(DIMSContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<User> GetUserDetail(int userId)
        {
            var userinfo = await _context.Users.Where(a => a.UserId == userId && a.Role != "ADMIN").FirstOrDefaultAsync();
            return userinfo;
        }

        public async Task<int> UpdateUserInfo(int userId, UserUpdateInput userinput)
        {
            try
            {
                var userUpdate = await _context.Users.Where(a => a.UserId == userId && a.Role != "ADMIN").FirstOrDefaultAsync();

                _mapper.Map(userinput, userUpdate);
                if (await _context.SaveChangesAsync() > 0)
                    return 1;
                return 3;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<IEnumerable<HotelOutput>> GetListAvaiableHotel(string? searchadress, DateTime? start, DateTime? end)
        {
            var terms = searchadress.Split(' ');
           
            var lsHotel = await _context.Hotels
                .Include(p => p.Photos)
                .Include(h => h.Rooms)
                .Include(w => w.WardNavigation)
                .Include(d => d.DistrictNavigation)
                .Include(pr => pr.ProvinceNavigation)
                .Where(op => op.Status == 1)
                
                .ToListAsync();

            var results = lsHotel
               .Where(a => terms.Any(term => a.HotelAddress.Contains(searchadress)))
               .ToList();

            var returnHotel = _mapper.Map<IEnumerable<HotelOutput>>(lsHotel);
            return returnHotel;
        }

        public async Task<IEnumerable<HotelRoomOutput>> GetListAvaiableHotelRoom(int hotelId, DateTime start, DateTime end)
        {
            var lsHotelRoom = await _context.BookingDetails
                .Include(b => b.Booking)
               .Where(op => op.Status == 1 && op.Booking.HotelId == hotelId)
               .Where(op => ((op.StartDate > start && op.StartDate < end) && (op.EndDate > start && op.EndDate < end))
                         || (op.StartDate < end && op.EndDate > end)
                        || (op.StartDate < start && op.EndDate > start))
               .ToListAsync();

            var lsRoom = _context.Rooms.WhereBulkNotContains(lsHotelRoom, a => a.RoomId).Where(op => op.HotelId == hotelId);
            var returnHotelRoom = _mapper.Map<IEnumerable<HotelRoomOutput>>(lsRoom);

            return returnHotelRoom;
        }
    }
}