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
        private readonly fptdimsContext _context;
        private readonly IMapper _mapper;
        private readonly IOtherService _other;
        private readonly IMail _mail;
        private readonly IFireBaseService _firebase;
        private string purpose1 = "ACTIVE ACCOUNT";

        public UserManageRepository(IFireBaseService firebase, IMail mail, fptdimsContext context, IMapper mapper, IOtherService other)
        {
            _context = context;
            _mapper = mapper;
            _other = other;
            _mail = mail;
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
            return "2";
        }

        public async Task<int> GetActiveCodeMailSend(int userId)
        {
            var otpCode = await _context.Otps.Include(u => u.User)
                .Where(a => a.Purpose.Equals(purpose1) && userId == a.UserId).SingleOrDefaultAsync();
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
        public async Task<string> HostGetActiveCodeMailSend(int userId)
        {
            var otpCode = await _context.Otps.Include(u => u.User)
               .Where(a => a.Purpose.Equals("ACTIVE ACCOUNT") && userId == a.UserId).SingleOrDefaultAsync();
            otpCode.CodeOtp = _other.RandomString(6);

            if (await _context.SaveChangesAsync() > 0)
            {
                try
                {
                    await _mail.SendEmailAsync(otpCode.User.Email, otpCode.CodeOtp);
                }
                catch (Exception ex)
                {
                    return "2";
                }
            }
            return "1";
        }

        public async Task<string> HostActiveAccount(string activeCode, int userId)
        {
            var user = await _context.Users
                 .Where(a => userId == a.UserId).SingleOrDefaultAsync();
            var otpCode = await _context.Otps
                .Where(a => a.Purpose.Equals("ACTIVE ACCOUNT") && userId == a.UserId).SingleOrDefaultAsync();
            if (otpCode == null)
                return "Already Active";
            if (otpCode.CodeOtp.Trim().Equals(activeCode.Trim()))
            {
                otpCode.CodeOtp = null;
                user.Role = "HOST";
            }
            if (await _context.SaveChangesAsync() > 0)
                return "1";
            return "2";
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
                var lsPr = await _context.Provinces
                    .Where(p => p.ProvinceNoMark.Contains(terms))
                    .ToListAsync();
                var lsdis = await _context.Districts
                    .Where(p => p.DistrictNoMark.Contains(terms))
                    .ToListAsync();
                var lsProvince = new List<SearchLocationAreaOutput>();
                lsProvince.AddRange(_mapper.Map<IEnumerable<SearchLocationAreaOutput>>(lsdis));
                lsProvince.AddRange(_mapper.Map<IEnumerable<SearchLocationAreaOutput>>(lsPr));

                var lsHo = await _context.Hotels
                    .Where(op => op.Status == true
                    && op.DistrictNavigation.DistrictNoMark.Contains(terms)
                    || op.ProvinceNavigation.ProvinceNoMark.Contains(terms))
                    .ToListAsync();
                var lsHotel = new List<SearchLocationHotelOutput>();
                lsHotel.AddRange(_mapper.Map<IEnumerable<SearchLocationHotelOutput>>(lsHo));

                returnLocation.Areas = lsProvince;
                returnLocation.Hotels = lsHotel;
                return returnLocation;
            }
            return null;
        }

        public async Task<IEnumerable<HotelOutput>> GetListSearchHotel(string Location, string LocationName, DateTime ArrivalDate, int TotalNight)
        {
            DateTime StartDate = ArrivalDate;
            DateTime EndDate = _other.GetEndDate(ArrivalDate, TotalNight);
            var terms = _other.RemoveMark(LocationName);

            if (ArrivalDate.Date < DateTime.Now.Date || TotalNight <= 0) { return null; }
            IQueryable<Hotel> hotels = _context.Hotels
                .Include(p => p.Photos)
                .Include(p => p.Categories)
                .Include(h => h.HotelType)
                .Include(w => w.WardNavigation)
                .Include(d => d.DistrictNavigation)
                .Include(pr => pr.ProvinceNavigation)
                .Include(r => r.Rooms.Where(bd => bd.BookingDetails.Where(op => op.Status.Value).All(op => (
                                                  !(((op.StartDate.Value.Date > StartDate.Date && op.StartDate.Value.Date < EndDate.Date)
                                                  && (op.EndDate.Value.Date > StartDate.Date && op.EndDate.Value.Date < EndDate.Date))
                                                  || (op.StartDate.Value.Date < StartDate.Date && op.EndDate.Value.Date > EndDate.Date)
                                                  || (op.StartDate.Value.Date < EndDate.Date && op.EndDate.Value.Date > EndDate.Date)
                                                  || (op.StartDate.Value.Date < StartDate.Date && op.EndDate.Value.Date > StartDate.Date)
                                                  || (op.StartDate.Value.Date == StartDate.Date
                                                  || op.StartDate.Value.Date == EndDate.Date
                                                  || op.EndDate.Value.Date == StartDate.Date
                                                  || op.EndDate.Value.Date == EndDate.Date)
                                                  ))
                                                )))
                .Where(op => op.Rooms.Count() > 0);

            if (Location.ToLower().Trim() == "areas")
            {
                hotels = hotels.Where(op => op.ProvinceNavigation.ProvinceNoMark.Contains(terms)
                                                    || op.DistrictNavigation.DistrictNoMark.Contains(terms)
                                                    && op.Status == true);
            }
            else
            {
                hotels = hotels.Where(op => op.HotelNameNoMark.Contains(terms) && op.Status == true);
            }

            await hotels.ToListAsync();
            var returnls = _mapper.Map<IEnumerable<HotelOutput>>(hotels.OrderByDescending(r => r.TotalRate));

            return returnls;
        }

        public async Task<HotelCateInfoOutput> GetListAvaiableHotelCate(int? hotelId, DateTime ArrivalDate, int TotalNight, int peopleQuanity)
        {
            DateTime StartDate = ArrivalDate;
            DateTime EndDate = _other.GetEndDate(ArrivalDate, TotalNight);
            if (ArrivalDate.Date < DateTime.Now.Date || TotalNight <= 0 || peopleQuanity <= 0) { return null; }

            IQueryable<Category> lsCateRooms = _context.Categories
                .Include(p => p.Photos)
                .Include(pr => pr.SpecialPrices.Where(op => op.SpecialDate.Value.Date >= DateTime.Now.Date))
                .Include(r => r.Rooms)
                .Where(op => op.HotelId == hotelId && op.Quanity >= peopleQuanity && op.Status == true);

            if (lsCateRooms == null) { return null; }

            var AHotel = await _context.Hotels
                .Include(p => p.Vouchers.Where(op => op.EndDate.Value.Date >= DateTime.Now.Date))
                .Include(p => p.Photos)
                .Include(h => h.HotelType)
                .Include(c => c.Categories)
                .Include(r => r.Rooms.Where(op => op.Status == true)
                                     .Where(a => a.BookingDetails.Where(op => op.Status.Value).All(op => (
                                                    !(((op.StartDate.Value.Date >= StartDate.Date && op.StartDate.Value.Date <= EndDate.Date)
                                                    && (op.EndDate.Value.Date >= StartDate.Date && op.EndDate.Value.Date <= EndDate.Date))
                                                    || (op.StartDate.Value.Date <= StartDate.Date && op.EndDate.Value.Date >= EndDate.Date)
                                                    || (op.StartDate.Value.Date <= EndDate.Date && op.EndDate.Value.Date >= EndDate.Date)
                                                    || (op.StartDate.Value.Date <= StartDate.Date && op.EndDate.Value.Date >= StartDate.Date)
                                                    ))
                                                )))
                .Include(w => w.WardNavigation)
                .Include(d => d.DistrictNavigation)
                .Include(pr => pr.ProvinceNavigation)
                .Where(op => op.HotelId == hotelId )
                .SingleOrDefaultAsync();

            var result = await lsCateRooms.ToListAsync();

            var HotelDetail = new HotelCateInfoOutput();
            _mapper.Map(AHotel, HotelDetail);

            var fullCateRoom = new List<HotelCateOutput>();
            _mapper.Map(result, fullCateRoom);
            HotelDetail.LsCate = fullCateRoom;

            return HotelDetail;

        }

        public async Task<IEnumerable<District>> ListAllDistrict()
        {
            var ls = await _context.Districts.ToListAsync();
            return ls;
        }

        public async Task<int> userFeedback(int userId, int BookingId, FeedBackInput fb)
        {
            if (fb.Rating < 0 || fb.Rating > 10)
            {
                return 2;
            }
            var newFb = new Feedback();
            newFb.BookingId = BookingId;
            newFb.Comment = fb.Comment;
            newFb.Rating = fb.Rating;
            newFb.Status = true;

            await _context.Feedbacks.AddAsync(newFb);
            if (await _context.SaveChangesAsync() > 0)
            {
                var bok = await _context.Bookings
                    .Where(op => op.BookingId == BookingId)
                    .FirstOrDefaultAsync();
                if (bok.EndDate >= DateTime.Now)
                {
                    var hotel = await _context.Hotels
                        .Include(b => b.Bookings).ThenInclude(f => f.Feedback)
                        .Where(op => op.HotelId == bok.HotelId)
                        .FirstOrDefaultAsync();
                    hotel.TotalRate = (int?)hotel.Bookings.Average(a => a.Feedback.Rating);
                    if (await _context.SaveChangesAsync() > 0)
                        return 1;
                    return 3;
                }
            }
            return 0;
        }

        public async Task<int> userUpdateFeedback(int userId, int feedbackId, FeedBackInput fb)
        {
            if (fb.Rating < 0 || fb.Rating > 10)
            {
                return 2;
            }
            var feedback = await _context.Feedbacks
                .Where(op => op.FeedbackId == feedbackId).SingleOrDefaultAsync();
            if (feedback != null)
            {
                feedback.Comment = fb.Comment;
                feedback.Rating = fb.Rating;
                if (await _context.SaveChangesAsync() > 0)
                {
                    var f = await _context.Feedbacks
                        .Include(b => b.Booking)
                        .Where(op => op.FeedbackId == feedbackId)
                        .FirstOrDefaultAsync();

                    var hotel = await _context.Hotels
                        .Include(b => b.Bookings).ThenInclude(f => f.Feedback)
                        .Where(op => op.HotelId == f.Booking.HotelId)
                        .FirstOrDefaultAsync();

                    hotel.TotalRate = (int?)hotel.Bookings.Average(a => a.Feedback.Rating);
                    if (await _context.SaveChangesAsync() > 0)
                        return 1;
                    return 3;
                }
            }
            return 0;
        }

        public async Task<int> CreateNoMarkColumCHEAT()
        {
            var a = await _context.Hotels.ToListAsync();
            var b = await _context.Rooms
                .Include(a => a.Category)
                .ToListAsync();

            foreach (var bb in b)
            {
                bb.RoomPrice = bb.Category.PriceDefault;
            }
            foreach (var aa in a)
            {
                aa.HotelNameNoMark = _other.RemoveMark(aa.HotelName);
            }

            if (await _context.SaveChangesAsync() > 0)
                return 3;
            return 0;
        }

        
        
    }
}