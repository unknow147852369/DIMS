using AutoMapper;
using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;
using Microsoft.EntityFrameworkCore;

namespace DIMSApis.Repositories
{
    public class AdminManageRepository : IAdminManage
    {
        private readonly fptdimsContext _context;
        private readonly IMapper _mapper;
        private readonly IOtherService _otherservice;
        private readonly IMail _mail;

        private string PendingStatus1 = "ACTIVE";
        private string PendingStatus2 = "DENY";
        private string purpose1 = "ACTIVE ACCOUNT";
        private string purpose2 = "CHANGE PASS";
        private string role1 = "HOST";

        public AdminManageRepository(IMail mail, IOtherService otherservice, fptdimsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _otherservice = otherservice;
            _mail = mail;
        }

        public async Task<int> AcpectHost(int UserId)
        {
            var user = await _context.Users
                .Where(u => u.UserId == UserId && u.Status.Value)
                .FirstOrDefaultAsync();
            if (user != null)
            {
                user.Role = role1;
                if (await _context.SaveChangesAsync() > 0)
                    return 1;
                return 3;
            }
            return 0;
        }

        public async Task<int> AcpectHotel(int hotelId)
        {
            var hotel = await _context.Hotels
                .Where(u => u.HotelId == hotelId && u.Status.Value)
                .FirstOrDefaultAsync();
            if (hotel != null)
            {
                hotel.Status = true;
                if (await _context.SaveChangesAsync() > 0)
                    return 1;
                return 3;
            }
            return 0;
        }

        public async Task<IEnumerable<User>> ListAllHost()
        {
            var User = await _context.Users
                .Include(u => u.Hotels)
                .Where(r => r.Role.Equals("HOST"))
                .ToListAsync();
            return User.OrderBy(a => a.Status);
        }

        public async Task<IEnumerable<HotelOutput>> ListAllHotel()
        {
            var hotel = await _context.Hotels
                .Include(u => u.User)
                .Include(p => p.Photos)
                .Include(w => w.WardNavigation)
                .Include(pr => pr.ProvinceNavigation)
                .Include(d => d.DistrictNavigation)
                .ToListAsync();
            return _mapper.Map<IEnumerable<HotelOutput>>(hotel).OrderBy(a => a.Status);
        }

        public async Task<string> AdminCreateUser(AdminRegisterInput userinput)
        {
            byte[] passwordHash, passwordSalt;
            _otherservice.CreatePasswordHash(userinput.Password, out passwordHash, out passwordSalt);
            var ltOtp = new List<NewOtpInput>();
            ltOtp.Add(new NewOtpInput
            {
                Purpose = purpose1,
                CodeOtp = null,
                CreateDate = DateTime.Now,
                Status = 1,
            });
            ltOtp.Add(new NewOtpInput
            {
                Purpose = purpose2,
                CodeOtp = null,
                CreateDate = DateTime.Now,
                Status = 1,
            });
            User user = new()
            {
                Email = userinput.Email.ToLower(),
                CreateDate = DateTime.Now,
                Gender = "UNKNOW",
                Role = userinput.role.Trim().ToUpper(),
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true,
            };

            _mapper.Map(ltOtp, user.Otps);
            await _context.Users.AddAsync(user);
            if (await _context.SaveChangesAsync() > 0)
                return "1";
            return "3";
        }

        public async Task<string> AcpectHotelUpdateRequest(int HotelRequestID, int pendingStatus)
        {
            try
            {
                var hotelRequest = await _context.HotelRequests
                   .Where(op => op.Status == true && op.HotelId != null && op.HotelRequestId == HotelRequestID)
                   .SingleOrDefaultAsync();
                if (hotelRequest == null) { return "Not found request"; }
                if (pendingStatus == 2)
                {
                    hotelRequest.PendingStatus = PendingStatus1;
                    hotelRequest.Status = false;
                }
                else if (pendingStatus == 2)
                {
                    hotelRequest.PendingStatus = PendingStatus1;
                    hotelRequest.Status = false;
                    var hotel = await _context.Hotels
                        .Where(op => op.HotelId == hotelRequest.HotelId)
                        .SingleOrDefaultAsync();
                    if (hotelRequest == null) { return "Not found request"; }

                    hotel.HotelName = hotelRequest.HotelName;
                    hotel.HotelAddress = hotelRequest.HotelAddress;
                    hotel.HotelTypeId = hotelRequest.HotelTypeId;
                    hotel.Province = hotelRequest.Province;
                    hotel.District = hotelRequest.District;
                    hotel.Status = hotelRequest.Status;
                    hotel.Star = hotelRequest.Star;
                    hotel.HotelNameNoMark = _otherservice.RemoveMark(hotelRequest.HotelName);
                }
                else { return "wrong status only 1,2 (1: acpect ; 2:Deny)"; }
                if (await _context.SaveChangesAsync() > 0)
                    return "1";
                return "3";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> AcpectHotelAddRequest(int HotelRequestID, int pendingStatus)
        {
            try
            {
                var hotelRequest = await _context.HotelRequests
                    .Where(op => op.Status == true && op.HotelId == null && op.HotelRequestId == HotelRequestID)
                    .SingleOrDefaultAsync();

                if (hotelRequest == null) { return "Not found request"; }
                if (pendingStatus == 2)
                {
                    hotelRequest.PendingStatus = PendingStatus1;
                    hotelRequest.Status = false;
                }
                else if (pendingStatus == 2)
                {
                    hotelRequest.PendingStatus = PendingStatus1;
                    hotelRequest.Status = false;

                    var newHotel = new Hotel();
                    newHotel.HotelName = hotelRequest.HotelName;
                    newHotel.HotelAddress = hotelRequest.HotelAddress;
                    newHotel.HotelTypeId = hotelRequest.HotelTypeId;
                    newHotel.Province = hotelRequest.Province;
                    newHotel.District = hotelRequest.District;
                    newHotel.Status = hotelRequest.Status;
                    newHotel.Star = hotelRequest.Star;
                    newHotel.HotelNameNoMark = _otherservice.RemoveMark(hotelRequest.HotelName);

                    await _context.AddAsync(newHotel);
                }
                else { return "wrong status only 1,2 (1: acpect ; 2:Deny)"; }
                if (await _context.SaveChangesAsync() > 0)
                    return "1";
                return "3";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<IEnumerable<HotelRequest>> GetLitHotelAddRequests()
        {
            var returnlist = await _context.HotelRequests
                .Where(op => op.Status == true && op.HotelId == null)
                .ToListAsync();
            return returnlist;
        }

        public async Task<IEnumerable<HotelRequest>> GetListHotelUpdateRequests()
        {
            var returnlist = await _context.HotelRequests
                .Where(op => op.Status == true && op.HotelId != null)
                .ToListAsync();
            return returnlist;
        }
    }
}