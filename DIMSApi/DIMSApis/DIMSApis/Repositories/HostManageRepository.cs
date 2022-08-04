using AutoMapper;
using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using DIMSApis.Models.Helper;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;
using Microsoft.EntityFrameworkCore;

namespace DIMSApis.Repositories
{
    public class HostManageRepository : IHostManage
    {
        private readonly fptdimsContext _context;
        private readonly IMapper _mapper;
        private readonly IMail _mail;
        private readonly IOtherService _other;
        private readonly IGenerateQr _generateqr;
        private readonly IFireBaseService _fireBase;
        private readonly IMailQrService _qrmail;
        private readonly IMailBillService _billmail;
        private readonly IMailCheckOut _checkoutmail;
        private readonly IPaginationService _pagination;

        private string error = "";

        public HostManageRepository(IMail mail, IPaginationService pagination,IMailCheckOut checkoutmail, fptdimsContext context, IMapper mapper, IOtherService other, IMailBillService billmail, IMailQrService qrmail, IFireBaseService fireBase, IGenerateQr generateqr)
        {
            _context = context;
            _mapper = mapper;
            _other = other;
            _billmail = billmail;
            _qrmail = qrmail;
            _fireBase = fireBase;
            _generateqr = generateqr;
            _checkoutmail = checkoutmail;
            _pagination = pagination;
             _mail=mail;
        }

        public async Task<IEnumerable<HotelOutput>> GetListAllHotel(int userId)
        {
            var lsHotel = await _context.Hotels
                .Include(p => p.Photos)
                .Include(ht => ht.HotelType)
                .Include(h => h.Rooms)
                .Include(w => w.WardNavigation)
                .Include(d => d.DistrictNavigation)
                .Include(pr => pr.ProvinceNavigation)
                .Where(op => op.UserId == userId).ToListAsync();
            var returnHotel = _mapper.Map<IEnumerable<HotelOutput>>(lsHotel);
            return returnHotel;
        }

        public async Task<HotelCateInfoOutput> GetAHotelAllInfo(int hotelId, int userId)
        {
            IQueryable<Category> lsCateRooms = _context.Categories
                .Include(p => p.Photos)
                .Include(pr => pr.SpecialPrices.Where(op => op.SpecialDate.Value.Date >= DateTime.Now.Date))
                .Include(r => r.Rooms)
                .Where(op => op.HotelId == hotelId && op.Hotel.UserId == userId);

            if (lsCateRooms == null) { return null; }

            var AHotel = await _context.Hotels
                .Include(p => p.Vouchers.Where(op => op.EndDate.Value.Date >= DateTime.Now.Date))
                .Include(p => p.Photos)
                .Include(h => h.HotelType)
                .Include(c => c.Categories)
                .Include(r => r.Rooms)
                .Include(w => w.WardNavigation)
                .Include(d => d.DistrictNavigation)
                .Include(pr => pr.ProvinceNavigation)
                .Where(op => op.HotelId == hotelId && op.UserId == userId)
                .SingleOrDefaultAsync();

            var result = await lsCateRooms.ToListAsync();

            var HotelDetail = new HotelCateInfoOutput();
            _mapper.Map(AHotel, HotelDetail);

            var fullCateRoom = new List<HotelCateOutput>();
            _mapper.Map(result, fullCateRoom);
            HotelDetail.LsCate = fullCateRoom;

            return HotelDetail;
        }

