using AutoMapper;
using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;
using Microsoft.EntityFrameworkCore;

namespace DIMSApis.Repositories
{
    public class BookingManageRepository : IBookingManage
    {
        private readonly fptdimsContext _context;
        private readonly IMapper _mapper;
        private readonly IStripePayment _stripe;
        private readonly IGenerateQr _generateqr;
        private readonly IFireBaseService _fireBase;
        private readonly IMailQrService _qrmail;
        private readonly IMailBillService _billmail;
        private string error = "";

        private string condition1 = "ONLINE";
        private string condition2 = "LOCAl";
        private string condition4 = "succeeded";

        public BookingManageRepository(IMailBillService billmail, IMailQrService qrmail, IFireBaseService fireBase, fptdimsContext context, IMapper mapper, IStripePayment stripe, IGenerateQr generateqr)
        {
            _context = context;
            _mapper = mapper;
            _stripe = stripe;
            _generateqr = generateqr;
            _fireBase = fireBase;
            _qrmail = qrmail;
            _billmail = billmail;
        }

        public async Task<IEnumerable<Booking>> GetListBookingInfo(int UserId)
        {
            var bill = await _context.Bookings
               .Include(h => h.Hotel)
               .Include(b => b.BookingDetails)
               .ThenInclude(r => r.Room).ThenInclude(c => c.Category)
               .Where(op => op.UserId == UserId)
               .OrderByDescending(b => b.BookingId).ToListAsync();

            return bill;
        }

        public async Task<VoucherInfoOutput> VertifyVouvher(int hotelId, string code)
        {
            var vou = await _context.Vouchers
              .Include(h => h.Hotel)
              .Where(op => op.EndDate >= DateTime.Now
              && op.HotelId == hotelId && op.VoucherCode.Equals(code)
              && op.Quantitylimited > 0)
              .SingleOrDefaultAsync();
            var returnVoucher = _mapper.Map<VoucherInfoOutput>(vou);
            return returnVoucher;
        }

