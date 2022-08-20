using AutoMapper;
using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using Microsoft.EntityFrameworkCore;

namespace DIMSApis.Repositories
{
    public class UserQrRepository : IUserQr
    {
        private readonly fptdimsContext _context;
        private readonly IGenerateQr _generateqr;
        private readonly IMapper _mapper;
        private readonly IOtherService _other;
        private readonly IMailQrService _qrmail;

        public UserQrRepository(IMailQrService qrmail, IOtherService other, fptdimsContext context, IMapper mapper, IGenerateQr generateqr)
        {
            _context = context;
            _mapper = mapper;
            _generateqr = generateqr;
            _other = other;
            _qrmail = qrmail;
        }

        public async Task<string> UserGetNewQrRoom(int bookingID, int bookingdetailID)
        {
            try
            {
                var check = await _context.Bookings
                               .Include(h => h.Hotel)
                            .Include(q => q.QrCheckUp)
                            .Include(q => q.BookingDetails).ThenInclude(q => q.Qr)
                            .Include(q => q.BookingDetails).ThenInclude(r => r.Room)
                            .Where(op => op.BookingId == bookingID && op.BookingDetails.All(r => r.BookingDetailId == bookingdetailID))
                            .Where(op => op.QrCheckUp.CheckOut == null)
                            .SingleOrDefaultAsync();
                if (check == null)
                {
                    return "booking info wrong!";
                }

                var ListRoom = _mapper.Map<IEnumerable<QrInput>>(check.BookingDetails);
                var room = ListRoom.FirstOrDefault();
                var returndetail = check.BookingDetails.First();
                if (returndetail.Qr.QrLimitNumber == 3)
                {
                    return "You have reach limit to renew Qr 3 times";
                }
                string DetailQrUrl = "";
                string DetailQrContent = "";
                var randomString = _other.RandomString(6);
                _generateqr.GetQrDetailUrlContent(room, randomString, out DetailQrContent, out DetailQrUrl);

                returndetail.Qr.QrContent = DetailQrContent;

                returndetail.Qr.QrUrl = DetailQrUrl;
                returndetail.Qr.QrRandomString = randomString;
                if (returndetail.Qr.QrCreateDate.Value.Date == DateTime.Now.Date)
                {
                    returndetail.Qr.QrLimitNumber += 1;
                }
                else
                {
                    returndetail.Qr.QrCreateDate = DateTime.Now;
                    returndetail.Qr.QrLimitNumber = 0;
                }

                await _qrmail.SendQrEmailAsync(DetailQrUrl, check, room, check.Hotel.HotelName);

                if (await _context.SaveChangesAsync() > 0)
                    return "1";
                return "3";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}