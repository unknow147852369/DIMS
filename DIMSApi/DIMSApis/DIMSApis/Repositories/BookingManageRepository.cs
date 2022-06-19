using AutoMapper;
using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;
using Microsoft.EntityFrameworkCore;
using System.Text;

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
               .Include(b => b.BookingDetails).ThenInclude(r => r.Room).ThenInclude(c => c.Category)
               .OrderByDescending(b => b.BookingId).ToListAsync();

            return bill;
        }

        public async Task<string> PaymentProcessing(PaymentProcessingInput ppi, int userId)
        {
            try
            {
                var checkExist = true;
                Booking bok = new();
                _mapper.Map(ppi, bok);
                float total = 0;
                float sale = 0;
                foreach (BookingDetail r in bok.BookingDetails)
                {
                    var price = await _context.Rooms.Where(a => a.RoomId.Equals(r.RoomId)).FirstOrDefaultAsync();
                    r.Price = price.Price;
                    r.StartDate = bok.StartDate;
                    r.EndDate = bok.EndDate;
                    r.Status = true;
                    total = (float)(total + price.Price);
                }
                bok.UserId = userId;
                if (ppi.VoucherId != null)
                {
                    bok.Voucher = await _context.Vouchers.Where(a => a.VoucherId.Equals(ppi.VoucherId)).FirstOrDefaultAsync();
                    sale = ((float)(total * bok.TotalNight * bok.Voucher.VoucherSale / 100));
                    bok.VoucherDiscoundPrice = sale;
                }
                else
                {
                    bok.VoucherDiscoundPrice = 0;
                }
                bok.SubTotal = total * bok.TotalNight;
                bok.TotalPrice = bok.SubTotal - bok.VoucherDiscoundPrice;

                var paymentstatus = condition4;
                //_stripe.PayWithStripe(ppi.Email, ppi.Token, bok);
                if (paymentstatus.Contains(condition4))
                {
                    if (ppi.Condition.ToLower().Trim().Contains("online"))
                    {
                        bok.Condition = condition1;
                    }
                    else { bok.Condition = condition2; }

                    var lsHotelRoom = await _context.Rooms
                            .Where(op => op.Status == true && op.HotelId == bok.HotelId && bok.EndDate > DateTime.Now)
                            .Where(a => a.BookingDetails.All(op => !(op.EndDate > DateTime.Today &&
                                                          ((op.StartDate > bok.StartDate && op.StartDate < bok.EndDate) && (op.EndDate > bok.StartDate && op.EndDate < bok.EndDate))
                                                          || (op.StartDate < bok.EndDate && op.EndDate > bok.EndDate)
                                                          || (op.StartDate < bok.StartDate && op.EndDate > bok.StartDate))
                                                        )).ToListAsync(); 
                    
                    checkExist = bok.BookingDetails.Select(b =>  b.RoomId ).ToList()
                       .Any(op => lsHotelRoom.Select(a =>   a.RoomId ).ToList().Contains(op.Value));

                    //
                    if (checkExist)
                    {
                        return "some rooms had been booked!";
                    }

                    await _context.Bookings.AddAsync(bok);
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        var ListRoom = _mapper.Map<IEnumerable<QrInput>>(bok.BookingDetails);
                        var bookingFullDetail = await _context.Bookings
                            .Include(v => v.Voucher)
                            .Include(h => h.Hotel)
                            .Include(b => b.BookingDetails).ThenInclude(r => r.Room).ThenInclude(c => c.Category)
                            .Where(a => a.BookingId == bok.BookingId).FirstOrDefaultAsync();
                        await _billmail.SendBillEmailAsync(bookingFullDetail);
                        foreach (var room in ListRoom)
                        {
                            //
                            Qr qrdetail = new();
                            _fireBase.createFilePath(room, out string imagePath, out string imageName);
                            qrdetail.QrContent = _generateqr.createQrContent(room);
                            //
                            qrdetail.QrString = Encoding.UTF8.GetBytes(_generateqr.GenerateQrString(room, imagePath, imageName));
                            //
                            var link = await _fireBase.GetlinkImage(room, imagePath, imageName);

                            await _qrmail.SendQrEmailAsync(link, bok, room, bok.Hotel.HotelName);
                            _fireBase.RemoveDirectories(imagePath);
                            //
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
                return ex.ToString();
            }
        }

        public async Task<VoucherInfoOutput> VertifyVouvher(int hotelId, string code)
        {
            var vou = await _context.Vouchers
              .Include(h => h.Hotel).Where(op => op.EndDate > DateTime.Now && op.HotelId == hotelId && op.VoucherCode.Equals(code))
              .SingleOrDefaultAsync();
            var returnVoucher = _mapper.Map<VoucherInfoOutput>(vou);
            return returnVoucher;
        }
    }
}