        public async Task<IEnumerable<AHotelAllRoomStatusOutput>> GetListAHotelAllRoomStatusSearch(int userId, int hotelId, DateTime today, int totalnight)
        {
            DateTime StartDate = today.Add(new TimeSpan(14, 00, 0));
            DateTime EndDate = _other.GetEndDate(today, totalnight);
            var allRoomStatus = await _context.Rooms
                .Include(c => c.Category)
                .ThenInclude(sp => sp.SpecialPrices.Where(op => op.SpecialDate.Value.Date >= DateTime.Now.Date
                        && op.SpecialDate.Value.Date >= StartDate.Date
                        && op.SpecialDate.Value.Date <= EndDate.Date
                        && op.Status.Value))
                .Where(op => op.HotelId == hotelId)
                .ToListAsync();
            if (allRoomStatus == null) { return null; }
            var lsHotelRoomNotBooked = await _context.Rooms
                     .Include(c => c.BookingDetails).ThenInclude(b => b.Booking).ThenInclude(q => q.QrCheckUp)
                    .Where(op => op.HotelId == hotelId)
                     .Where(a => a.BookingDetails.Where(op => op.Status.Value).All(op => (
                                                  !(((op.StartDate.Value.Date > StartDate.Date && op.StartDate.Value.Date < EndDate.Date)
                                                  && (op.EndDate.Value.Date > StartDate.Date && op.EndDate.Value.Date < EndDate.Date))
                                                  || (op.StartDate.Value.Date < StartDate.Date && op.EndDate.Value.Date > EndDate.Date)
                                                  || (op.StartDate.Value.Date < EndDate.Date && op.EndDate.Value.Date > EndDate.Date)
                                                  || (op.StartDate.Value.Date < StartDate.Date && op.EndDate.Value.Date > StartDate.Date)
                                                  || (op.StartDate.Value.Date == StartDate.Date
                                                  || op.StartDate.Value.Date == EndDate.Date
                                                  || op.EndDate.Value.Date == StartDate.Date
                                                  || op.EndDate.Value.Date == EndDate.Date)
                                                  ))
                                                ))
                                    .ToListAsync();
            var returnResult = _mapper.Map<IEnumerable<AHotelAllRoomStatusOutput>>(allRoomStatus);

            foreach (var result in returnResult)
            {
                if (lsHotelRoomNotBooked.Select(s => s.RoomId).Contains(result.RoomId))
                {
                    result.AllStatus = 1;
                    result.BookedStatus = false;
                }
                else
                {
                    var checkCheckIn = await _context.Bookings
                        .Include(q => q.QrCheckUp)
                        .Where(op => op.BookingDetails.Where(op => op.RoomId == result.RoomId
                                                             && op.EndDate >= DateTime.Now.Date
                                                               && op.Status == true
                                                               ).Count() == 1)
                        .SingleOrDefaultAsync();
                    ;
                    if (checkCheckIn.QrCheckUp.Status == true)
                    {
                        result.AllStatus = 2;
                    }
                    else
                    {
                        result.AllStatus = 4;
                    }
                    result.BookedStatus = true;
                }
                if (result.CleanStatus == true)
                {
                    result.AllStatus = 3;
                }
            }
            return returnResult.Where(op => op.BookedStatus == false);
        }

        public async Task<RoomDetailInfoOutput> GetADetailRoom(int userId, int RoomId, DateTime today)
        {
            var RoomDetail = await _context.Rooms.Where(op => op.RoomId == RoomId)
                .Include(bd => bd.BookingDetails.Where(op => op.StartDate.Value.Date <= today.Date && op.EndDate.Value.Date >= today.Date && op.Status.Value)).ThenInclude(b => b.Booking).ThenInclude(ib => ib.InboundUsers)
                .Include(bd => bd.BookingDetails.Where(op => op.StartDate.Value.Date <= today.Date && op.EndDate.Value.Date >= today.Date && op.Status.Value)).ThenInclude(b => b.Booking).ThenInclude(u => u.User)
                .Include(c => c.Category)
                .SingleOrDefaultAsync()
            ;

            if (RoomDetail == null) { return null; }
            var returnResult = _mapper.Map<RoomDetailInfoOutput>(RoomDetail);
            return returnResult;
        }

        public async Task<IEnumerable<AHotelAllRoomStatusOutput>> GetListAHotelAllRoomStatusToday(int userId, int hotelId, DateTime today)
        {
            var allRoomStatus = await _context.Rooms
                .Include(c => c.Category)
                .ThenInclude(sp => sp.SpecialPrices.Where(op => op.SpecialDate.Value.Date == DateTime.Now.Date
                                                                && op.Status.Value))
                .Where(op => op.HotelId == hotelId)
                .ToListAsync();

            if (allRoomStatus == null) { return null; }
            var lsHotelRoomNotBooked = await _context.Rooms

                    .Where(op => op.HotelId == hotelId)
                     .Where(a => a.BookingDetails.Where(op => op.Status.Value).All(op => (
                                                  !(op.StartDate.Value.Date <= today.Date
                                                  && op.EndDate.Value.Date >= today.Date)
                                                  )
                                                ))
                                    .ToListAsync();
            var returnResult = _mapper.Map<IEnumerable<AHotelAllRoomStatusOutput>>(allRoomStatus);

            foreach (var result in returnResult)
            {
                if (lsHotelRoomNotBooked.Select(s => s.RoomId).Contains(result.RoomId))
                {
                    result.AllStatus = 1;
                    result.BookedStatus = false;
                }
                else
                {
                    var checkCheckIn = await _context.Bookings
                        .Include(q => q.QrCheckUp)
                        .Where(op => op.BookingDetails.Where(op=>op.RoomId == result.RoomId
                                                             && op.EndDate >= DateTime.Now.Date
                                                               && op.Status == true
                                                               ).Count()==1)
                        .SingleOrDefaultAsync();
                    ;
                    if (checkCheckIn.QrCheckUp.Status == true)
                    {
                        result.AllStatus = 2;
                    }
                    else
                    {
                        result.AllStatus = 4;
                    }
                    result.BookedStatus = true;
                }
                if (result.CleanStatus == true)
                {
                    result.AllStatus = 3;
                }
            }
            return returnResult;
        }

