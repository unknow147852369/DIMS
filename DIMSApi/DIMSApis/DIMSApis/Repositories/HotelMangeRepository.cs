using AutoMapper;
using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
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
            if (hotelPhotos.Any()) { return "Not found image"; }
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

        public async Task<IEnumerable<HotelPhotosOutput>> GetListHotelPhotos(int userId, int hotelId)
        {
            var hotelPhotos = await _context.Photos
                .Where(op => op.HotelId == hotelId && op.Status == true).ToListAsync();
            return _mapper.Map<IEnumerable<HotelPhotosOutput>>(hotelPhotos);
        }
    }
}
