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
        private readonly IOtherService _other;
        private readonly IMail _mail;
        private readonly IMailQrService _mailQrService;
        private readonly IFireBaseService _firebase;
        private string purpose1 = "ACTIVE ACCOUNT";

        public UserManageRepository(IFireBaseService firebase,IMailQrService mailQrService, IMail mail, DIMSContext context, IMapper mapper, IOtherService other)
        {
            _context = context;
            _mapper = mapper;
            _other = other;
            _mail = mail;
            _mailQrService = mailQrService;
            _firebase = firebase;
        }

        public async Task<User> GetUserDetail(int userId)
        {
            var userinfo = await _context.Users
                .Where(a => a.UserId == userId && a.Role != "ADMIN").FirstOrDefaultAsync();
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
            var terms = _other.RemoveMark(searchadress);

            var lsHotel = await _context.Hotels
                .Include(p => p.Photos)
                .Include(h => h.Rooms)
                .Include(w => w.WardNavigation)
                .Include(d => d.DistrictNavigation)
                .Include(pr => pr.ProvinceNavigation)
                .Where(op => op.Status == 1)
                .ToListAsync();
            if (terms == "")
            {
                return _mapper.Map<IEnumerable<HotelOutput>>(lsHotel);
            }
            else
            {
                var searchhotel = new List<Hotel>();
                foreach (var hotel in lsHotel)
                {
                    if (_other.RemoveMark(hotel.HotelAddress).Contains(terms))
                    {
                        if (start != null && end != null)
                        {
                            var lsHotelRoom = await _context.BookingDetails
                               .Include(b => b.Booking)
                               .Where(op => op.Status == 1 && op.Booking.HotelId == hotel.HotelId)
                               .Where(op => ((op.StartDate > start && op.StartDate < end) && (op.EndDate > start && op.EndDate < end))
                                         || (op.StartDate < end && op.EndDate > end)
                                        || (op.StartDate < start && op.EndDate > start))
                               .ToListAsync();

                            var lsRoom = _context.Rooms.WhereBulkNotContains(lsHotelRoom, a => a.RoomId).Where(op => op.HotelId == hotel.HotelId).Count();
                            if (lsRoom > 0)
                            {
                                searchhotel.Add(hotel);
                            }
                        }
                        else
                        {
                            searchhotel.Add(hotel);
                        }
                    }
                }

                var returnHotel = _mapper.Map<IEnumerable<HotelOutput>>(searchhotel);
                return returnHotel;
            }
        }

        public async Task<IEnumerable<HotelRoomOutput>> GetListAvaiableHotelRoom(int? hotelId, DateTime? start, DateTime? end)
        {
            if (hotelId == null || start == null || end == null) { return null; }
            var lsHotelRoom = await _context.BookingDetails
                .Include(b => b.Booking)
               .Where(op => op.Status == 1 && op.Booking.HotelId == hotelId)
               .Where(op => ((op.StartDate > start && op.StartDate < end) && (op.EndDate > start && op.EndDate < end))
                         || (op.StartDate < end && op.EndDate > end)
                        || (op.StartDate < start && op.EndDate > start))
               .ToListAsync();

            var lsRoom = _context.Rooms
                .Include(c => c.Category).ThenInclude(b => b.Photos)
                .Where(op => op.HotelId == hotelId)
                .WhereBulkNotContains(lsHotelRoom, a => a.RoomId);

            var returnHotelRoom = _mapper.Map<IEnumerable<HotelRoomOutput>>(lsRoom);

            return returnHotelRoom;
        }

        public async Task<IEnumerable<HotelCateOutput>> GetListAvaiableHotelCate(int? hotelId, DateTime? start, DateTime? end , int peopleQuanity)
        {
            if (hotelId == null || start == null || end == null || peopleQuanity == null) { return null; }
            var lsHotelRoom = await _context.BookingDetails
                .Include(b => b.Booking)
                .Include(c => c.Room).ThenInclude(b => b.Category)
               .Where(op => op.Status == 1 && op.Booking.HotelId == hotelId)
               .Where(op => ((op.StartDate > start && op.StartDate < end) && (op.EndDate > start && op.EndDate < end))
                         || (op.StartDate < end && op.EndDate > end)
                        || (op.StartDate < start && op.EndDate > start))
               .ToListAsync();

            var lsRoom = _context.Rooms
                .Include(c => c.Category).ThenInclude(b => b.Photos)
                .Where(op => op.HotelId == hotelId && op.Category.Quanity >= peopleQuanity)
                .WhereBulkNotContains(lsHotelRoom, a => a.RoomId);

            var returnHotelRoom = _mapper.Map<IEnumerable<HotelCateOutput>>(lsRoom);
            var result = returnHotelRoom
                   .GroupBy(item => new
                   {
                       item.CategoryId,
                   })
                   .Select(group => group.FirstOrDefault())
                   .ToList().OrderBy(i => i.CategoryId);
            return result;
        }

        public async Task<string> ActiveAccount(string activeCode, int userId)
        {
            var user = await _context.Users
                .Where(a => userId == a.UserId).SingleOrDefaultAsync();
            var otpCode = await _context.Otps
                .Where(a => a.Purpose.Equals(purpose1) && userId == a.UserId).SingleOrDefaultAsync();
            if (otpCode == null)
                return "Already Active";
            if (otpCode.CodeOtp.Trim().Equals(activeCode.Trim()))
            {
                otpCode.CodeOtp = null;
                user.Role = "USER";
            }
            if (await _context.SaveChangesAsync() > 0)
                return "1";
            return "0";
        }

        public async Task<int> GetActiveCodeMailSend(int userId)
        {
            if (userId == null)
                return 0;
            var otpCode = await _context.Otps.Include(u => u.User)
                .Where(a => a.Purpose.Equals(purpose1) && userId == a.UserId).SingleOrDefaultAsync();
            if (otpCode == null)
                return 0;
            otpCode.CodeOtp = _other.RandomString(6);
            if (await _context.SaveChangesAsync() > 0)
            {
                try
                {
                    await _mail.SendEmailAsync(otpCode.User.Email, otpCode.CodeOtp);
                }
                catch (Exception ex)
                {
                    return 2;
                }
            }
            return 1;
        }

        public async Task<IEnumerable<Province>> SearchProvince(string province)
        {
            var terms = _other.RemoveMark(province);
            if (terms != null)
            {
                var ls = await _context.Provinces.ToListAsync();
                var lsProvince = new List<Province>();
                foreach (var item in ls)
                {
                    if (_other.RemoveMark(item.Name).Contains(terms))
                    {
                        lsProvince.Add(item);
                    }
                }
                return lsProvince;
            }
            return null;
        }

        public async Task<IEnumerable<Ward>> SearchWard(string ward)
        {
            var terms = _other.RemoveMark(ward);
            if (terms != null)
            {
                var ls = await _context.Wards.ToListAsync();
                var lsWard = new List<Ward>();
                foreach (var item in ls)
                {
                    if (_other.RemoveMark(item.Name).Contains(terms))
                    {
                        lsWard.Add(item);
                    }
                }
                return lsWard;
            }
            return null;
        }

        public async Task<IEnumerable<District>> SearchDistrict(string district)
        {
            var terms = _other.RemoveMark(district);
            if (terms != null)
            {
                var ls = await _context.Districts.ToListAsync();
                var lsDistrict = new List<District>();
                foreach (var item in ls)
                {
                    if (_other.RemoveMark(item.Name).Contains(terms))
                    {
                        lsDistrict.Add(item);
                    }
                }
                return lsDistrict;
            }
            return null;
        }

        public async Task<SearchLocationOutput> SearchLocation(string LocationName)
        {
            var terms = _other.RemoveMark(LocationName);
            if (terms != null)
            {
                var returnLocation = new SearchLocationOutput();
                var lsPr = await _context.Provinces.ToListAsync();
                var lsProvince = new List<SearchLocationAreaOutput>();
                foreach (var item in lsPr)
                {
                    if (_other.RemoveMark(item.Name).Contains(terms))
                    {
                        lsProvince.Add(_mapper.Map<SearchLocationAreaOutput>(item));
                    }
                }
                var lsHo = await _context.Hotels.Where(op => op.Status == 1).ToListAsync();
                var lsHotel = new List<SearchLocationHotelOutput>();
                foreach (var item in lsHo)
                {
                    if (_other.RemoveMark(item.HotelName).Contains(terms))
                    {
                        lsHotel.Add(_mapper.Map<SearchLocationHotelOutput>(item));
                    }
                }
                returnLocation.Areas = lsProvince;
                returnLocation.Hotels = lsHotel;
                return returnLocation;
            }
            return null;
        }

        public async Task<IEnumerable<HotelOutput>> GetListSearchHotel(SearchFilterInput sInp)
        {
            var terms = _other.RemoveMark(sInp.LocationName);
            var lsHotel = await _context.Hotels
                .Include(p => p.Photos)
                .Include(h => h.Rooms)
                .Include(w => w.WardNavigation)
                .Include(d => d.DistrictNavigation)
                .Include(pr => pr.ProvinceNavigation)
                .Where(op => op.Status == 1)
                .ToListAsync();
            var searchhotel = new List<Hotel>();

            foreach (var hotel in lsHotel)
            {
                if (_other.RemoveMark(hotel.HotelAddress).Contains(terms)
                    || _other.RemoveMark(hotel.ProvinceNavigation.Name).Contains(terms)
                    || _other.RemoveMark(hotel.HotelName).Contains(terms))
                {
                    if (sInp.StartDate != null && sInp.EndDate != null)
                    {
                        var lsHotelRoom = await _context.BookingDetails
                           .Include(b => b.Booking)
                           .Where(op => op.Status == 1 && op.Booking.HotelId == hotel.HotelId)
                           .Where(op => ((op.StartDate > sInp.StartDate && op.StartDate < sInp.EndDate) && (op.EndDate > sInp.StartDate && op.EndDate < sInp.EndDate))
                                     || (op.StartDate < sInp.EndDate && op.EndDate > sInp.EndDate)
                                    || (op.StartDate < sInp.StartDate && op.EndDate > sInp.StartDate))
                           .ToListAsync();

                        var lsRoom = _context.Rooms.WhereBulkNotContains(lsHotelRoom, a => a.RoomId).Where(op => op.HotelId == hotel.HotelId).Count();
                        if (lsRoom > 0)
                        {
                            searchhotel.Add(hotel);
                        }
                    }
                }
            }
            return _mapper.Map<IEnumerable<HotelOutput>>(searchhotel);
        }

        public async Task<bool> GetForgotCodeMailSend(ForgotCodeMailInput mail)
        {
            //var link = await _firebase.GetlinkImage("a");
            //if (_firebase.RemoveDirectories("a"))
            //{
            //    await _mailQrService.SendEmailAsync(mail.Email, link);
            //    return true;
            //}
            return false;
        }
    }
}