        public async Task<IEnumerable<AHotelAllRoomStatusOutput>> GetListAHotelAllRoomStatusCheckOut(int userId, int hotelId, DateTime today)
        {
            var allRoomStatus = await _context.Rooms
                .Include(c => c.Category)
                .ThenInclude(sp => sp.SpecialPrices.Where(op => op.SpecialDate.Value.Date == DateTime.Now.Date
                                                                && op.Status.Value))
                .Where(op => op.HotelId == hotelId)
                .ToListAsync();

            if (allRoomStatus == null) { return null; }
            var lsHotelRoomNotBooked = await _context.Rooms
                    .Where(op => op.HotelId == hotelId)
                     .Where(a => a.BookingDetails.Where(op => op.Status.Value).All(op => (
                                                    !(op.StartDate.Value.Date <= today.Date
                                                    && op.EndDate.Value.Date >= today.Date
                                                    )
                                                    )
                                                ))
                                    .ToListAsync();
            var returnResult = _mapper.Map<IEnumerable<AHotelAllRoomStatusOutput>>(allRoomStatus);

            foreach (var result in returnResult)
            {
                if (lsHotelRoomNotBooked.Select(s => s.RoomId).Contains(result.RoomId))
                {
                    result.AllStatus = 1;
                    result.BookedStatus = false;
                }
                else
                {
                    result.AllStatus = 2;
                    result.BookedStatus = true;
                }
                if (result.CleanStatus == true)
                {
                    result.AllStatus = 3;
                }
            }
            return returnResult.Where(op => op.BookedStatus == true).ToList();
        }

