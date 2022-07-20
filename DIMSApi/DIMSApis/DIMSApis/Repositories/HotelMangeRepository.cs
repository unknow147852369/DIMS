using AutoMapper;
using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;
using Microsoft.EntityFrameworkCore;

namespace DIMSApis.Repositories
{
    public class HotelMangeRepository : IHotelManage
    {
        private readonly fptdimsContext _context;
        private readonly IMapper _mapper;
        private readonly IOtherService _other;
        private readonly IGenerateQr _generateqr;
        private readonly IFireBaseService _fireBase;
        private readonly IMailQrService _qrmail;
        private readonly IMailBillService _billmail;

        private string error = "";

        public HotelMangeRepository(fptdimsContext context, IMapper mapper, IOtherService other, IMailBillService billmail, IMailQrService qrmail, IFireBaseService fireBase, IGenerateQr generateqr)
        {
            _context = context;
            _mapper = mapper;
            _other = other;
            _billmail = billmail;
            _qrmail = qrmail;
            _fireBase = fireBase;
            _generateqr = generateqr;
        }

        public async Task<string> UpdateHotelMainPhoto(int photoID, int hotelID)
        {
            var hotelPhotos = await _context.Photos
               .Where(op => op.HotelId == hotelID && op.Status == true).ToListAsync();
            if (!hotelPhotos.Select(s => s.PhotoId).Contains(photoID)) { return "Not found image"; }
            foreach (var item in hotelPhotos)
            {
                item.IsMain = false;
                if (item.PhotoId == photoID)
                {
                    item.IsMain = true;
                }
            }

            if (await _context.SaveChangesAsync() > 0)
                return "1";
            return "3";
        }

        public async Task<IEnumerable<HotelPhotosOutput>> GetListHotelPhotos(int hotelId)
        {
            var hotelPhotos = await _context.Photos
                .Where(op => op.HotelId == hotelId && op.Status == true).ToListAsync();
            if (hotelPhotos.Count == 0) { return null; }
            return _mapper.Map<IEnumerable<HotelPhotosOutput>>(hotelPhotos);
        }

        public async Task<string> AddAHotelPhotos(ICollection<NewHotelPhotosInput> newPhotos)
        {
            var Phots = _mapper.Map<ICollection<Photo>>(newPhotos);

            await _context.AddRangeAsync(Phots);
            if (await _context.SaveChangesAsync() > 0)
                return "1";
            return "3";
        }

        public async Task<string> RemoveAHotelPhotos(int photo)
        {
            var hotelPhoto = await _context.Photos
               .Where(op => op.Status == true && op.PhotoId == photo).SingleOrDefaultAsync();
            _context.Photos.Remove(hotelPhoto);

            if (await _context.SaveChangesAsync() > 0)
                return "1";
            return "3";
        }

        public async Task<string> RemoveAHotelCate(int CateId)
        {
            var hotelCate = await _context.Categories
                .Include(r => r.Rooms)
            .Where(op => op.CategoryId == CateId).SingleOrDefaultAsync();
            if (hotelCate.Rooms.Count > 0) { return "Some Room are use this cate"; }
            _context.Categories.Remove(hotelCate);

            if (await _context.SaveChangesAsync() > 0)
                return "1";
            return "3";
        }

        public async Task<string> AddAHotelCates(ICollection<NewHotelCateInpput> newCate)
        {
            var cats = _mapper.Map<ICollection<Category>>(newCate);

            await _context.AddRangeAsync(cats);
            if (await _context.SaveChangesAsync() > 0)
                return "1";
            return "3";
        }

        public async Task<string> UpdateHotelCate(NewUpdateHotelCateInput newCate)
        {
            if (newCate.Quanity == 0) { return "Quanity not avaiable"; }
            if (newCate.PriceDefault == 0) { return "Price not avaiable"; }
            var hotelCate = await _context.Categories
                    .Where(op => op.HotelId == newCate.HotelId && op.CategoryId == newCate.CategoryId).SingleOrDefaultAsync();
            if (hotelCate == null) { return "Not found Cate in hotel"; }

            _mapper.Map(newCate, hotelCate);

            if (await _context.SaveChangesAsync() > 0)
                return "1";
            return "3";
        }

        public async Task<IEnumerable<Category>> GetListHotelCates(int hotelId)
        {
            var hotelCates = await _context.Categories
                .Where(op => op.HotelId == hotelId).ToListAsync();
            if (hotelCates.Count == 0) { return null; }
            return hotelCates;
        }

        public async Task<string> RemoveACatePhotos(int photo)
        {
            var hotelPhoto = await _context.Photos
                 .Where(op => op.Status == true && op.PhotoId == photo).SingleOrDefaultAsync();
            _context.Photos.Remove(hotelPhoto);

            if (await _context.SaveChangesAsync() > 0)
                return "1";
            return "3";
        }

