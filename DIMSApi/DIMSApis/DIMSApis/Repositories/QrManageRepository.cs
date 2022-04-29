using AutoMapper;
using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;
using System.Data.Entity;

namespace DIMSApis.Repositories
{
    public class QrManageRepository:IQrManage
    {
        private readonly DIMSContext _context;
        private readonly IMapper _mapper;

        public QrManageRepository(DIMSContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<int> CreateBookingQrString(QrInput qrIn)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<QrOutput>> getListQrString(QrInput qrIn)
        {
            throw new NotImplementedException();
        }
    }
}
