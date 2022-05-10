using AutoMapper;
using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
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

        public async Task<int> PayBooking(StripeInput stripeIn, int userId)
        {
            var booking = await _context.Bookings
                      .Where(r => r.BookingId.Equals(stripeIn.BooingId) && r.Condition.Equals(condition3))
                      .FirstOrDefaultAsync();
            if (booking.UserId != userId)
            {
                return 0;
            }
            else
            {
                if (stripeIn.conditon.Equals(condition1))
                {
                    if (booking != null)
                    {
                        var paymentstatus = _stripe.PayWithStripe(booking.Email, stripeIn.Token, booking);
                        if (paymentstatus.Contains(condition4))
                        {
                            booking.Condition = condition1;

                            var content = await _context.BookingDetails
                                .Where(a => a.BookingId == stripeIn.BooingId && a.Booking.Condition.Equals(condition3))
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
                            }

                            if (await _context.SaveChangesAsync() > 0)
                                return 1;
                            return 3;
                        }
                        else
                        {
                            return 2;
                        }
                    }
                }
                else
                {
                    booking.Condition = condition2;
                    if (await _context.SaveChangesAsync() > 0)
                        return 1;
                    return 3;
                }
            }
            return 0;
        }

        public async Task<IEnumerable<Booking>> GetListBookingInfo(int UserId)
        {
            var bill = await _context.Bookings
               .Include(h => h.Hotel)
               .Include(b => b.BookingDetails).ThenInclude(r => r.Room).ThenInclude(c => c.Category)
               .OrderByDescending(b => b.BookingId).ToListAsync();

            return bill;
        }

        public async Task<int> SendBookingRequest(BookingInput bok, int UserId)
        {
            try
            {
                Booking book = new();
                foreach (BookingDetailInput r in bok.BookingDetails)
                {
                    var price = await _context.Rooms.Where(a => a.RoomId.Equals(r.RoomId)).FirstOrDefaultAsync();
                    r.Price = price.Price;
                    r.StartDate = bok.StartDate;
                    r.EndDate = bok.EndDate;
                }
                book.UserId = UserId;
                _mapper.Map(bok, book);
                await _context.Bookings.AddAsync(book);

                if (await _context.SaveChangesAsync() > 0)
                    return 1;
                return 3;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}