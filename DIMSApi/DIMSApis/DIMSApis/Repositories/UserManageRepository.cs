using AutoMapper;
using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Text;

namespace DIMSApis.Repositories
{
    public class UserManageRepository : IUserManage
    {
        private readonly DIMSContext _context;
        private readonly IMapper _mapper;
        private readonly IOtherService _other;
        private readonly IMail _mail;
        private readonly IFireBaseService _firebase;
        private string purpose1 = "ACTIVE ACCOUNT";

        public UserManageRepository(IFireBaseService firebase, IMail mail, DIMSContext context, IMapper mapper, IOtherService other)
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
                var lsdis = await _context.Districts.ToListAsync();
                var lsProvince = new List<SearchLocationAreaOutput>();
                foreach (var item in lsPr)
                {
                    if (_other.RemoveMark(item.Name).Contains(terms))
                    {
                        lsProvince.Add(_mapper.Map<SearchLocationAreaOutput>(item));
                    }
                }
                foreach (var item in lsdis)
                {
                    if (_other.RemoveMark(item.Name).Contains(terms))
                    {
                        lsProvince.Add(
                            new SearchLocationAreaOutput
                            {
                                Id = item.Id,
                                Name = item.Name,
                                Type = item.Type,
                            });
                    }
                }
                var lsHo = await _context.Hotels.Where(op => op.Status == 1).ToListAsync();
                var lsHotel = new List<SearchLocationHotelOutput>();
                foreach (var item in lsHo)
                {
                    if (_other.RemoveMark(item.HotelName).Contains(terms)
                        || _other.RemoveMark(item.DistrictNavigation.Name).Contains(terms)
                        || _other.RemoveMark(item.ProvinceNavigation.Name).Contains(terms))
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
        public async Task<IEnumerable<HotelOutputNews>> GetListSearchHotelNews(string Location, string LocationName, DateTime ArrivalDate, int TotalNight)
        {
            var terms = _other.RemoveMark(LocationName);

            var searchhotel = new List<HotelOutputNews>();
            DateTime StartDate = ArrivalDate;
            DateTime EndDate = _other.GetEndDate(ArrivalDate, TotalNight);
            if (TotalNight > 0)
            {
                if (Location.ToLower().Trim() == "areas")
                {
                    try
                    {
                        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                        builder.DataSource = "fptdims.database.windows.net";
                        builder.UserID = "handez";
                        builder.Password = "tuananh@123";
                        builder.InitialCatalog = "fptdims";
                        var queryWithForJson = "Select DISTINCT h.* , MIN(r.Price) as Price from Hotels h, Rooms r , Bookings b , BookingDetails bd , Provinces p , Districts d where h.HotelID = r.HotelID and r.RoomID not in (Select r.RoomID  from Bookings b , BookingDetails bd , Rooms r, Hotels h where  ((b.StartDate < @StartDate and b.EndDate >  @EndDate)or ( b.StartDate BETWEEN @StartDate and @EndDate or b.EndDate BETWEEN @StartDate and @EndDate))and b.BookingID = bd.BookingID and bd.RoomID = r.RoomID and r.HotelID = h.HotelID )and h.Province in (select p.id from Provinces p where dbo.ufn_removeMark(p.[Name]) LIKE @hello ) GROUP BY  h.HotelName , h.Province , h.HotelID, h.HotelAddress,h.TotalRate , h.UserID , h.Ward ,h.District , h.CreateDate , h.Status , h.HotelNameNoMark ";
                        using (var conn = new SqlConnection(builder.ConnectionString))
                        {
                            using (var cmd = new SqlCommand(queryWithForJson, conn))
                            {
                                conn.Open();
                                var jsonResult = new StringBuilder();
                                cmd.Parameters.AddWithValue("@StartDate", "2022-06-13");
                                cmd.Parameters.AddWithValue("@EndDate", "2022-06-14");
                                cmd.Parameters.AddWithValue("@hello", "%" + "ho chi" + "%");
                                var reader = cmd.ExecuteReader();
                                if (!reader.HasRows)
                                {
                                    jsonResult.Append("[]");
                                }
                                else
                                {
                                    while (reader.Read())
                                    {
                                        var HotelID = reader.GetInt32(0);
                                        var HotelName = reader.GetString(1);
                                        
                                        var HotelAddress = reader.GetString(3);
                                        var UserID = reader.GetInt32(4);
                                        var District = reader.GetString(6);
                                        var Province = reader.GetString(7);
                                        var CreateDate = reader.GetDateTime(8);
                                        var Status = reader.GetInt32(9);
                                        var TotalRate = reader.GetInt32(10);
                                        var Price = reader.GetDouble(11);
                                        HotelOutputNews hotel = new HotelOutputNews();
                                        hotel.HotelId = HotelID;
                                        hotel.HotelAddress = HotelAddress;
                                        hotel.HotelName = HotelName;
                                        hotel.Province = Province;
                                        hotel.District = District;
                                        hotel.CreateDate = CreateDate;
                                        hotel.SmallPrice = Price;
                                        hotel.Status = Status;
                                        hotel.TotalRate = TotalRate;
                                        hotel.UserId = UserID;
                                        searchhotel.Add(hotel);
                                    }
                                }
                            }
                        }
                    }
                    catch (SqlException e)
                    {

                    }
                   }
            }
            return (IEnumerable<HotelOutputNews>)searchhotel;
        }
        public async Task<IEnumerable<HotelOutput>> GetListSearchHotel(string Location, string LocationName, DateTime ArrivalDate, int TotalNight)
        {
            var terms = _other.RemoveMark(LocationName);

            var searchhotel = new List<Hotel>();
            DateTime StartDate = ArrivalDate;
            DateTime EndDate = _other.GetEndDate(ArrivalDate, TotalNight);
            if (TotalNight > 0)
            { 
                if (Location.ToLower().Trim() == "areas")
                {
                    var lsHotel = await _context.Hotels
                        .Include(p => p.Photos)
                        .Include(h => h.Rooms)
                        .Include(w => w.WardNavigation)
                        .Include(d => d.DistrictNavigation)
                        .Include(pr => pr.ProvinceNavigation)
                        .Where(op => op.Status == 1)
                        .Where(op => op.DistrictNavigation.DistrictNoMark.Contains(terms) || op.ProvinceNavigation.ProvinceNoMark.Contains(terms))
                        .ToListAsync();
                    foreach (var hotel in lsHotel)
                    {
                        if (StartDate != null && EndDate != null)
                        {
                            var lsHotelRoom = await _context.BookingDetails
                               .Include(b => b.Booking)
                               .Where(op => op.Status == 1 && op.Booking.HotelId == hotel.HotelId)
                               .Where(op => ((op.StartDate > StartDate && op.StartDate < EndDate) && (op.EndDate > StartDate && op.EndDate < EndDate))
                                         || (op.StartDate < EndDate && op.EndDate > EndDate)
                                        || (op.StartDate < StartDate && op.EndDate > StartDate))
                               .ToListAsync();

                            var lsRoom = await _context.Rooms.WhereBulkNotContains(lsHotelRoom, a => a.RoomId).Where(op => op.HotelId == hotel.HotelId).ToListAsync();
                            var count = lsRoom.Count();
                            if (count > 0)
                            {
                                searchhotel.Add(hotel);
                            }
                        }
                    }
                }
                else
                {
                    var lsHotel = await _context.Hotels
                        .Include(p => p.Photos)
                        .Include(h => h.Rooms)
                        .Include(w => w.WardNavigation)
                        .Include(d => d.DistrictNavigation)
                        .Include(pr => pr.ProvinceNavigation)
                        .Where(op => op.Status == 1)
                        .Where(op => op.HotelNameNoMark.Contains(terms))
                        .ToListAsync();
                    foreach (var hotel in lsHotel)
                    {
                        if (StartDate != null && EndDate != null)
                        {
                            var lsHotelRoom = await _context.BookingDetails
                               .Include(b => b.Booking)
                               .Where(op => op.Status == 1 && op.Booking.HotelId == hotel.HotelId)
                               .Where(op => ((op.StartDate > StartDate && op.StartDate < EndDate) && (op.EndDate > StartDate && op.EndDate < EndDate))
                                         || (op.StartDate < EndDate && op.EndDate > EndDate)
                                        || (op.StartDate < StartDate && op.EndDate > StartDate))
                               .ToListAsync();

                            var lsRoom = await _context.Rooms.WhereBulkNotContains(lsHotelRoom, a => a.RoomId).Where(op => op.HotelId == hotel.HotelId).ToListAsync();
                            var count = lsRoom.Count();
                            if (count > 0)
                            {
                                searchhotel.Add(hotel);
                            }
                        }
                    }
                }
                return _mapper.Map<IEnumerable<HotelOutput>>(searchhotel.OrderByDescending(r => r.TotalRate));
            }
            return (IEnumerable<HotelOutput>)searchhotel;
        }

            public async Task<HotelCateInfoOutput> GetListAvaiableHotelCate(int? hotelId, DateTime ArrivalDate, int TotalNight, int peopleQuanity)
            {
                DateTime start = ArrivalDate;
                DateTime end = _other.GetEndDate(ArrivalDate, TotalNight);
                if (hotelId == null || start == null || end == null || peopleQuanity == null) { return null; }
                var lsHotelRoom = await _context.BookingDetails
                    .Include(b => b.Booking)
                    .Include(c => c.Room).ThenInclude(b => b.Category)
                   .Where(op => op.Status == 1 && op.Booking.HotelId == hotelId)
                   .Where(op => ((op.StartDate > start && op.StartDate < end) && (op.EndDate > start && op.EndDate < end))
                             || (op.StartDate < end && op.EndDate > end)
                            || (op.StartDate < start && op.EndDate > start))
                   .ToListAsync();

                var lsRoom = await _context.Rooms
                    .Include(c => c.Category).ThenInclude(b => b.Photos)
                    .Where(op => op.HotelId == hotelId && op.Category.Quanity >= peopleQuanity)
                    .WhereBulkNotContains(lsHotelRoom, a => a.RoomId).ToListAsync();

                var AHotel = await _context.Hotels
                    .Include(p => p.Photos)
                    .Include(c => c.Categories)
                    .Include(r => r.Rooms)
                    .Include(w => w.WardNavigation)
                    .Include(d => d.DistrictNavigation)
                    .Include(pr => pr.ProvinceNavigation)
                    .Where(op => op.Status == 1 && op.HotelId == hotelId)
                    .SingleOrDefaultAsync();

                var result = lsRoom
                .GroupBy(item => new
                {
                    item.CategoryId,
                    item.HotelId,
                    item.Category.CategoryName,
                    item.Category.CateDescrpittion,
                    item.Category.Quanity,
                    item.Category.Status,
                })
                .Select(gr => new HotelCateOutput
                {
                    CategoryId = (int)gr.Key.CategoryId,
                    HotelId = gr.Key.HotelId,
                    CategoryName = gr.Key.CategoryName,
                    CateDescrpittion = gr.Key.CateDescrpittion,
                    Quanity = gr.Key.Quanity,
                    CateStatus = gr.Key.Status,

                    Rooms = gr.Select(op => new HotelCateRoomOutput
                    {
                        CategoryId = op.CategoryId,
                        RoomId = op.RoomId,
                        RoomName = op.RoomName,
                        RoomDescription = op.RoomDescription,
                        Price = op.Price,
                        Status = op.Status,
                    }).ToList(),
                }).ToList();

                var HotelDetail = new HotelCateInfoOutput();
                _mapper.Map(AHotel, HotelDetail);

                var catephoto = await _context.Categories
                    .Include(p => p.Photos)
                    .Where(op => op.HotelId == hotelId)
                    .ToListAsync();
                _mapper.Map(catephoto, result);
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
                newFb.UserId = userId;
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
                var b = await _context.Provinces.ToListAsync();
                var c = await _context.Wards.ToListAsync();
                var d = await _context.Districts.ToListAsync();

                foreach (var aa in a)
                {
                    aa.HotelNameNoMark = _other.RemoveMark(aa.HotelName);
                }
                foreach (var bb in b)
                {
                    bb.ProvinceNoMark = _other.RemoveMark(bb.Name);
                }
                foreach (var cc in c)
                {
                    cc.WardNoMark = _other.RemoveMark(cc.Name);
                }
                foreach (var dd in d)
                {
                    dd.DistrictNoMark = _other.RemoveMark(dd.Name);
                }

                if (await _context.SaveChangesAsync() > 0)
                    return 3;
                return 0;
            }
        }
    }