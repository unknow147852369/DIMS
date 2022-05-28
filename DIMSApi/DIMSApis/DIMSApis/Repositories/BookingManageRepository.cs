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
        private readonly DIMSContext _context;
        private readonly IMapper _mapper;
        private readonly IStripePayment _stripe;
        private readonly IGenerateQr _generateqr;

        private string condition1 = "PAID";
        private string condition2 = "CANCEL";
        private string condition3 = "WAIT";
        private string condition4 = "succeeded";
        private string condition5 = "fail";

        public BookingManageRepository(DIMSContext context, IMapper mapper, IStripePayment stripe, IGenerateQr generateqr)
        {
            _context = context;
            _mapper = mapper;
            _stripe = stripe;
            _generateqr = generateqr;
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
                    r.Status = 1;
                    total = (float)(total + price.Price);
                }
                bok.UserId = userId;
                if (ppi.VoucherId != null)
                {
                    bok.Voucher = await _context.Vouchers.Where(a => a.VoucherId.Equals(ppi.VoucherId)).FirstOrDefaultAsync();
                    sale = ((float)(total * bok.Voucher.VoucherSale / 100));
                }
                bok.TotalPrice = (total - sale) * (bok.EndDate - bok.StartDate).Value.TotalDays;

                var paymentstatus = condition4;
                    //_stripe.PayWithStripe(ppi.Email, ppi.Token, bok);
                if (paymentstatus.Contains(condition4))
                {
                    bok.Condition = condition1;
                    await _context.Bookings.AddAsync(bok);
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        var ListRoom = _mapper.Map<IEnumerable<QrInput>>(bok.BookingDetails);
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

        public async Task<VoucherInfoOutput> VertifyVouvher(int hotelId,string code)
        {
            var vou = await _context.Vouchers
              .Include(h => h.Hotel).Where(op=>op.EndDate > DateTime.Now && op.HotelId == hotelId && op.VoucherCode.Equals(code))
              .SingleOrDefaultAsync();
            var returnVoucher = _mapper.Map<VoucherInfoOutput>(vou);
            return returnVoucher;
        }
    }
}