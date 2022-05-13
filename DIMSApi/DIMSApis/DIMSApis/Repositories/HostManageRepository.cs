using AutoMapper;
using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;
using Microsoft.EntityFrameworkCore;

namespace DIMSApis.Repositories
{
    public class HostManageRepository : IHostManage
    {
        private readonly DIMSContext _context;
        private readonly IMapper _mapper;

        public HostManageRepository(DIMSContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> CreateHotel(HotelInput hotel, int userId)
        {
            Hotel ht = new();
            ht.UserId = userId;
            _mapper.Map(hotel, ht);
            await _context.Hotels.AddAsync(ht);
            if (await _context.SaveChangesAsync() > 0)
                return 1;
            return 3;
        }

        public async Task<string> CreateRoom(NewRoomInput room, int userId)
        {
            var lsRoom = await _context.Rooms.Where(op => op.HotelId == room.HotelId).ToListAsync();
            var duplicateRoom = "";
            if (lsRoom != null)
            {
                foreach (var r in lsRoom)
                {
                    if (r.RoomName == room.RoomName)
                    {
                        duplicateRoom += room.RoomName + ",";
                    }
                }
            }
            if (duplicateRoom == "")
            {
                Room ro = new();
                _mapper.Map(room, ro);
                await _context.Rooms.AddAsync(ro);
                if (await _context.SaveChangesAsync() > 0)
                    return "1";
                return "3";
            }
            return duplicateRoom;
        }

        public async Task<IEnumerable<HotelOutput>> GetListAllHotel(int userId)
        {
            var lsHotel = await _context.Hotels
                .Include(p => p.Photos)
                .Include(h => h.Rooms)
                .Include(w => w.WardNavigation)
                .Include(d => d.DistrictNavigation)
                .Include(pr => pr.ProvinceNavigation)
                .Where(op => op.UserId == userId).ToListAsync();
            var returnHotel = _mapper.Map<IEnumerable<HotelOutput>>(lsHotel);
            return returnHotel;
        }

        public async Task<IEnumerable<HotelRoomOutput>> GetListAllHotelRoom(int hotelId, int userId)
        {
            var lsHotelRoom = await _context.Rooms
                .Include(h => h.Category)
                .Where(op => op.HotelId == hotelId).ToListAsync();
            var returnHotelRoom = _mapper.Map<IEnumerable<HotelRoomOutput>>(lsHotelRoom);

            return returnHotelRoom;
        }
    }
}