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

        public async Task<string> checkIn(checkInInput checkIn)
        {
           // var bokingdetailInfo = await _context.Qrs
           //     .Include(a => a.BookingDetail).ThenInclude(b=>b.InboundUsers)
           //     .Where(a => a.BookingDetail.BookingId == checkIn.BookingId && a.BookingDetail.RoomId == checkIn.RoomId)
           // .FirstOrDefaultAsync();

           // var detail = await _context.BookingDetails
           //    .Include(a => a.Booking)
           //    .Include(b => b.InboundUsers)
           //    .Where(a => a.BookingId == checkIn.BookingId && a.RoomId == checkIn.RoomId)
           //.FirstOrDefaultAsync();
           // if (detail != null)
           // {
           //     foreach (var people in checkIn.InboundUsers)
           //     {
           //         InboundUser ib = new()
           //         {
           //             UserName = people.UserName,
           //             UserIdCard = people.UserIdCard,
           //             UserBirthday = people.UserBirthday,
           //             Status = 1,
           //         };
           //         _mapper.Map(detail, ib);
           //         await _context.InboundUsers.AddAsync(ib);
           //     }
           // }

           // if (bokingdetailInfo != null)
           // {
           //     bokingdetailInfo.Status = 1;
           //     bokingdetailInfo.CheckIn = checkIn.CheckIn;
           // }

           // if (await _context.SaveChangesAsync() > 0)
           //     return "1";
            return "3";
        }

        public async Task<string> CheckOut(CheckOutInput checkOut)
        {
            //var bokingdetailInfo = await _context.Qrs
            //    .Include(a => a.BookingDetail)
            //    .Where(a => a.BookingDetail.BookingId == checkOut.BookingId && a.BookingDetail.RoomId == checkOut.RoomId)
            //.FirstOrDefaultAsync();

            //if (bokingdetailInfo != null)
            //{
            //    bokingdetailInfo.Status = 0;
            //    bokingdetailInfo.CheckOut = checkOut.CheckOut;
            //}
            //if (await _context.SaveChangesAsync() > 0)
            //    return "1";
            return "3";
        }

        public async Task<IEnumerable<Qr>> getListQrString(int bookingID)
        {
            var content = await _context.Qrs.Include(b => b.BookingDetail)
                .Where(a => a.BookingDetail.BookingId == bookingID)
                .ToListAsync();
            return content;
        }

        public async Task<QrOutput> getStringToCheckRoom(int hotel, string roomName)
        {
            var today = DateTime.Now.Date;
            var lsHotelRoom = await _context.BookingDetails
                          .Include(b => b.Booking)
                          .Include(r => r.Room)
                          .Include(q => q.Qr)
                          .Where(op =>  op.Booking.HotelId == hotel && op.Room.RoomName.Equals(roomName.Trim().ToLower()))
                          .Where(op => ((op.StartDate.Value.Date <= today && op.EndDate.Value >= today)))
                          .FirstOrDefaultAsync();

            if (lsHotelRoom != null)
            {
                return _mapper.Map<QrOutput>(lsHotelRoom.Qr);
            }

            return null;
        }

        public async Task<Booking> vertifyMainQrCheckIn(VertifyMainQrInput qrIn)
        {
            string BookingId, HotelId;
            _generateqr.GetMainQrDetail(qrIn, out BookingId,out HotelId);
            if(BookingId == "" || HotelId == "")
            {
                return null;
            }
            if (!qrIn.HotelId.Equals(int.Parse(HotelId)))
            {
                return null;
            }
            var qrvertify = await _context.QrCheckUps
                .Include(b => b.Booking)
                .Where(op=>op.BookingId.Equals(int.Parse(BookingId)))
                .FirstOrDefaultAsync();
            if (qrvertify != null)
            {
                qrvertify.Status = true;
                qrvertify.CheckIn = DateTime.Now;
            }
            var qrs = await _context.Qrs
                .Where(op => op.BookingDetail.Booking.BookingId.Equals(int.Parse(BookingId)))
                .ToListAsync();
            qrs.ForEach(op => op.Status = true);

            if (await _context.SaveChangesAsync() > 0)
            { 
                var bookinginfo = await _context.Bookings
                    .Include(bd => bd.BookingDetails)
                    .Where(op => op.BookingId.Equals(int.Parse(BookingId)))
                    .FirstOrDefaultAsync();
                return bookinginfo;
            }
            return null;
        }


        public async Task<string> vertifyQrContent(VertifyQrInput qrIn)
        {
            var condition = "";
            string BookingId, RoomId;
            _generateqr.GetQrDetail(qrIn, out BookingId, out RoomId);
            var qrvertify = await _context.Qrs
                .Include(b => b.BookingDetail).ThenInclude(r => r.Room)
                .Include(b => b.BookingDetail).ThenInclude(a => a.Booking)
                .Where(c => c.BookingDetail.BookingId.Equals(int.Parse(BookingId))
                && c.BookingDetail.RoomId.Equals(int.Parse(RoomId))
                && c.Status == true)
                .FirstOrDefaultAsync();

            if (qrvertify != null)
            {
                if (qrvertify.BookingDetail.Booking.HotelId.Equals(qrIn.HotelId)
                    && qrvertify.BookingDetail.RoomId.Equals(qrIn.RoomId))
                {
                    condition = "1";
                }
                else
                {
                    condition = "0";
                }
            }
            else { condition = "wrong infrom"; }
            return condition;
        }
    }
}