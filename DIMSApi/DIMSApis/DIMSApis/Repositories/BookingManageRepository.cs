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

        private readonly DIMSContext _context;
        private readonly IMapper _mapper;

        public BookingManageRepository(DIMSContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<int> AccecptBookingRequest(int BooingId, int[] roomId, string condition)
        {
            var booking = await _context.Bookings.Where(r => r.BookingId == BooingId).FirstOrDefaultAsync();
            if (booking != null)
            {
                if (condition.Equals("APPROVED"))
                {
                    foreach (var item in roomId) {
                        var Room = await _context.Rooms.Where(r => r.HotelId == booking.HotelId && r.HotelId == item).FirstOrDefaultAsync();
                        if (Room != null)
                        {
                            BookingDetail bok = new()
                            {
                                BookingId = BooingId,
                                RoomId = item,
                                StartDate = booking.StartDate,
                                EndDate = booking.EndDate,
                                Status = 1,
                            };
                            await _context.AddAsync(bok);
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    booking.Condition = "APPROVED";
                    if (await _context.SaveChangesAsync() > 0)
                        return 1;
                    return 3;
                }
                else
                {
                    booking.Condition = "DENY";
                    if (await _context.SaveChangesAsync() > 0)
                        return 1;
                    return 3;
                }
            }
            return 0;
        }

        public async Task<IEnumerable<Booking>> GetListBookingInfo(int UserId)
        {
             var bill = await _context.Bookings.Where(a => a.Condition.Equals("APPROVED"))
                .Include(h => h.Hotel)
                .Include(b => b.BookingDetails).ThenInclude(r => r.Room).ThenInclude(c => c.Category)
                .OrderByDescending(b => b.BookingId ).ToListAsync();

            return bill;
        }

        public async Task<int> SendBookingRequest(BookingDetailInput bok, int UserId)
        {
            try
            {
                Booking book = new();
                book.UserId = UserId;
                book.CreateDate = DateTime.Now;
                book.Status = 1;
                book.Condition = "WAIT";
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
