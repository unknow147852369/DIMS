using AutoMapper;
using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using DIMSApis.Models.Input;

namespace DIMSApis.Repositories
{
    public class UserBookingManageRepository : IUserBookingManage
    {

        private readonly DIMSContext _context;
        private readonly IMapper _mapper;

        public UserBookingManageRepository(DIMSContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<bool> SendBookingRequest(BookingRequestInput rent, int UserId)
        {
            //RentedRoom renter = new()
            //{
            //    RoomId = rent.RoomId,
            //    UserId = UserId,
            //    EndDate = rent.EndDate,
            //    CreateDate = DateTime.Today,
            //    Status = 1
            //};
            //await _context.RentedRooms.AddAsync(renter);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
