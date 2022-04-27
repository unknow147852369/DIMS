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

        public Task<IEnumerable<QrOutput>> getQrString(QrInput qrIn)
        {
            throw new NotImplementedException();
        }
    }
}
