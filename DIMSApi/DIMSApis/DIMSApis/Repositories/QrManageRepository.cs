using AutoMapper;
using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;
using Microsoft.EntityFrameworkCore;

namespace DIMSApis.Repositories
{
    public class QrManageRepository : IQrManage
    {
        private readonly fptdimsContext _context;
        private readonly IGenerateQr _generateqr;
        private readonly IMapper _mapper;

        public QrManageRepository(fptdimsContext context, IMapper mapper, IGenerateQr generateqr)
        {
            _context = context;
            _mapper = mapper;
            _generateqr = generateqr;
        }



        public async Task<string> CheckInOnline(int hotelId, int BookingID)
        {
            var check = await _context.Bookings
                            .Include(q => q.QrCheckUp)
                            .Include(q => q.BookingDetails).ThenInclude(q => q.Qr)
                            .Where(op => op.BookingId == BookingID && op.HotelId == hotelId)
                            .SingleOrDefaultAsync();

            if (check == null || check.QrCheckUp == null) { return "0"; }
            check.Status = true;
            check.QrCheckUp.Status = true;
            check.QrCheckUp.CheckIn = DateTime.Now;
            check.BookingDetails.ToList().ForEach(q => q.Status = true);
            check.BookingDetails.ToList().ForEach(q => q.Qr.Status = true);
            if (await _context.SaveChangesAsync() > 0)
                return "1";
            return "3";
        }

        public async Task<string> CheckOutOnline(int hotelId, int BookingID)
        {
            var check = await _context.Bookings
                            .Include(q => q.QrCheckUp)
                            .Include(q => q.BookingDetails).ThenInclude(q => q.Qr)
                            .Where(op => op.BookingId == BookingID && op.HotelId == hotelId)
                            .SingleOrDefaultAsync();

            if (check == null || check.QrCheckUp == null) { return "0"; }
            check.Status = false;
            check.QrCheckUp.Status = false;
            check.QrCheckUp.CheckOut = DateTime.Now;
            check.BookingDetails.ToList().ForEach(q => q.Status = false);
            check.BookingDetails.ToList().ForEach(q => q.Qr.Status = false);
            if (await _context.SaveChangesAsync() > 0)
                return "1";
            return "3";
        }

        public async Task<IEnumerable<Qr>> getListQrString(int bookingID)
        {
            var content = await _context.Qrs.Include(b => b.BookingDetail)
                .Where(a => a.BookingDetail.BookingId == bookingID)
                .ToListAsync();
            return content;
        }

        public async Task<string> getStringToCheckRoom(int hotel, string roomName)
        {
            var st = "";
            var today = DateTime.Now.Date;
            var lsHotelRoom = await _context.BookingDetails
                          .Include(b => b.Booking)
                          .Include(r => r.Room)
                          .Include(q => q.Qr)
                          .Where(op => op.Status == true && op.Booking.HotelId == hotel && op.Room.RoomName.Equals(roomName.Trim().ToLower()))
                          .Where(op=>op.Qr.Status.Value)
                          .Where(op => ((op.StartDate.Value.Date <= today && op.EndDate.Value >= today)))
                          .FirstOrDefaultAsync();

            if (lsHotelRoom != null)
            {
                var qr = _mapper.Map<QrOutput>(lsHotelRoom.Qr);
                st = qr.QrContent.ToString();
            }

            return st;
        }

        public async Task<Booking> vertifyMainQrCheckIn(VertifyMainQrInput qrIn)
        {
            string BookingId, HotelId,randomString;
            _generateqr.GetMainQrDetail(qrIn, out BookingId, out HotelId,out randomString);
            if (BookingId == "" || HotelId == "" || randomString =="")
            {
                return null;
            }
            if (!qrIn.HotelId.Equals(int.Parse(HotelId)))
            {
                return null;
            }
            var qrvertify = await _context.QrCheckUps
                .Include(b => b.Booking)
                .Where(op => op.BookingId.Equals(int.Parse(BookingId)) && op.QrCheckUpRandomString.Equals(randomString.Trim()))
                .FirstOrDefaultAsync();
            if (qrvertify != null)
            {
                var check = await _context.Bookings
                            .Include(q => q.QrCheckUp)
                            .Include(ib=>ib.InboundUsers)
                            .Include(q => q.BookingDetails).ThenInclude(q => q.Qr)
                            .Where(op => op.BookingId.Equals(int.Parse(BookingId)) && op.HotelId == qrIn.HotelId)
                            .SingleOrDefaultAsync();

                if (check == null || check.QrCheckUp == null) { return null; }
                check.QrCheckUp.Status = true;
                check.QrCheckUp.CheckIn = DateTime.Now;
                check.BookingDetails.ToList().ForEach(q => q.Qr.Status = true);
                if (await _context.SaveChangesAsync() > 0)
                    return check;
            }
            return null;
        }

        public async Task<string> vertifyQrContent(int HotelId, string RoomName, string QrContent)
        {
            try
            {
                var condition = "";
                string BookingId, RoomId, RandomString;
                _generateqr.GetQrDetail(QrContent, out BookingId, out RoomId, out RandomString);
                var qrvertify = await _context.Qrs
                    .Include(b => b.BookingDetail).ThenInclude(r => r.Room)
                    .Include(b => b.BookingDetail).ThenInclude(a => a.Booking)
                    .Where(c => c.BookingDetail.BookingId.Equals(int.Parse(BookingId))
                    && c.BookingDetail.RoomId.Equals(int.Parse(RoomId))
                    && c.Status == true)
                    .FirstOrDefaultAsync();
                
                if (qrvertify != null)
                {
                    if (qrvertify.BookingDetail.Booking.HotelId.Equals(HotelId)
                        && qrvertify.BookingDetail.Room.RoomName.Equals(RoomName.Trim()))
                    {
                        condition = "1";
                    }
                    else
                    {
                        condition = "0";
                    }
                }
                else { condition = "wrong infrom"; }
                var log = new DoorLog
                {
                    RoomlId = int.Parse(RoomId),
                    CreateDate = DateTime.Now,
                    DoorQrContent = QrContent,
                    DoorCondition = condition,
                    DoorLogStatus = true
                };
                await _context.AddAsync(log);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return condition;
                }
                return "0";
            }catch (Exception ex)
            {
                return "wrong infrom";
            }
        }
    }
}