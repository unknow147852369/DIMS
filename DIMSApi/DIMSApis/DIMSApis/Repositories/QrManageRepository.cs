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

        public async Task<int> CreateBookingQrString(int bookingID)
        {
            var content = await _context.BookingDetails.Where(a => a.BookingId == bookingID && a.Booking.Condition.Equals("APPROVED"))
                 .Include(b => b.Room).ToListAsync();
            var ListRoom = _mapper.Map<IEnumerable<QrInput>>(content);
            if (content != null)
            {         
                foreach (var room in ListRoom)
                {
                    Qr qrdetail = new()
                    {
                        QrContent = _generateqr.createQrContent(room),
                        QrString = Encoding.UTF8.GetBytes(_generateqr.GenerateQrString(room)),
                    };
                    _mapper.Map(room, qrdetail);

                    await _context.Qrs.AddAsync(qrdetail);
                }
                if (await _context.SaveChangesAsync() > 0)
                    return 1;
                return 3;
            }
            else { return 0; }
        }

        public async Task<IEnumerable<Qr>> getListQrString(int bookingID)
        {
            var content = await _context.Qrs.Include(b => b.BookingDetail)
                .Where(a => a.BookingDetail.BookingId == bookingID)
                .ToListAsync();
            return content;
            
        }

        public async Task<string> vertifyQrContent(VertifyQrInput qrIn)
        {
            var condition = "";
            String BookingId, RoomId;
            _generateqr.GetQrDetail(qrIn, out BookingId, out RoomId);
            var qrvertify = await _context.Qrs.Include(b => b.BookingDetail)
                .Where(c => c.BookingDetail.BookingId.Equals(int.Parse(BookingId))
                && c.BookingDetail.RoomId.Equals(int.Parse(RoomId)))
                .FirstOrDefaultAsync();
            if(qrvertify == null)
            {
                condition = "close";
            }
            else
            {
                condition = "open";
            }

            return condition; 
        }
    }
}