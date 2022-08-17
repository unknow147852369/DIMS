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
        private readonly IOtherService _other;
        private readonly IMailQrService _qrmail;

        public QrManageRepository(IMailQrService qrmail,IOtherService other,fptdimsContext context, IMapper mapper, IGenerateQr generateqr)
        {
            _context = context;
            _mapper = mapper;
            _generateqr = generateqr;
            _other = other;
            _qrmail = qrmail;
        }



        public async Task<string> CheckInOnline(int hotelId, int BookingID)
        {
            var check = await _context.Bookings
                               .Include(h=>h.Hotel)
                            .Include(q => q.QrCheckUp)
                            .Include(q => q.BookingDetails).ThenInclude(q => q.Qr)
                            .Include(q => q.BookingDetails).ThenInclude(r=>r.Room)
                            .Where(op => op.BookingId == BookingID && op.HotelId == hotelId)
                            .SingleOrDefaultAsync();

            if (check == null || check.QrCheckUp == null) { return "0"; }
            bool earlthcheck = check.StartDate.Value.Add(new TimeSpan(13, 00, 0)) > DateTime.Now;
            //if (earlthcheck) { return "can't check in earlier more than 1h your avaiable time to checkin is " + check.StartDate.Value.Date.Add(new TimeSpan(13, 00, 0)); }
            if (check.QrCheckUp.CheckIn != null) { return "your bookingID has been checkin at " + check.QrCheckUp.CheckIn; }
            check.Status = true;
            check.QrCheckUp.Status = true;
            check.QrCheckUp.CheckIn = DateTime.Now;

            var ListRoom = _mapper.Map<IEnumerable<QrInput>>(check.BookingDetails);
            foreach (var room in ListRoom)
            {
                //
                Qr qrdetail = new();
                //
                string DetailQrUrl = "";
                string DetailQrContent = "";
                var randomString = _other.RandomString(6);
                _generateqr.GetQrDetailUrlContent(room, randomString, out DetailQrContent, out DetailQrUrl);

                qrdetail.QrContent = DetailQrContent;

                qrdetail.QrUrl = DetailQrUrl;
                qrdetail.QrRandomString = randomString;

                await _qrmail.SendQrEmailAsync(DetailQrUrl, check, room, check.Hotel.HotelName);

                //
                _mapper.Map(room, qrdetail);
                qrdetail.StartDate = check.StartDate;
                qrdetail.EndDate = check.EndDate;


                await _context.Qrs.AddAsync(qrdetail);
            }

            check.BookingDetails.ToList().ForEach(q => q.Status = true);
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

        public async Task<string> vertifyMainQrCheckIn(VertifyMainQrInput qrIn)
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
                .Include(b => b.Booking).ThenInclude(b=>b.BookingDetails).ThenInclude(q=>q.Qr)
                .Where(op => op.BookingId.Equals(int.Parse(BookingId)) && op.QrCheckUpRandomString.Equals(randomString.Trim()) && op.QrContent.Equals(qrIn.QrContent))
                .FirstOrDefaultAsync();
            if (qrvertify != null)
            {
                var check = await _context.Bookings
                               .Include(h => h.Hotel)
                            .Include(q => q.QrCheckUp)
                            .Include(q => q.BookingDetails).ThenInclude(q => q.Qr)
                            .Include(q => q.BookingDetails).ThenInclude(r => r.Room)
                            .Where(op => op.BookingId.Equals(int.Parse(BookingId)) && op.HotelId.Equals(int.Parse(HotelId)))
                            .SingleOrDefaultAsync();

                if (check == null || check.QrCheckUp == null) { return "0"; }
                bool earlthcheck = check.StartDate.Value.Add(new TimeSpan(13, 00, 0)) > DateTime.Now;
                //if (earlthcheck) { return "can't check in earlier more than 1h your avaiable time to checkin is " + check.StartDate.Value.Date.Add(new TimeSpan(13, 00, 0)); }
                if (check.QrCheckUp.CheckIn != null) { return "your bookingID has been checkin at " + check.QrCheckUp.CheckIn; }
                check.Status = true;
                check.QrCheckUp.Status = true;
                check.QrCheckUp.CheckIn = DateTime.Now;

                var ListRoom = _mapper.Map<IEnumerable<QrInput>>(check.BookingDetails);
                foreach (var room in ListRoom)
                {
                    //
                    Qr qrdetail = new();
                    //
                    string DetailQrUrl = "";
                    string DetailQrContent = "";
                    var randomStringRoom = _other.RandomString(6);
                    _generateqr.GetQrDetailUrlContent(room, randomStringRoom, out DetailQrContent, out DetailQrUrl);

                    qrdetail.QrContent = DetailQrContent;

                    qrdetail.QrUrl = DetailQrUrl;
                    qrdetail.QrRandomString = randomStringRoom;

                    await _qrmail.SendQrEmailAsync(DetailQrUrl, check, room, check.Hotel.HotelName);

                    //
                    _mapper.Map(room, qrdetail);
                    qrdetail.StartDate = check.StartDate;
                    qrdetail.EndDate = check.EndDate;


                    await _context.Qrs.AddAsync(qrdetail);
                }

                check.BookingDetails.ToList().ForEach(q => q.Status = true);
                if (await _context.SaveChangesAsync() > 0)
                    return "1";
                return "3";
            }
            return "0";
        }

        public async Task<string> vertifyQrContent(int HotelId, string RoomName, string QrContent)
        {
            try
            {
                var condition = "";
                string BookingId, RoomId, RandomString;
                _generateqr.GetQrDetail(QrContent, out BookingId, out RoomId, out RandomString);
                if (BookingId == "" || RoomId == "" || RandomString == "") { condition = "wrong infrom"; }
                else
                {
                    var qrvertify = await _context.Qrs
                        .Include(b => b.BookingDetail).ThenInclude(r => r.Room)
                        .Include(b => b.BookingDetail).ThenInclude(a => a.Booking)
                        .Where(c => c.BookingDetail.BookingId.Equals(int.Parse(BookingId))
                        && c.BookingDetail.RoomId.Equals(int.Parse(RoomId))
                        && c.QrContent.Equals(QrContent.Trim())
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
                }
                var log = new DoorLog();
                if(condition == "1") {
                    log.RoomlId = int.Parse(RoomId) ;
                }
                log.CreateDate = DateTime.Now;
                log.DoorQrContent = QrContent;
                log.DoorCondition = condition;
                log.DoorLogStatus = true;
               
                await _context.DoorLogs.AddAsync(log);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return condition;
                }
                return condition;
            }catch (Exception ex)
            {
                return "wrong infrom";
            }
        }
    }
}