        public async Task<string> LocalPaymentFinal(LocalPaymentInput ppi, int userId)
        {
            try
            {
                if (ppi.ArrivalDate.Date < DateTime.Now.Date)
                {
                    error += "Wrong date ;";
                }
                Booking bok = await PaymentCalculateData(ppi, userId);
                bok.PaymentMethod = "LOCAL";
                //
                bool checkExist = await PaymentCheckRoomExist(bok);
                //
                if (checkExist)
                {
                    error += "some rooms had been booked!;";
                }
                if (error != "")
                {
                    return error;
                }
                await _context.Bookings.AddAsync(bok);
                if (await _context.SaveChangesAsync() > 0)
                {
                    var bookingFullDetail = await _context.Bookings
                       .Include(v => v.Voucher)
                       .Include(h => h.Hotel)
                       .Include(u => u.User)
                       .Include(b => b.BookingDetails).ThenInclude(r => r.Room).ThenInclude(c => c.Category)
                       .Where(a => a.BookingId == bok.BookingId).FirstOrDefaultAsync();

                    string mainQrUrl, mainQrContent;
                    var MainrandomString = _other.RandomString(6);
                    _generateqr.GetMainQrUrlContent(bookingFullDetail, MainrandomString, out mainQrContent, out mainQrUrl);

                    QrCheckUp qc = new QrCheckUp
                    {
                        BookingId = bok.BookingId,
                        QrUrl = mainQrUrl,
                        CheckIn = DateTime.Now,
                        QrCheckUpRandomString = MainrandomString,
                        Status = true,
                        QrContent = mainQrContent,
                    };

                    await _billmail.SendBillEmailAsync(bookingFullDetail, mainQrUrl);

                    await _context.QrCheckUps.AddAsync(qc);

                    var ListRoom = _mapper.Map<IEnumerable<QrInput>>(bok.BookingDetails);

                    foreach (var room in ListRoom)
                    {
                        //
                        Qr qrdetail = new();
                        //
                        string DetailQrUrl = "";
                        string DetailQrContent = "";
                        var randomString = _other.RandomString(6);
                        _generateqr.GetQrDetailUrlContent(room, randomString, out DetailQrContent, out DetailQrUrl);

                        qrdetail.QrContent = DetailQrContent;

                        qrdetail.QrUrl = DetailQrUrl;
                        qrdetail.QrRandomString = randomString;

                        await _qrmail.SendQrEmailAsync(DetailQrUrl, bok, room, bok.Hotel.HotelName);

                        //
                        qrdetail.StartDate = bok.StartDate;
                        qrdetail.EndDate = bok.EndDate;
                        _mapper.Map(room, qrdetail);
                        qrdetail.Status = true;

                        await _context.Qrs.AddAsync(qrdetail);
                    }
                    if (await _context.SaveChangesAsync() > 0)
                        return "1";
                    return "3";
                }
                return "0";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private async Task<Booking> PaymentCalculateData(LocalPaymentInput ppi, int userId)
        {
            Booking bok = new();
            _mapper.Map(ppi, bok);
            foreach (var user in ppi.InboundUsersUnknow)
            {
                var lsInfo = user.Split('|').ToList();

                bok.InboundUsers.Add(new InboundUser
                {
                    UserAddress = lsInfo[0],
                    UserBirthday = new DateTime(int.Parse(lsInfo[1].Substring(4, 4)), int.Parse(lsInfo[1].Substring(2, 2)), int.Parse(lsInfo[1].Substring(0, 2))),
                    UserIdCard = lsInfo[2],
                    UserName = lsInfo[3],
                    UserSex = lsInfo[4],
                    Status = true,
                });
            }

            IQueryable<Room> roomprice = _context.Rooms
                .Include(c => c.Category)
                .ThenInclude(sp => sp.SpecialPrices.Where(op => op.SpecialDate.Value.Date >= DateTime.Now.Date
                                    && op.SpecialDate.Value.Date >= bok.StartDate.Value.Date
                                    && op.SpecialDate.Value.Date <= bok.EndDate.Value.Date
                                    && op.Status.Value))
                .Where(op => ppi.BookingDetails.Select(s => s.RoomId).Contains(op.RoomId)
                )
                 ;

            var data = await roomprice.ToListAsync();
            bok.SubTotal = ppi.BookingDetails.Sum(s => s.TotalRoomPrice);
            bok.TotalPrice = ppi.BookingDetails.Sum(s => s.TotalRoomPrice);
            foreach (BookingDetail r in bok.BookingDetails)
            {
                //var detail = data
                //    .Where(op => op.RoomId == r.RoomId);

                //var specialDate = detail.Select(s => s.Category.SpecialPrices);
                //var sumSpecialPrice = specialDate.Select(s => s.Sum(op => op.SpecialPrice1));
                //var normalDate = bok.TotalNight - specialDate.Select(s => s.Count()).First();
                //var normalPrice = detail.Sum(op => op.RoomPrice);
                //var AveragePrice = (normalDate * normalPrice) + sumSpecialPrice.First();

                r.StartDate = bok.StartDate;
                r.EndDate = bok.EndDate;
                r.BookingDetailPrices.Add(new BookingDetailPrice
                {
                    Price = Math.Round((double)(r.AveragePrice/bok.TotalNight),2),
                    Status = true,
                });
                //var oj = specialDate.ToList()[0];
                //foreach (var item in oj)
                //{
                //    r.BookingDetailPrices.Add(new BookingDetailPrice
                //    {
                //        Date = item.SpecialDate,
                //        Price = item.SpecialPrice1,
                //        Status = true,
                //    });
                //}
            }

            bok.UserId = userId;

            if (error != "") { throw new Exception(error); }

            return bok;
        }

        private async Task<bool> PaymentCheckRoomExist(Booking bok)
        {
            IQueryable<Room> lsHotelRoomNotBooked = _context.Rooms
                    .Where(op => op.Status == true && op.HotelId == bok.HotelId)
                    .Where(a => a.BookingDetails.Where(op => op.Status.Value).All(op => (
                                                    !(((op.StartDate.Value.Date >= bok.StartDate.Value.Date && op.StartDate.Value.Date <= bok.EndDate.Value.Date)
                                                    && (op.EndDate.Value.Date >= bok.StartDate.Value.Date && op.EndDate.Value.Date <= bok.EndDate.Value.Date))
                                                    || (op.StartDate.Value.Date <= bok.StartDate.Value.Date && op.EndDate.Value.Date >= bok.EndDate.Value.Date)
                                                    || (op.StartDate.Value.Date <= bok.EndDate.Value.Date && op.EndDate.Value.Date >= bok.EndDate.Value.Date)
                                                    || (op.StartDate.Value.Date <= bok.StartDate.Value.Date && op.EndDate.Value.Date >= bok.StartDate.Value.Date)
                                                    ))
                                                ));

            var checkExist = bok.BookingDetails.Select(b => b.RoomId).ToList()
                .Any(op => !lsHotelRoomNotBooked.Select(a => a.RoomId).ToList().Contains(op));

            return checkExist;
        }

        public async Task<string> CheckRoomDateBooking(CheckRoomDateInput chek)
        {
            DateTime StartDate = chek.ArrivalDate;
            DateTime EndDate = _other.GetEndDate(chek.ArrivalDate, chek.TotalNight);

            IQueryable<Room> lsHotelRoomNotBooked = _context.Rooms
                    .Where(op => op.Status == true && op.HotelId == chek.HotelId)
                    .Where(a => a.BookingDetails.Where(op => op.Status.Value).All(op => (op.Status.Value &&
                                                  !(((op.StartDate.Value.Date >= StartDate.Date && op.StartDate.Value.Date <= EndDate.Date)
                                                  && (op.EndDate.Value.Date >= StartDate.Date && op.EndDate.Value.Date <= EndDate.Date))
                                                  || (op.StartDate.Value.Date <= StartDate.Date && op.EndDate.Value.Date >= EndDate.Date)
                                                  || (op.StartDate.Value.Date <= EndDate.Date && op.EndDate.Value.Date >= EndDate.Date)
                                                  || (op.StartDate.Value.Date <= StartDate.Date && op.EndDate.Value.Date >= StartDate.Date)
                                                  ))
                                                ));
            var error = "Room ";
            foreach (var room in chek.BookingDetails)
            {
                var checkExist = !lsHotelRoomNotBooked.Select(a => a.RoomId).ToList().Contains(room.RoomId);
                if (checkExist)
                {
                    var r = await _context.Rooms.Where(a => a.RoomId == room.RoomId && a.HotelId == chek.HotelId).FirstOrDefaultAsync();
                    if (r == null) { return "Some room not exist!"; }
                    error += r.RoomName + ", ";
                }
            }
            ;
            if (error.Equals("Room ")) { return "All Roome are avaiable!"; }
            return error + " have been booked!";
        }

        public async Task<IEnumerable<HotelListMenuOutput>> GetListMenu(int hotelID)
        {
            var returnList = await _context.Menus
                .Where(op => op.HotelId == hotelID && op.MenuStatus.Value).ToListAsync();
            return _mapper.Map<IEnumerable<HotelListMenuOutput>>(returnList).OrderBy(o => o.MenuType);
        }

        public async Task<string> CheckOutLocal(int hotelId, int BookingID)
        {
            try
            {
                var check = await _context.Bookings
                    .Include(q => q.InboundUsers)
                    .Include(q => q.QrCheckUp)
                    .Include(q => q.Hotel)
                    .Include(q => q.BookingDetails).ThenInclude(q => q.Qr)
                    .Include(q => q.BookingDetails).ThenInclude(q => q.Room)
                    .Include(q => q.BookingDetails).ThenInclude(q => q.BookingDetailMenus)
                    .Where(op => op.BookingId == BookingID && op.HotelId == hotelId)
                    .SingleOrDefaultAsync();

                if (check == null || check.QrCheckUp == null) { return "0"; }
                check.QrCheckUp.Status = false;
                check.QrCheckUp.CheckOut = DateTime.Now;
                check.Deposit = check.TotalPrice;
                await _checkoutmail.SendCheckOutBillEmailAsync(check);
                check.BookingDetails.ToList().ForEach(q => q.Qr.Status = false);
                check.Status = false;
                check.BookingDetails.ToList().ForEach(q => q.Status = false);
                check.BookingDetails.ToList().ForEach(q => q.Room.CleanStatus = true);

                if (await _context.SaveChangesAsync() > 0)
                    return "1";
                return "3";
            }catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> UpdateCleanStatus(int RoomID)
        {
            var hotelRoom = await _context.Rooms
               .Where(op => op.RoomId == RoomID && op.Status == true).SingleOrDefaultAsync();
            if (hotelRoom == null) { return "Not found Room"; }

            if (hotelRoom.CleanStatus.Value)
            {
                hotelRoom.CleanStatus = false;
            }
            else
            {
                hotelRoom.CleanStatus = true;
            }

            if (await _context.SaveChangesAsync() > 0)
                return "1";
            return "3";
        }

        public async Task<string> AddInboundUser(checkInInput checkIn)
        {
            var detail = await _context.Bookings
               .Include(b => b.InboundUsers)
               .Where(a => a.BookingId == checkIn.BookingId && a.HotelId == checkIn.HotelId && a.Status == true)
           .FirstOrDefaultAsync();

            if (detail != null)
            {
                if (detail.InboundUsers != null)
                {
                    _context.InboundUsers.RemoveRange(detail.InboundUsers);
                }
                foreach (var user in checkIn.InboundUsers)
                {
                    var lsInfo = user.Split('|').ToList();

                    detail.InboundUsers.Add(new InboundUser
                    {
                        UserAddress = lsInfo[0],
                        UserBirthday = new DateTime(int.Parse(lsInfo[1].Substring(4, 4)), int.Parse(lsInfo[1].Substring(2, 2)), int.Parse(lsInfo[1].Substring(0, 2))),
                        UserIdCard = lsInfo[2],
                        UserName = lsInfo[3],
                        UserSex = lsInfo[4],
                        Status = true,
                    });
                }
                if (await _context.SaveChangesAsync() > 0)
                    return "1";
                return "3";
            }
            return "0";
        }

        public async Task<string> AddItemForExtraFee(ICollection<ExtraFeeMenuDetailInput> ex)
        {
            try
            {
                var roomBooking = await _context.BookingDetails
                    .Include(b=>b.Booking)
                    .Include(m => m.BookingDetailMenus.Where(op => op.BookingDetailMenuStatus.Value)).ThenInclude(m => m.Menu)
                    .Where(op => op.BookingDetailId == ex.First().BookingDetailId)
                    .SingleOrDefaultAsync();
                ;
                if (roomBooking == null) { return "0"; }
                foreach (var item in ex)
                {
                    if (!roomBooking.BookingDetailMenus.Select(s => s.MenuId).ToList().Contains(item.MenuId))
                    {
                        var me = await _context.Menus.Where(op => op.MenuId == item.MenuId).FirstAsync();
                        roomBooking.BookingDetailMenus.Add(new BookingDetailMenu
                        {
                            BookingDetailMenuName = me.MenuName.ToString(),
                            BookingDetailMenuPrice = double.Parse(me.MenuPrice.ToString()),
                            BookingDetailMenuStatus = true,
                            BookingDetailMenuQuanity = item.BookingDetailMenuQuanity,
                            MenuId = item.MenuId,
                        });
                    }
                    else
                    {
                        var me = await _context.Menus.Where(op => op.MenuId == item.MenuId).FirstAsync();
                        var detail = roomBooking.BookingDetailMenus.Where(op => op.MenuId == item.MenuId && op.BookingDetailMenuPrice == me.MenuPrice && op.BookingDetailMenuName == me.MenuName);
                        if (detail.Any())
                        {
                            detail.ToList()[0].BookingDetailMenuQuanity = item.BookingDetailMenuQuanity;
                        }
                        else
                        {
                            roomBooking.BookingDetailMenus.Add(new BookingDetailMenu
                            {
                                BookingDetailMenuName = me.MenuName.ToString(),
                                BookingDetailMenuPrice = double.Parse(me.MenuPrice.ToString()),
                                BookingDetailMenuStatus = true,
                                BookingDetailMenuQuanity = item.BookingDetailMenuQuanity,
                                MenuId = item.MenuId,
                            });
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                roomBooking.Booking.SubTotal = roomBooking.Booking.SubTotal - roomBooking.ExtraFee;
                roomBooking.ExtraFee = roomBooking.BookingDetailMenus.Sum(s => s.BookingDetailMenuQuanity * s.BookingDetailMenuPrice);
                roomBooking.Booking.SubTotal = roomBooking.Booking.SubTotal + roomBooking.ExtraFee;

                roomBooking.Booking.TotalPrice = roomBooking.Booking.SubTotal;
                
                
                if (await _context.SaveChangesAsync() > 0)
                    return "1";
                return "3";
            }
            catch (Exception er)
            {
                return er.Message;
            }
        }

        public async Task<BookingDetail> GetUserMenu(int BookingDetailID)
        {
            var DetailLsMenu = await _context.BookingDetails.Include(m => m.BookingDetailMenus)
                .Where(op => op.BookingDetailId == BookingDetailID).SingleOrDefaultAsync();
            return DetailLsMenu;
        }

        public async Task<string> AddItemMenu(ICollection<ItemMenuInput> item)
        {
            var lsItem = _mapper.Map<ICollection<Menu>>(item);
            await _context.Menus.AddRangeAsync(lsItem);
            if (await _context.SaveChangesAsync() > 0)
                return "1";
            return "3";
        }

        public async Task<string> UpdateItemMenu(int MenuID, ItemMenuInput item)
        {
            var menu = await _context.Menus.Where(op => op.MenuId == MenuID).FirstAsync();
            _mapper.Map(item, menu);
            if (await _context.SaveChangesAsync() > 0)
                return "1";
            return "3";
        }

        public async Task<string> AddProblemForExtraFee(ICollection<ProblemExtraFeeInput> prEx)
        {
            try
            {
                var roomBooking = await _context.BookingDetails
                    .Include(m=>m.Booking)
                    .Include(m => m.BookingDetailMenus.Where(op => op.BookingDetailMenuStatus.Value)).ThenInclude(m => m.Menu)
                    .Where(op => op.BookingDetailId == prEx.First().BookingDetailId)
                    .SingleOrDefaultAsync();
                ;
                if (roomBooking == null) { return "0"; }
                foreach (var item in prEx)
                {
                    roomBooking.BookingDetailMenus.Add(new BookingDetailMenu
                    {
                        BookingDetailMenuName = item.ProblemDetailMenuName,
                        BookingDetailMenuPrice = item.Price,
                        BookingDetailMenuStatus = true,
                        BookingDetailMenuQuanity = 1,
                    });

                    await _context.SaveChangesAsync();
                }
                roomBooking.Booking.SubTotal = roomBooking.Booking.SubTotal - roomBooking.ExtraFee;
                roomBooking.ExtraFee = roomBooking.BookingDetailMenus.Sum(s => s.BookingDetailMenuQuanity * s.BookingDetailMenuPrice);
                roomBooking.Booking.SubTotal = roomBooking.Booking.SubTotal + roomBooking.ExtraFee;

                roomBooking.Booking.TotalPrice = roomBooking.Booking.SubTotal;

                if (await _context.SaveChangesAsync() > 0)
                    return "1";
                return "3";
            }
            catch (Exception er)
            {
                return er.Message;
            }
        }

        public async Task<string> DeleteItemForExtraFee(int BookingDetailId, int BookingDetailMenuId)
        {
            var item = await _context.BookingDetailMenus
                .Where(op => op.BookingDetailMenuId == BookingDetailMenuId).SingleOrDefaultAsync();
            if (item == null) { return "0"; }
            _context.BookingDetailMenus.Remove(item);
            var roomBooking = await _context.BookingDetails
                .Include(m => m.BookingDetailMenus.Where(op => op.BookingDetailMenuStatus.Value))
                .Where(op => op.BookingDetailId == BookingDetailId).SingleOrDefaultAsync();
            if (await _context.SaveChangesAsync() > 0)
            {
                roomBooking.ExtraFee = roomBooking.BookingDetailMenus.Sum(s => s.BookingDetailMenuQuanity * s.BookingDetailMenuPrice);
                if (await _context.SaveChangesAsync() > 0)
                    return "1";
                return "3";
            }
            return "0";
        }

        public async Task<IEnumerable<AHotelAllRoomStatusOutput>> GetListAHotelOnlyRoomStatus13Search(int userId, int hotelId, DateTime today, int totalnight)
        {
            DateTime StartDate = today.Add(new TimeSpan(14, 00, 0));
            DateTime EndDate = _other.GetEndDate(today, totalnight);
            var allRoomStatus = await _context.Rooms
                .Include(c => c.Category)
                .ThenInclude(sp => sp.SpecialPrices.Where(op => op.SpecialDate.Value.Date >= DateTime.Now.Date
                        && op.SpecialDate.Value.Date >= StartDate.Date
                        && op.SpecialDate.Value.Date <= EndDate.Date
                        && op.Status.Value))
                .Where(op => op.HotelId == hotelId)
                .ToListAsync();
            if (allRoomStatus == null) { return null; }
            var lsHotelRoomNotBooked = await _context.Rooms
                    .Where(op => op.HotelId == hotelId)
                     .Where(a => a.BookingDetails.Where(op => op.Status.Value).All(op => (
                                                  !(((op.StartDate.Value.Date > StartDate.Date && op.StartDate.Value.Date < EndDate.Date)
                                                  && (op.EndDate.Value.Date > StartDate.Date && op.EndDate.Value.Date < EndDate.Date))
                                                  || (op.StartDate.Value.Date < StartDate.Date && op.EndDate.Value.Date > EndDate.Date)
                                                  || (op.StartDate.Value.Date < EndDate.Date && op.EndDate.Value.Date > EndDate.Date)
                                                  || (op.StartDate.Value.Date < StartDate.Date && op.EndDate.Value.Date > StartDate.Date)
                                                  || (op.StartDate.Value.Date == StartDate.Date
                                                  || op.StartDate.Value.Date == EndDate.Date
                                                  || op.EndDate.Value.Date == StartDate.Date
                                                  || op.EndDate.Value.Date == EndDate.Date)
                                                  ))
                                                ))
                                    .ToListAsync();
            var returnResult = _mapper.Map<IEnumerable<AHotelAllRoomStatusOutput>>(allRoomStatus);

            foreach (var result in returnResult)
            {
                if (lsHotelRoomNotBooked.Select(s => s.RoomId).Contains(result.RoomId))
                {
                    result.AllStatus = 1;
                    result.BookedStatus = false;
                }
                else
                {
                    var checkCheckIn = await _context.Bookings
                        .Include(q => q.QrCheckUp)
                        .Where(op => op.BookingDetails.Where(op => op.RoomId == result.RoomId
                                                             && op.EndDate >= DateTime.Now.Date
                                                               && op.Status == true
                                                               ).Count() == 1)
                        .SingleOrDefaultAsync();
                    ;
                    if (checkCheckIn.QrCheckUp.Status == true)
                    {
                        result.AllStatus = 2;
                    }
                    else
                    {
                        result.AllStatus = 4;
                    }
                    result.BookedStatus = true;
                }
                if (result.CleanStatus == true)
                {
                    result.AllStatus = 3;
                }
            }
            return returnResult.Where(op => op.BookedStatus == false);
        }

        public async Task<IEnumerable<NewInboundUser>> GetAllInboundUserBookingInfo(int hotelId)
        {
            var CustomerInfo = await _context.InboundUsers
                    .Where(op => op.Booking.HotelId == hotelId && op.Booking.Status.Value)
                     .Where(a => a.Booking.BookingDetails.Where(op => op.Status.Value).All(op => (op.EndDate.Value.Date > DateTime.Today.Date &&
                                                    (op.StartDate.Value.Date <= DateTime.Now.Date
                                                    && op.EndDate.Value.Date >= DateTime.Now.Date
                                                    )
                                                    )
                                                ))
                                    .ToListAsync();

            var returnResult = _mapper.Map<IEnumerable<NewInboundUser>>(CustomerInfo);
            return returnResult;
        }

        public async Task<FullRoomMoneyDetailSumaryOutput> GetFullRoomMoneyDetailByFilter(int hotelID, DateTime startDate, DateTime endDate)
        {
            var lsHotelBooked = await _context.Hotels
                .Include(tbl => tbl.Bookings
                .Where(op=>op.QrCheckUp.CheckOut != null && op.QrCheckUp.Status == false)
                .Where(op => op.QrCheckUp.CheckOut.Value.Date >= startDate.Date && op.QrCheckUp.CheckOut.Value.Date <= endDate.Date)
                 )
                .Include(tbl => tbl.Bookings).ThenInclude(tbl => tbl.InboundUsers)
                .Include(tbl => tbl.Bookings).ThenInclude(tbl => tbl.QrCheckUp)
                .Include(tbl => tbl.Bookings).ThenInclude(tbl => tbl.BookingDetails).ThenInclude(tbl => tbl.BookingDetailMenus)
                .Where(op => op.HotelId == hotelID)
                .SingleOrDefaultAsync();
            var returnLs = _mapper.Map<FullRoomMoneyDetailSumaryOutput>(lsHotelBooked);

            return returnLs;
        }

        public async Task<FullRoomMoneyDetailSumaryOutput> GetFullRoomMoneyNotCheckOutDetailByDate(int hotelId, DateTime startDate, DateTime endDate)
        {
            var lsHotelBooked = await _context.Hotels
                .Include(tbl => tbl.Bookings
                .Where(op => op.QrCheckUp.CheckOut == null )
                .Where(op => op.QrCheckUp.CheckIn.Value.Date >= startDate.Date && op.QrCheckUp.CheckIn.Value.Date <= endDate.Date)
                 )
                .Include(tbl => tbl.Bookings).ThenInclude(tbl => tbl.InboundUsers)
                .Include(tbl => tbl.Bookings).ThenInclude(tbl => tbl.QrCheckUp)
                .Include(tbl => tbl.Bookings).ThenInclude(tbl => tbl.BookingDetails).ThenInclude(tbl => tbl.BookingDetailMenus)
                .Where(op => op.HotelId == hotelId)
                .SingleOrDefaultAsync();
            var returnLs = _mapper.Map<FullRoomMoneyDetailSumaryOutput>(lsHotelBooked);
            returnLs.TotalPriceByfilter = returnLs.Bookings.Sum(s => s.TotalPrice);
            return returnLs;
        }

        public async Task<Pagination<Booking>> HostgetListBookingByPage<T>(int hotelID,int CurrentPage, int pageSize) where T : class
        {
            IQueryable<Booking> lsBooks =  _context.Bookings
                .Include(tbl=>tbl.QrCheckUp)
                .Where(op => op.HotelId == hotelID)
                .OrderByDescending(o=>o.CreateDate);

            var returnLS = await _pagination.GetPagination(lsBooks,CurrentPage,pageSize);
            return returnLS;
        }

        public async Task<ABookingFullOutput> HostGetABookingFullDetail(int bookingID)
        {
            var fullDetailBooking = await _context.Bookings
                .Include(tbl => tbl.InboundUsers)
                .Include(tbl => tbl.QrCheckUp)
                .Include(tbl => tbl.Voucher)
                .Include(tbl => tbl.BookingDetails).ThenInclude(tbl=>tbl.BookingDetailMenus.Where(op=>op.BookingDetailMenuQuanity>0))
                .Include(tbl => tbl.BookingDetails).ThenInclude(tbl=>tbl.Qr)
                .Where(op=>op.BookingId==bookingID)
                .SingleOrDefaultAsync();
            var returnLs =_mapper.Map<ABookingFullOutput>(fullDetailBooking);
            return returnLs;
        }

      
    }
}