        public async Task<string> PaymentProcessing(PaymentProcessingInput ppi, int userId)
        {
            try
            {
                string error = "";
                if (ppi.ArrivalDate.Date < DateTime.Now.Date)
                {
                    error += "Wrong date ;";
                }
                Booking bok = await PaymentCalculateData(ppi, userId);
                if (bok == null)
                {
                    return "some date out of service";
                }
                //if (bok.Voucher == null)
                //{
                //    error += "Your voucher have reach limit ;";
                //}
                //
                //var paymentstatus = condition4;
                var paymentstatus = _stripe.PayWithStripe(ppi.Email, ppi.Token, bok);
                if (paymentstatus.Contains(condition4))
                {
                    if (ppi.Condition.ToLower().Trim().Contains("online"))
                    {
                        bok.Condition = condition1;
                    }
                    else { bok.Condition = condition2; }
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
                           .Include(b => b.BookingDetails).ThenInclude(r => r.Room).ThenInclude(c => c.Category)
                           .Where(a => a.BookingId == bok.BookingId).FirstOrDefaultAsync();

                        _fireBase.createFileMainPath(bok, out string imageMainPath, out string imageMainName);
                        _generateqr.GenerateMainQr(bok, imageMainPath, imageMainName);
                        var qrMainLink = await _fireBase.GetlinkMainImage(bok, imageMainPath, imageMainName);

                        QrCheckUp qc = new QrCheckUp
                        {
                            BookingId = bok.BookingId,
                            QrUrl = qrMainLink,
                            Status = false,
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
                }
                return "0";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private async Task<Booking> PaymentCalculateData(PaymentProcessingInput ppi, int userId)
        {
            Booking bok = new();
            _mapper.Map(ppi, bok);
            float total = 0;
            float sale = 0;
            var error = "";

            IQueryable<SpecialPrice> DatePrice = _context.SpecialPrices
                .Include(c => c.Category).ThenInclude(r => r.Rooms.Where(op => ppi.BookingDetails.Select(s => s.RoomId).Contains(op.RoomId)))
                 .Where(op => op.SpecialDate.Value.Date >= DateTime.Now.Date
                 && op.SpecialDate.Value.Date >= bok.StartDate.Value.Date
                 && op.SpecialDate.Value.Date <= bok.EndDate.Value.Date
                 && op.Status == true
                 && op.Category.Rooms.Where(op => ppi.BookingDetails.Select(s => s.RoomId).Contains(op.RoomId)).Count() > 0)
                ;

            IQueryable<Room> roomprice = _context.Rooms
                .Include(c => c.Category)
                .ThenInclude(sp => sp.SpecialPrices.Where(op => op.SpecialDate.Value.Date >= DateTime.Now.Date
                                    && op.SpecialDate.Value.Date >= bok.StartDate.Value.Date
                                    && op.SpecialDate.Value.Date <= bok.EndDate.Value.Date
                                    && op.Status.Value))
                .Where(op => ppi.BookingDetails.Select(s => s.RoomId).Contains(op.RoomId)
                )
                 ;

            var checkDate = DatePrice.Any(op => op.SpecialPrice1 == null);
            if (checkDate)
            {
                return null;
            }
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
                total = (float)(total + AveragePrice);
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
            if (ppi.VoucherId != null)
            {
                bok.Voucher = await _context.Vouchers
                    .Where(a => a.EndDate.Value.Date >= DateTime.Now.Date && a.VoucherId.Equals(ppi.VoucherId) && a.Quantitylimited > 0)
                    .FirstOrDefaultAsync();
                if (bok.Voucher != null)
                {
                    sale = ((float)(total * bok.TotalNight * bok.Voucher.VoucherSale / 100));
                    bok.VoucherDiscoundPrice = Math.Round(sale, 2);
                    bok.Voucher.Quantitylimited = bok.Voucher.Quantitylimited - 1;
                }
                else
                {
                    error= "voucher out of limit";
                }
            }
            else
            {
                bok.VoucherDiscoundPrice = 0;
            }
            bok.SubTotal = Math.Round((double)(total * bok.TotalNight), 2);
            bok.TotalPrice = Math.Round((double)(bok.SubTotal - bok.VoucherDiscoundPrice), 2);

            return bok;
        }

        private async Task<bool> PaymentCheckRoomExist(Booking bok)
        {
            IQueryable<Room> lsHotelRoomNotBooked = _context.Rooms
                    .Where(op => op.Status == true && op.HotelId == bok.HotelId)
                    .Where(a => a.BookingDetails.All(op => (op.EndDate.Value.Date > DateTime.Today.Date &&
                                                  !(((op.StartDate.Value.Date > bok.StartDate.Value.Date && op.StartDate.Value.Date < bok.EndDate.Value.Date)
                                                  && (op.EndDate.Value.Date > bok.StartDate.Value.Date && op.EndDate.Value.Date < bok.EndDate.Value.Date))
                                                  || (op.StartDate.Value.Date < bok.StartDate.Value.Date && op.EndDate.Value.Date > bok.EndDate.Value.Date)
                                                  || (op.StartDate.Value.Date < bok.EndDate.Value.Date && op.EndDate.Value.Date > bok.EndDate.Value.Date)
                                                  || (op.StartDate.Value.Date < bok.StartDate.Value.Date && op.EndDate.Value.Date > bok.StartDate.Value.Date)
                                                  || (op.StartDate.Value.Date == bok.StartDate.Value.Date
                                                  || op.StartDate.Value.Date == bok.EndDate.Value.Date
                                                  || op.EndDate.Value.Date == bok.StartDate.Value.Date
                                                  || op.EndDate.Value.Date == bok.EndDate.Value.Date)
                                                  ))
                                                ));

            var checkExist = bok.BookingDetails.Select(b => b.RoomId).ToList()
                .Any(op => !lsHotelRoomNotBooked.Select(a => a.RoomId).ToList().Contains(op.Value));

            return checkExist;
        }
    }
}