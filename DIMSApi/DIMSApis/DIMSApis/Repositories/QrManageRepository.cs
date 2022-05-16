using AutoMapper;
using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;
using Microsoft.EntityFrameworkCore;
using System.Text;

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
                          .Include(b => b.Room)
                          .Where(op => op.Status == 1 && op.Booking.HotelId == hotel)
                          .Where(op => ((op.StartDate < today && op.EndDate > today)))
                          .ToListAsync();
            foreach (var item in lsHotelRoom)
            {
                if(item.Room.RoomName == roomName)
                {
                    check = item.RoomId.ToString();
                }
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
                && c.Status == "1")
                .FirstOrDefaultAsync();
            if(qrvertify.BookingDetail.Booking.HotelId.Equals(qrIn.HotelId) 
                && qrvertify.BookingDetail.RoomId.Equals(qrIn.RoomId))
            {
                condition = "1";
            }
            else
            {
                condition = "0";
            }

            return condition; 
        }
    }
}