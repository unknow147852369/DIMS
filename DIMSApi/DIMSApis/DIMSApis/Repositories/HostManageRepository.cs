using AutoMapper;
using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;
using Microsoft.EntityFrameworkCore;

namespace DIMSApis.Repositories
{
    public class HostManageRepository : IHostManage
    {
        private readonly fptdimsContext _context;
        private readonly IMapper _mapper;
        private readonly IOtherService _other;

        public HostManageRepository(fptdimsContext context, IMapper mapper, IOtherService other)
        {
            _context = context;
            _mapper = mapper;
            _other = other;
        }

        public async Task<IEnumerable<HotelOutput>> GetListAllHotel(int userId)
        {
            var lsHotel = await _context.Hotels
                .Include(p => p.Photos)
                .Include(ht => ht.HotelType)
                .Include(h => h.Rooms)
                .Include(w => w.WardNavigation)
                .Include(d => d.DistrictNavigation)
                .Include(pr => pr.ProvinceNavigation)
                .Where(op => op.UserId == userId).ToListAsync();
            var returnHotel = _mapper.Map<IEnumerable<HotelOutput>>(lsHotel);
            return returnHotel;
        }

        public async Task<HotelCateInfoOutput> GetAHotelAllInfo(int hotelId, int userId)
        {
            IQueryable<Category> lsCateRooms = _context.Categories
                .Include(p => p.Photos)
                .Include(pr => pr.SpecialPrices.Where(op => op.SpecialDate.Value.Date >= DateTime.Now.Date))
                .Include(r => r.Rooms)
                .Where(op => op.HotelId == hotelId && op.Hotel.UserId == userId);

            if (lsCateRooms == null) { return null; }

            var AHotel = await _context.Hotels
                .Include(p => p.Vouchers.Where(op => op.EndDate.Value.Date >= DateTime.Now.Date))
                .Include(p => p.Photos)
                .Include(h => h.HotelType)
                .Include(c => c.Categories)
                .Include(r => r.Rooms)
                .Include(w => w.WardNavigation)
                .Include(d => d.DistrictNavigation)
                .Include(pr => pr.ProvinceNavigation)
                .Where(op => op.Status == true && op.HotelId == hotelId && op.UserId == userId)
                .SingleOrDefaultAsync();

            var result = await lsCateRooms.ToListAsync();

            var HotelDetail = new HotelCateInfoOutput();
            _mapper.Map(AHotel, HotelDetail);

            var fullCateRoom = new List<HotelCateOutput>();
            _mapper.Map(result, fullCateRoom);
            HotelDetail.LsCate = fullCateRoom;

            return HotelDetail;
        }

        public async Task<IEnumerable<HotelPhotosOutput>> GetListHotelPhotos(int userId, int hotelId)
        {
            var hotelPhotos = await _context.Photos
                .Where(op => op.HotelId == hotelId && op.Status == true).ToListAsync();
            return _mapper.Map<IEnumerable<HotelPhotosOutput>>(hotelPhotos);
        }

        public async Task<string> UpdateHotelMainPhoto(int photoID, int hotelID)
        {
            var hotelPhotos = await _context.Photos
               .Where(op => op.HotelId == hotelID && op.Status == true).ToListAsync();
            if (hotelPhotos.Any()) { return "Not found image"; }
            foreach (var item in hotelPhotos)
            {
                item.IsMain = false;
                if (item.PhotoId == photoID)
                {
                    item.IsMain = true;
                }
            }

            if (await _context.SaveChangesAsync() > 0)
                return "1";
            return "3";
        }


