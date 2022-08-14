using AutoMapper;
using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace DIMSApis.Repositories
{
    public class UserQrRepository : IUserQr
    {
        private readonly fptdimsContext _context;
        private readonly IGenerateQr _generateqr;
        private readonly IMapper _mapper;
        private readonly IOtherService _other;
        private readonly IMailQrService _qrmail;

        public UserQrRepository(IMailQrService qrmail, IOtherService other, fptdimsContext context, IMapper mapper, IGenerateQr generateqr)
        {
            _context = context;
            _mapper = mapper;
            _generateqr = generateqr;
            _other = other;
            _qrmail = qrmail;
        }

        public Task<string> UserGetNewQrRoom(int userId, int bookingdetailID)
        {
            throw new NotImplementedException();
        }
    }
}
