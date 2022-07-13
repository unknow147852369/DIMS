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
        private readonly IGenerateQr _generateqr;
        private readonly IFireBaseService _fireBase;
        private readonly IMailQrService _qrmail;
        private readonly IMailBillService _billmail;

        private string error = "";

        public HostManageRepository(fptdimsContext context, IMapper mapper, IOtherService other, IMailBillService billmail, IMailQrService qrmail, IFireBaseService fireBase, IGenerateQr generateqr)
        {
            _context = context;
            _mapper = mapper;
            _other = other;
            _billmail = billmail;
            _qrmail = qrmail;
            _fireBase = fireBase;
            _generateqr = generateqr;
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
                    result.AllStatus = 1;
                }
                else
                {
                    result.AllStatus = 2;
                }
                if (result.CleanStatus == true)
                {
                    result.AllStatus = 3;
                }
            }
            return returnResult;
        }

        public async Task<RoomDetailInfoOutput> GetADetailRoom(int userId, int RoomId, DateTime today)
        {
            var RoomDetail = await _context.Rooms.Where(op => op.RoomId == RoomId)
                .Include(bd => bd.BookingDetails.Where(op => op.StartDate.Value.Date <= today.Date && op.EndDate.Value.Date >= today.Date)).ThenInclude(b => b.Booking).ThenInclude(ib => ib.InboundUsers)
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
                    result.AllStatus = 1;
                }
                else
                {
                    result.AllStatus = 2;
                }
                if (result.CleanStatus == true)
                {
                    result.AllStatus = 3;
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
                    result.AllStatus = 1;
                    result.BookedStatus = false;
                }
                else
                {
                    result.AllStatus = 2;
                    result.BookedStatus = true;
                }
                if (result.CleanStatus == true)
                {
                    result.AllStatus = 3;
                }
            }
            return returnResult.Where(op => op.BookedStatus == true).ToList();
        }

        public async Task<string> LocalPaymentFinal(LocalPaymentInput ppi, int userId)
        {
            try
            {
                if (ppi.ArrivalDate.Date < DateTime.Now.Date)
                {
                    error += "Wrong date ;";
                }
                Booking bok = await PaymentCalculateData(ppi, userId);
                bok.PaymentMethod = "LOCAL";
                //
                bool checkExist = await PaymentCheckRoomExist(bok);
                //
                if (checkExist)
                {
                    error += "some rooms had been booked!;";
                }
                if (error != "")
                {
                    return error;
                }
                await _context.Bookings.AddAsync(bok);
                if (await _context.SaveChangesAsync() > 0)
                {
                    var bookingFullDetail = await _context.Bookings
                       .Include(v => v.Voucher)
                       .Include(h => h.Hotel)
                       .Include(u => u.User)
                       .Include(b => b.BookingDetails).ThenInclude(r => r.Room).ThenInclude(c => c.Category)
                       .Where(a => a.BookingId == bok.BookingId).FirstOrDefaultAsync();

                    _fireBase.createFileMainPath(bok, out string imageMainPath, out string imageMainName);
                    _generateqr.GenerateMainQr(bok, imageMainPath, imageMainName);
                    var qrMainLink = await _fireBase.GetlinkMainImage(bok, imageMainPath, imageMainName);

                    QrCheckUp qc = new QrCheckUp
                    {
                        BookingId = bok.BookingId,
                        QrUrl = qrMainLink,
                        Status = true,
                        QrContent = _generateqr.createMainQrContent(bok)
                    };
                    _fireBase.RemoveDirectories(imageMainPath);

                    await _billmail.SendBillEmailAsync(bookingFullDetail, qrMainLink);

                    await _context.QrCheckUps.AddAsync(qc);

                    var ListRoom = _mapper.Map<IEnumerable<QrInput>>(bok.BookingDetails);
                    foreach (var room in ListRoom)
                    {
                        //
                        Qr qrdetail = new();
                        _fireBase.createFilePath(room, out string imagePath, out string imageName);
                        qrdetail.QrContent = _generateqr.createQrContent(room);
                        //
                        _generateqr.GenerateQr(room, imagePath, imageName);
                        //
                        var link = await _fireBase.GetlinkImage(room, imagePath, imageName);
                        qrdetail.QrUrl = link;
                        await _qrmail.SendQrEmailAsync(link, bok, room, bok.Hotel.HotelName);
                        _fireBase.RemoveDirectories(imagePath);
                        //
                        qrdetail.StartDate = bok.StartDate;
                        qrdetail.EndDate = bok.EndDate;
                        _mapper.Map(room, qrdetail);
                        await _context.Qrs.AddAsync(qrdetail);
                    }
                    if (await _context.SaveChangesAsync() > 0)
                        return "1";
                    return "3";
                }
                return "0";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private async Task<Booking> PaymentCalculateData(LocalPaymentInput ppi, int userId)
        {
            Booking bok = new();
            _mapper.Map(ppi, bok);


            IQueryable<Room> roomprice = _context.Rooms
                .Include(c => c.Category)
                .ThenInclude(sp => sp.SpecialPrices.Where(op => op.SpecialDate.Value.Date >= DateTime.Now.Date
                                    && op.SpecialDate.Value.Date >= bok.StartDate.Value.Date
                                    && op.SpecialDate.Value.Date <= bok.EndDate.Value.Date
                                    && op.Status.Value))
                .Where(op => ppi.BookingDetails.Select(s => s.RoomId).Contains(op.RoomId)
                )
                 ;

            var data = await roomprice.ToListAsync();
            foreach (BookingDetail r in bok.BookingDetails)
            {
                var detail = data
                    .Where(op => op.RoomId == r.RoomId);

                var specialDate = detail.Select(s => s.Category.SpecialPrices);
                var sumSpecialPrice = specialDate.Select(s => s.Sum(op => op.SpecialPrice1));
                var normalDate = bok.TotalNight - specialDate.Select(s => s.Count()).First();
                var normalPrice = detail.Sum(op => op.RoomPrice);
                var AveragePrice = (normalDate * normalPrice) + sumSpecialPrice.First();
                r.AveragePrice = AveragePrice;
                r.StartDate = bok.StartDate;
                r.EndDate = bok.EndDate;
                r.Status = true;
                r.BookingDetailPrices.Add(new BookingDetailPrice
                {
                    Price = normalPrice,
                    Status = true,
                });
                var oj = specialDate.ToList()[0];
                foreach (var item in oj)
                {
                    r.BookingDetailPrices.Add(new BookingDetailPrice
                    {
                        Date = item.SpecialDate,
                        Price = item.SpecialPrice1,
                        Status = true,
                    });
                }
            }

            bok.UserId = userId;
           
            if (error != "") { throw new Exception(error); }

            return bok;
        }

        private async Task<bool> PaymentCheckRoomExist(Booking bok)
        {
            IQueryable<Room> lsHotelRoomNotBooked = _context.Rooms
                    .Where(op => op.Status == true && op.HotelId == bok.HotelId)
                    .Where(a => a.BookingDetails.All(op => (
                                                  !(((op.StartDate.Value.Date >= bok.StartDate.Value.Date && op.StartDate.Value.Date <= bok.EndDate.Value.Date)
                                                  && (op.EndDate.Value.Date >= bok.StartDate.Value.Date && op.EndDate.Value.Date <= bok.EndDate.Value.Date))
                                                  || (op.StartDate.Value.Date <= bok.StartDate.Value.Date && op.EndDate.Value.Date >= bok.EndDate.Value.Date)
                                                  || (op.StartDate.Value.Date <= bok.EndDate.Value.Date && op.EndDate.Value.Date >= bok.EndDate.Value.Date)
                                                  || (op.StartDate.Value.Date <= bok.StartDate.Value.Date && op.EndDate.Value.Date >= bok.StartDate.Value.Date)
                                                  ))
                                                ));

            var checkExist = bok.BookingDetails.Select(b => b.RoomId).ToList()
                .Any(op => !lsHotelRoomNotBooked.Select(a => a.RoomId).ToList().Contains(op.Value));

            return checkExist;
        }

        public async Task<string> CheckRoomDateBooking(CheckRoomDateInput chek)
        {
            DateTime StartDate = chek.ArrivalDate;
            DateTime EndDate = _other.GetEndDate(chek.ArrivalDate, chek.TotalNight);

            IQueryable<Room> lsHotelRoomNotBooked = _context.Rooms
                    .Where(op => op.Status == true && op.HotelId == chek.HotelId)
                    .Where(a => a.BookingDetails.All(op => (
                                                  !(((op.StartDate.Value.Date >= StartDate.Date && op.StartDate.Value.Date <= EndDate.Date)
                                                  && (op.EndDate.Value.Date >= StartDate.Date && op.EndDate.Value.Date <= EndDate.Date))
                                                  || (op.StartDate.Value.Date <= StartDate.Date && op.EndDate.Value.Date >= EndDate.Date)
                                                  || (op.StartDate.Value.Date <= EndDate.Date && op.EndDate.Value.Date >= EndDate.Date)
                                                  || (op.StartDate.Value.Date <= StartDate.Date && op.EndDate.Value.Date >= StartDate.Date)
                                                  ))
                                                ));
            var error = "Room ";
            foreach (var room in chek.BookingDetails)
            {
                var checkExist = !lsHotelRoomNotBooked.Select(a => a.RoomId).ToList().Contains(room.RoomId);
                if (checkExist)
                {
                    var r = await _context.Rooms.Where(a => a.RoomId == room.RoomId && a.HotelId==chek.HotelId).FirstOrDefaultAsync();
                    if (r == null) { return "Some room not exist!"; }
                    error += r.RoomName+", ";
                }
            }
            ;
            if (error.Equals(error)) { return "All Roome are avaiable!"; }
            return error +" have been booked!";
        }

        public async Task<IEnumerable<HotelListMenuOutput>> GetListMenu(int hotelID)
        {
            var returnList = await _context.Menus
                .Where(op=>op.HotelId==hotelID && op.MenuStatus.Value).ToListAsync();
            return _mapper.Map< IEnumerable < HotelListMenuOutput >> (returnList);
        }
    }
}