        public async Task<IEnumerable<AHotelAllRoomStatusOutput>> GetListAHotelAllRoomStatusSearch(int userId, int hotelId, DateTime today, int totalnight)
        {
            DateTime StartDate = today;
            DateTime EndDate = _other.GetEndDate(today, totalnight);
            var allRoomStatus = await _context.Rooms
                .Include(c => c.Category)
                .ThenInclude(sp => sp.SpecialPrices.Where(op => op.SpecialDate.Value.Date >= DateTime.Now.Date
                        && op.SpecialDate.Value.Date >= StartDate.Date
                        && op.SpecialDate.Value.Date <= EndDate.Date
                        && op.Status.Value))
                .Where(op => op.HotelId == hotelId)
                .ToListAsync();
            if (allRoomStatus == null) { return null; }
            var lsHotelRoomNotBooked = await _context.Rooms
                    .Where(op => op.HotelId == hotelId)
                     .Where(a => a.BookingDetails.All(op => (op.EndDate.Value.Date > DateTime.Today.Date &&
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
                                                ))
                                    .ToListAsync();
            var returnResult = _mapper.Map<IEnumerable<AHotelAllRoomStatusOutput>>(allRoomStatus);

            foreach (var result in returnResult)
            {
                if (lsHotelRoomNotBooked.Select(s => s.RoomId).Contains(result.RoomId))
                {
                    result.BookedStatus = false;
                }
                else
                {
                    result.BookedStatus = true;
                }
                var ls = allRoomStatus.Where(s => s.RoomId == result.RoomId)
                    .Select(s => s.Category.SpecialPrices)
                    .ToList()[0];
                var check = ls.Any(s => s.SpecialPrice1 == null);
                if (check)
                {
                    result.OutOfServiceStatus = true;
                }
                else
                {
                    result.OutOfServiceStatus = false;
                }
            }
            return returnResult;
        }

        public async Task<RoomDetailInfoOutput> GetADetailRoom(int userId, int RoomId, DateTime today)
        {
            var RoomDetail = await _context.Rooms.Where(op => op.RoomId == RoomId)
                .Include(bd => bd.BookingDetails.Where(op=>op.StartDate.Value.Date<=today.Date && op.EndDate.Value.Date>=today.Date)).ThenInclude(b => b.Booking).ThenInclude(ib => ib.InboundUsers)
                .Include(bd => bd.BookingDetails.Where(op => op.StartDate.Value.Date <= today.Date && op.EndDate.Value.Date >= today.Date)).ThenInclude(b => b.Booking).ThenInclude(u => u.User)
                .Include(c => c.Category)
                .SingleOrDefaultAsync()
            ;

            if (RoomDetail == null) { return null; }
            var returnResult = _mapper.Map<RoomDetailInfoOutput>(RoomDetail);
            return returnResult;
        }

        public async Task<IEnumerable<AHotelAllRoomStatusOutput>> GetListAHotelAllRoomStatusToday(int userId, int hotelId, DateTime today)
        {
            var allRoomStatus = await _context.Rooms
                .Include(c => c.Category)
                .ThenInclude(sp => sp.SpecialPrices.Where(op => op.SpecialDate.Value.Date == DateTime.Now.Date
                                                                && op.Status.Value))
                .Where(op => op.HotelId == hotelId)
                .ToListAsync();

            if (allRoomStatus == null) { return null; }
            var lsHotelRoomNotBooked = await _context.Rooms
                    .Where(op => op.HotelId == hotelId)
                     .Where(a => a.BookingDetails.All(op => (op.EndDate.Value.Date > DateTime.Today.Date &&
                                                  !(op.StartDate.Value.Date <= today.Date
                                                  && op.EndDate.Value.Date >= today.Date)
                                                  )
                                                ))
                                    .ToListAsync();
            var returnResult = _mapper.Map<IEnumerable<AHotelAllRoomStatusOutput>>(allRoomStatus);

            foreach (var result in returnResult)
            {
                if (lsHotelRoomNotBooked.Select(s => s.RoomId).Contains(result.RoomId))
                {
                    result.BookedStatus = false;
                }
                else
                {
                    result.BookedStatus = true;
                }
                var ls = allRoomStatus.Where(s => s.RoomId == result.RoomId)
                    .Select(s => s.Category.SpecialPrices)
                    .ToList()[0];
                var check = ls.Any(s => s.SpecialPrice1 == null);
                if (check)
                {
                    result.OutOfServiceStatus = true;
                }
                else
                {
                    result.OutOfServiceStatus = false;
                }
            }
            return returnResult;
        }

        public async Task<IEnumerable<AHotelAllRoomStatusOutput>> GetListAHotelAllRoomStatusCheckOut(int userId, int hotelId, DateTime today)
        {
            var allRoomStatus = await _context.Rooms
                .Include(c => c.Category)
                .ThenInclude(sp => sp.SpecialPrices.Where(op => op.SpecialDate.Value.Date == DateTime.Now.Date
                                                                && op.Status.Value))
                .Where(op => op.HotelId == hotelId)
                .ToListAsync();

            if (allRoomStatus == null) { return null; }
            var lsHotelRoomNotBooked = await _context.Rooms
                    .Where(op => op.HotelId == hotelId)
                     .Where(a => a.BookingDetails.All(op => (op.EndDate.Value.Date > DateTime.Today.Date &&
                                                  !(op.StartDate.Value.Date <= today.Date
                                                  && op.EndDate.Value.Date >= today.Date)
                                                  )
                                                ))
                                    .ToListAsync();
            var returnResult = _mapper.Map<IEnumerable<AHotelAllRoomStatusOutput>>(allRoomStatus);

            foreach (var result in returnResult)
            {
                if (lsHotelRoomNotBooked.Select(s => s.RoomId).Contains(result.RoomId))
                {
                    result.BookedStatus = false;
                }
                else
                {
                    result.BookedStatus = true;
                }
                var ls = allRoomStatus.Where(s => s.RoomId == result.RoomId)
                    .Select(s => s.Category.SpecialPrices)
                    .ToList()[0];
                var check = ls.Any(s => s.SpecialPrice1 == null);
                if (check)
                {
                    result.OutOfServiceStatus = true;
                }
                else
                {
                    result.OutOfServiceStatus = false;
                }
            }
            return returnResult.Where(op=>op.BookedStatus==true).ToList();
        }
    }
}