        public async Task<string> AddACatePhotos(NewCatePhotosInput newPhotos)
        {
            var Phots = _mapper.Map<ICollection<Photo>>(newPhotos.Photos).ToList();
            Phots.ForEach(f => { f.HotelId = newPhotos.HotelId; f.CategoryId = newPhotos.CategoryId; });
            var hotelPhotos = await _context.Photos
                    .Where(op => op.HotelId == newPhotos.HotelId && op.Status == true && op.CategoryId == newPhotos.CategoryId).ToListAsync();
            if (hotelPhotos.Count() == 0) { Phots.First().IsMain = true; }
            await _context.AddRangeAsync(Phots);
            if (await _context.SaveChangesAsync() > 0)
                return "1";
            return "3";
        }

        public async Task<string> UpdateCateMainPhoto(int photoID, int hotelID, int cateID)
        {
            var hotelPhotos = await _context.Photos
                    .Where(op => op.HotelId == hotelID && op.Status == true && op.CategoryId == cateID).ToListAsync();
            if (!hotelPhotos.Select(s => s.PhotoId).Contains(photoID)) { return "Not found image"; }
            foreach (var item in hotelPhotos)
            {
                item.IsMain = false;
                if (item.PhotoId == photoID)
                {
                    item.IsMain = true;
                }
            }

            if (await _context.SaveChangesAsync() > 0)
                return "1";
            return "3";
        }

        public async Task<IEnumerable<Category>> GetListCatePhotos(int hotelId)
        {
            var hotelCatePhotos = await _context.Categories
                .Include(c => c.Photos)
                .Where(op => op.HotelId == hotelId).ToListAsync();
            if (hotelCatePhotos == null) { return null; }
            return hotelCatePhotos;
        }

        public async Task<string> RemoveARoom(int roomId)
        {
            try
            {
                var item = await _context.Rooms
                        .Where(op => op.RoomId == roomId).SingleOrDefaultAsync();
                _context.Rooms.Remove(item);

                if (await _context.SaveChangesAsync() > 0)
                    return "1";
                return "3";
            }
            catch (Exception ex)
            {
                return "Some Roome are in use";
            }
        }

        public async Task<string> AddRooms(NewRoomFirstInput newRoom)
        {
            var cate = await _context.Categories
                .Where(op => op.HotelId == newRoom.HotelId).ToListAsync();
            var rooms =await _context.Rooms
                .Where(op => op.HotelId == newRoom.HotelId).ToListAsync();
            var item = _mapper.Map<ICollection<Room>>(newRoom.Rooms).ToList();
            item.ForEach(f => { f.HotelId = newRoom.HotelId; });
            foreach (var r in item)
            {
                if (!cate.Select(s => s.CategoryId).ToList().Contains(r.CategoryId.Value))
                {
                    return "not found category in hotel";
                }
                if(rooms.Count() > 0)
                {
                    if(rooms.Select(s => s.RoomName).ToList().Contains(r.RoomName))
                    {
                        return "Room name" +r.RoomName+ " Name exist";
                    }
                }
                r.RoomPrice = cate.Where(op => op.CategoryId == r.CategoryId).Select(s => s.PriceDefault).First();
            }
            await _context.AddRangeAsync(item);
            if (await _context.SaveChangesAsync() > 0)
                return "1";
            return "3";
        }

        public async Task<string> UpdateARoom(NewUpdateRoomInput newRoom)
        {
            var item = await _context.Rooms
                    .Where(op => op.HotelId == newRoom.HotelId  && op.RoomId == newRoom.RoomId).SingleOrDefaultAsync();
            if (item == null) { return "Not found room in hotel"; }
            var cate = await _context.Categories
               .Where(op => op.HotelId == newRoom.HotelId).ToListAsync();
            var rooms = await _context.Rooms
               .Where(op => op.HotelId == newRoom.HotelId).ToListAsync();
            if (!cate.Select(s => s.CategoryId).ToList().Contains(newRoom.CategoryId.Value))
            {
                return "not found category in hotel";
            }
            if (rooms.Count() > 0)
            {
                if (rooms.Select(s => s.RoomName).ToList().Contains(newRoom.RoomName))
                {
                    return "Room name" + item.RoomName + " Name exist";
                }
            }
            _mapper.Map(newRoom, item);
            item.RoomPrice = cate.Where(op => op.CategoryId == item.CategoryId).Select(s => s.PriceDefault).First();

            if (await _context.SaveChangesAsync() > 0)
                return "1";
            return "3";
        }

        public async Task<IEnumerable<Room>> GetListRoom(int hotelId)
        {
            var hotelRooms = await _context.Rooms
                .Where(op => op.HotelId == hotelId).ToListAsync();
            if (hotelRooms.Count == 0) { return null; }
            return hotelRooms;
        }
    }
}