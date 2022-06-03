using AutoMapper;
using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using Microsoft.EntityFrameworkCore;

namespace DIMSApis.Repositories
{
    public class QrManageRepository : IQrManage
    {
        private readonly DIMSContext _context;
        private readonly IGenerateQr _generateqr;
        private readonly IMapper _mapper;

        public QrManageRepository(DIMSContext context, IMapper mapper, IGenerateQr generateqr)
        {
            _context = context;
            _mapper = mapper;
            _generateqr = generateqr;
        }

        public async Task<string> checkIn(checkInInput checkIn)
        {
            var bokingdetailInfo = await _context.Qrs
                .Include(a => a.BookingDetail).ThenInclude(b=>b.InboundUsers)
                .Where(a => a.BookingDetail.BookingId == checkIn.BookingId && a.BookingDetail.RoomId == checkIn.RoomId)
            .FirstOrDefaultAsync();

            var detail = await _context.BookingDetails
               .Include(a => a.Booking)
               .Include(b => b.InboundUsers)
               .Where(a => a.BookingId == checkIn.BookingId && a.RoomId == checkIn.RoomId)
           .FirstOrDefaultAsync();
            if (detail != null)
            {
                foreach (var people in checkIn.InboundUsers)
                {
                    InboundUser ib = new()
                    {
                        UserName = people.UserName,
                        UserIdCard = people.UserIdCard,
                        UserBirthday = people.UserBirthday,
                        Status = 1,
                    };
                    _mapper.Map(detail, ib);
                    await _context.InboundUsers.AddAsync(ib);
                }
            }

            if (bokingdetailInfo != null)
            {
                bokingdetailInfo.Status = 1;
                bokingdetailInfo.CheckIn = checkIn.CheckIn;
            }

            if (await _context.SaveChangesAsync() > 0)
                return "1";
            return "3";
        }

        public async Task<string> CheckOut(CheckOutInput checkOut)
        {
            var bokingdetailInfo = await _context.Qrs
                .Include(a => a.BookingDetail)
                .Where(a => a.BookingDetail.BookingId == checkOut.BookingId && a.BookingDetail.RoomId == checkOut.RoomId)
            .FirstOrDefaultAsync();

            if (bokingdetailInfo != null)
            {
                bokingdetailInfo.Status = 0;
                bokingdetailInfo.CheckOut = checkOut.CheckOut;
            }
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
            var today = DateTime.Now;
            var check = "empty";
            var lsHotelRoom = await _context.BookingDetails
                          .Include(b => b.Booking)
                          .Include(r => r.Room)
                          .Include(q => q.Qr)
                          .Where(op => op.Status == 1 && op.Booking.HotelId == hotel && op.Room.RoomName.Equals(roomName))
                          .Where(op => ((op.StartDate < today && op.EndDate > today)))
                          .FirstOrDefaultAsync();

            if (lsHotelRoom != null)
            {
                check = lsHotelRoom.Qr.QrContent;
            }

            return check;
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
                && c.Status == 1)
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