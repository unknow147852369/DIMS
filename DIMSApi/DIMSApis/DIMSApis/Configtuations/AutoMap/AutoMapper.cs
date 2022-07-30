using AutoMapper;
using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;

namespace DIMSApis.Configtuations.AutoMap
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<NewCategorySpecialPriceSecondInput,SpecialPrice >()
                .ForMember(a => a.Status, option => option.MapFrom(tbl => 1))
                .ForMember(a => a.SpecialDate, option => option.MapFrom(tbl => tbl.SpecialDate.Value.Date))
                ;
            CreateMap<NewCategorySpecialPriceUpdateInput,SpecialPrice>()
                ;
            //
            CreateMap<Booking, ABookingFullOutput>()
                .ForMember(a => a.BookingDetails, option => option.MapFrom(tbl => tbl.BookingDetails))
                .ForMember(a => a.Voucher, option => option.MapFrom(tbl => tbl.Voucher))
                .ForMember(a => a.QrCheckUp, option => option.MapFrom(tbl => tbl.QrCheckUp))
                .ForMember(a => a.InboundUsers, option => option.MapFrom(tbl => tbl.InboundUsers))
                ;
            CreateMap<QrCheckUp, ABookingFullQRcheckUpOutput>()
                ;
            CreateMap<InboundUser, ABookingFullInboundUSersOutput>()
                ;
            CreateMap<Voucher, ABookingFullVoucher>()
                ;
            CreateMap<BookingDetail, ABookingFullBookingDetailsOutput>()
                .ForMember(a => a.BookingDetailMenus, option => option.MapFrom(tbl => tbl.BookingDetailMenus))
                .ForMember(a => a.Qr, option => option.MapFrom(tbl => tbl.Qr))
                ;
            CreateMap<BookingDetailMenu, ABookingFullBookingDetailMenuOutput>()
                ;
            CreateMap<Qr, ABookingFullBookingDetailQrsOutput>()
                ;
            //
            CreateMap<Hotel,FullRoomMoneyDetailSumaryOutput>()
                .ForMember(a => a.TotalPriceByfilter, option => option.MapFrom(tbl => tbl.Bookings.Sum(s=>s.Deposit)))
                .ForMember(a => a.Bookings, option => option.MapFrom(tbl => tbl.Bookings))
                ;
            CreateMap<Booking, FullRoomMoneyDetailFirstOutput>()
                .ForMember(a => a.BookingDetails, option => option.MapFrom(tbl => tbl.BookingDetails))
                .ForMember(a => a.InboundUsers, option => option.MapFrom(tbl => tbl.InboundUsers))
                ;
            CreateMap<InboundUser, FullRoomMoneyDetailThirdOutput>()
                ;
            CreateMap<BookingDetail, FullRoomMoneyDetailSecondOutput>()
                .ForMember(a => a.BookingDetailMenus, option => option.MapFrom(tbl => tbl.BookingDetailMenus))
                ;
            CreateMap<BookingDetailMenu, FullRoomMoneyDetailMenusOutput>()
                ;
            //
            CreateMap<AhotelVoucherCreate, Voucher>()
                .ForMember(a => a.Status, option => option.MapFrom(tbl => 1))
                ;
            CreateMap<AHotelVouchersInput, Voucher>()
                ;
            //
            CreateMap<HotelRequestUpdateInput, HotelRequest>()
                .ForMember(a => a.PendingStatus, option => option.MapFrom(tbl => "PENDING"))
                .ForMember(a => a.CreateDate, option => option.MapFrom(tbl => DateTime.Now))
                .ForMember(a => a.HotelRequestStatus, option => option.MapFrom(tbl => 1))
                ;
            //
            CreateMap<HotelRequestAddInput,HotelRequest>()
                 .ForMember(a => a.PendingStatus, option => option.MapFrom(tbl => "PENDING"))
                 .ForMember(a => a.CreateDate, option => option.MapFrom(tbl => DateTime.Now))
                 .ForMember(a => a.HotelRequestStatus, option => option.MapFrom(tbl =>1))
                ;
            //
            CreateMap<NewUpdateRoomInput, Room>()
                .ForMember(a => a.CleanStatus, option => option.MapFrom(tbl => 0))
                .ForMember(a => a.HideStatus, option => option.MapFrom(tbl => 0))
            ;
            //
            CreateMap<NewRoomSecondInput, Room>()
                .ForMember(a => a.CleanStatus, option => option.MapFrom(tbl => 0))
                .ForMember(a => a.HideStatus, option => option.MapFrom(tbl => 0))
                .ForMember(a => a.Status, option => option.MapFrom(tbl => 1))
               ;
            //
            CreateMap<NewUrlPhotoOnlyInput, Photo>()
                .ForMember(a => a.CreateDate, option => option.MapFrom(tbl => DateTime.Now))
                .ForMember(a => a.IsMain, option => option.MapFrom(tbl => 0))
                .ForMember(a => a.Status, option => option.MapFrom(tbl => 1))
               ;
            //
            CreateMap<NewUpdateHotelCateInput, Category>()
               ;
            //
            CreateMap<NewHotelCateInpput, Category>()
                .ForMember(a => a.Status, option => option.MapFrom(tbl => 1))
                ;
            //
            CreateMap<NewHotelPhotosInput, Photo>()
                .ForMember(a => a.Status, option => option.MapFrom(tbl => 1))
                .ForMember(a => a.IsMain, option => option.MapFrom(tbl => 0))
                .ForMember(a => a.CreateDate, option => option.MapFrom(tbl => DateTime.Now))
                ;
            //
            CreateMap<InboundUser, NewInboundUser>();
            //
            CreateMap<ItemMenuInput, Menu>();
            //
            CreateMap<Menu, HotelListMenuOutput>();
            //
            CreateMap<LocalPaymentInput, Booking>()
                .ForMember(a => a.BookingDetails, option => option.MapFrom(tbl => tbl.BookingDetails))
                .ForMember(a => a.Status, option => option.MapFrom(tbl => 1))
                .ForMember(a => a.CreateDate, option => option.MapFrom(tbl => DateTime.Now))
                .ForMember(a => a.StartDate, option => option.MapFrom(tbl => tbl.ArrivalDate.Date.Add(new TimeSpan(14, 00, 0))))
                .ForMember(a => a.EndDate, option => option.MapFrom(tbl => tbl.ArrivalDate.Date.AddDays((double)(tbl.TotalNight)).Add(new TimeSpan(12, 00, 0))));
                ;
            CreateMap<LocalPaymentBookingdetailInput, BookingDetail>()
                .ForMember(a => a.AveragePrice, option => option.MapFrom(tbl => tbl.TotalRoomPrice))
                .ForMember(a => a.Status, option => option.MapFrom(tbl => 1))
                ;
            //
            CreateMap<Room, RoomDetailInfoOutput>()
                .ForMember(a => a.LsCustomer, option => option.MapFrom(tbl => tbl.BookingDetails.Count() == 0 ? null : tbl.BookingDetails.First().Booking.InboundUsers))
                .ForMember(a => a.CategoryName, option => option.MapFrom(tbl => tbl.Category.CategoryName))
                .ForMember(a => a.UserFullName, option => option.MapFrom(tbl => tbl.BookingDetails.Count() == 0 ? null : tbl.BookingDetails.First().Booking.User.UserName))
                .ForMember(a => a.Role, option => option.MapFrom(tbl => tbl.BookingDetails.Count() == 0 ? null : tbl.BookingDetails.First().Booking.User.Role))
                .ForMember(a => a.BookingId, option => option.MapFrom(tbl => tbl.BookingDetails.Count() == 0 ? -1 : tbl.BookingDetails.First().Booking.BookingId))
                .ForMember(a => a.BookingDetailId, option => option.MapFrom(tbl => tbl.BookingDetails.Count() == 0 ? -1 : tbl.BookingDetails.Select(s => s.BookingDetailId).First()))
                .ForMember(a => a.FullName, option => option.MapFrom(tbl => tbl.BookingDetails.Count() == 0 ? null : tbl.BookingDetails.First().Booking.FullName))
                .ForMember(a => a.Email, option => option.MapFrom(tbl => tbl.BookingDetails.Count() == 0 ? null : tbl.BookingDetails.First().Booking.Email))
                .ForMember(a => a.PhoneNumber, option => option.MapFrom(tbl => tbl.BookingDetails.Count() == 0 ? null : tbl.BookingDetails.First().Booking.PhoneNumber))
                .ForMember(a => a.StartDate, option => option.MapFrom(tbl => tbl.BookingDetails.Count() == 0 ? null : tbl.BookingDetails.First().Booking.StartDate))
                .ForMember(a => a.EndDate, option => option.MapFrom(tbl => tbl.BookingDetails.Count() == 0 ? null : tbl.BookingDetails.First().Booking.EndDate))
                .ForMember(a => a.TotalPrice, option => option.MapFrom(tbl => tbl.BookingDetails.Count() == 0 ? null : tbl.BookingDetails.First().Booking.TotalPrice))
                .ForMember(a => a.CreateDate, option => option.MapFrom(tbl => tbl.BookingDetails.Count() == 0 ? null : tbl.BookingDetails.First().Booking.CreateDate))
                .ForMember(a => a.PaymentMethod, option => option.MapFrom(tbl => tbl.BookingDetails.Count() == 0 ? null : tbl.BookingDetails.First().Booking.PaymentMethod))
                .ForMember(a => a.PaymentCondition, option => option.MapFrom(tbl => tbl.BookingDetails.Count() == 0 ? null : tbl.BookingDetails.First().Booking.PaymentCondition))
                .ForMember(a => a.Deposit, option => option.MapFrom(tbl => tbl.BookingDetails.Count() == 0 ? null : tbl.BookingDetails.First().Booking.Deposit))
                ;
            CreateMap<InboundUser, RoomDetailInfoPeopleOutput>()
                ;
            //
            CreateMap<Room, AHotelAllRoomStatusOutput>()
                .ForMember(a => a.CategoryName, option => option.MapFrom(tbl => tbl.Category.CategoryName))
                ;
            //
            CreateMap<Province, SearchLocationAreaOutput>()
                ;
            CreateMap<District, SearchLocationAreaOutput>()
                ;
            CreateMap<Hotel, SearchLocationHotelOutput>()
                ;

            CreateMap<Voucher, VoucherInfoOutput>()
                .ForMember(a => a.HotelName, option => option.MapFrom(tbl => tbl.Hotel.HotelName))
                ;
            //

            CreateMap<PaymentProcessingInput, Booking>()
                .ForMember(a => a.BookingDetails, option => option.MapFrom(tbl => tbl.BookingDetails))
                .ForMember(a => a.Status, option => option.MapFrom(tbl => 1))
                .ForMember(a => a.CreateDate, option => option.MapFrom(tbl => DateTime.Now))
                .ForMember(a => a.StartDate, option => option.MapFrom(tbl => tbl.ArrivalDate.Date.Add(new TimeSpan(14, 00, 0))))
                .ForMember(a => a.EndDate, option => option.MapFrom(tbl => tbl.ArrivalDate.Date.AddDays((double)(tbl.TotalNight)).Add(new TimeSpan(12, 00, 0))));
            ;
            CreateMap<PaymentProcessingDetailInput, BookingDetail>()
                ;
            //
            CreateMap<Hotel, AHotelOutput>()
                .ForMember(a => a.Ward, option => option.MapFrom(tbl => tbl.WardNavigation.Name))
                .ForMember(a => a.Province, option => option.MapFrom(tbl => tbl.ProvinceNavigation.Name))
                .ForMember(a => a.District, option => option.MapFrom(tbl => tbl.DistrictNavigation.Name))
                .ForMember(a => a.Categories, option => option.MapFrom(tbl => tbl.Categories))
                .ForMember(a => a.Photos, option => option.MapFrom(tbl => tbl.Photos))
                .ForMember(a => a.Menus, option => option.MapFrom(tbl => tbl.Menus))
                .ForMember(a => a.Vouchers, option => option.MapFrom(tbl => tbl.Vouchers))
                ;
            CreateMap<Photo, AHotelPhotosOutput>();
            CreateMap<Menu, AHotelMenuOutput>();
            CreateMap<Voucher, AHotelVouchersOutput>();

            //
            CreateMap<NewInboundUser, InboundUser>()
                .ForMember(a => a.Status, option => option.MapFrom(tbl => 1))
                ;

            //

            CreateMap<NewOtpInput, Otp>();

            //
            CreateMap<Category, HotelCateOutput>()
                .ForMember(a => a.CatePhotos, option => option.MapFrom(tbl => tbl.Photos))
                .ForMember(a => a.SpecialPPrice, option => option.MapFrom(tbl => tbl.SpecialPrices))
                ;

            CreateMap<Room, HotelCateRoomOutput>()

                ;

            CreateMap<Photo, HotelCatePhotosOutput>()
                ;
            CreateMap<SpecialPrice, HotelCateSpecialPricesOutput>()
                ;
            CreateMap<Voucher, HotelCateInfoVouchersOutput>()
                ;
            CreateMap<Hotel, HotelCateInfoOutput>()
                .ForMember(a => a.WardName, option => option.MapFrom(tbl => tbl.WardNavigation.Name))
                .ForMember(a => a.ProvinceName, option => option.MapFrom(tbl => tbl.ProvinceNavigation.Name))
                .ForMember(a => a.DistrictName, option => option.MapFrom(tbl => tbl.DistrictNavigation.Name))
                .ForMember(a => a.HotelTypeName, option => option.MapFrom(tbl => tbl.HotelType.HotelTypeName))
                .ForMember(a => a.Vouchers, option => option.MapFrom(tbl => tbl.Vouchers))
                ;
            CreateMap<HotelCateOutput, HotelCateInfoOutput>()
                 .ForMember(a => a.LsCate, option => option.MapFrom(tbl => tbl))
                ;
            //
            CreateMap<Room, HotelRoomOutput>()
                .ForMember(a => a.Photos, option => option.MapFrom(tbl => tbl.Category.Photos))
                .ForMember(a => a.CategoryName, option => option.MapFrom(tbl => tbl.Category.CategoryName))
                .ForMember(a => a.Quanity, option => option.MapFrom(tbl => tbl.Category.Quanity))
                .ForMember(a => a.CateDescrpittion, option => option.MapFrom(tbl => tbl.Category.CateDescrpittion))
                ;

            CreateMap<Photo, HotelRoomPhotosOutput>()
                ;
            //
            CreateMap<Hotel, HotelOutput>()
                .ForMember(a => a.TotalRoom, option => option.MapFrom(tbl => tbl.Rooms.Count))
                .ForMember(a => a.HotelTypeName, option => option.MapFrom(tbl => tbl.HotelType.HotelTypeName))
                .ForMember(a => a.WardName, option => option.MapFrom(tbl => tbl.WardNavigation.Name))
                .ForMember(a => a.SmallPrice, option => option.MapFrom(tbl => tbl.Rooms.Min(m => m.RoomPrice)))
                .ForMember(a => a.ProvinceName, option => option.MapFrom(tbl => tbl.ProvinceNavigation.Name))
                .ForMember(a => a.DistrictName, option => option.MapFrom(tbl => tbl.DistrictNavigation.Name))
                ;
            CreateMap<Photo, HotelPhotosOutput>();

            //
            CreateMap<NewRoomInput, Room>()
                .ForMember(a => a.Status, option => option.MapFrom(tbl => 1))
                ;
            //
            CreateMap<UserUpdateInput, User>();
            CreateMap<User, UserInfoOutput>()
                .ForMember(a => a.Age, condition => condition.MapFrom(d => CalculateAge(d.Birthday)))
                ;
            //
            CreateMap<BookingInput, Booking>()
                .ForMember(a => a.TotalPrice, option => option.MapFrom(tbl => CalculateTotalPrice(tbl.BookingDetails) * (tbl.EndDate - tbl.StartDate).Value.TotalDays))
                .ForMember(a => a.Status, option => option.MapFrom(tbl => 1))
                .ForMember(a => a.CreateDate, option => option.MapFrom(tbl => DateTime.Now))
                .ForMember(a => a.PaymentMethod, option => option.MapFrom(tbl => "WAIT"))
                ;

            CreateMap<BookingDetailInput, BookingDetail>()
                .ForMember(a => a.RoomId, option => option.MapFrom(tbl => tbl.RoomId))
                //.ForMember(a => a.a, option => option.MapFrom(tbl => tbl.Price))
                .ForMember(a => a.StartDate, option => option.MapFrom(tbl => tbl.StartDate))
                .ForMember(a => a.EndDate, option => option.MapFrom(tbl => tbl.EndDate))
                .ForMember(a => a.Status, option => option.MapFrom(tbl => 0))
                ;

            //

            //
            CreateMap<Qr, QrOutput>()
                ;
            //
            CreateMap<BookingDetail, QrInput>()
                .ForMember(a => a.UserId, option => option.MapFrom(tbl => tbl.Booking.UserId))
                .ForMember(a => a.RoomName, option => option.MapFrom(tbl => tbl.Room.RoomName))
                .ForMember(a => a.HotelId, option => option.MapFrom(tbl => tbl.Booking.HotelId))
                ;
            CreateMap<QrInput, Qr>()
                .ForMember(a => a.Status, option => option.MapFrom(tbl => false))
                ;
            //
            CreateMap<Booking, BookingInfoOutput>()
                .ForMember(a => a.Qrcheckup, option => option.MapFrom(tbl => tbl.QrCheckUp))
                .ForMember(a => a.HotelAddress, option => option.MapFrom(tbl => tbl.Hotel.HotelAddress))
                .ForMember(a => a.HotelName, option => option.MapFrom(tbl => tbl.Hotel.HotelName))
                .ForMember(a => a.HotelPhotos, option => option.MapFrom(tbl => tbl.Hotel.Photos))
                .ForMember(a => a.TotalDate, option => option.MapFrom(tbl => (tbl.EndDate - tbl.StartDate).Value.TotalDays))
                ;
            CreateMap<Photo, HotelPhotosOutput>();
            CreateMap<BookingDetail, BookingDetailInfoOutput>()
                .ForMember(a => a.CategoryName, option => option.MapFrom(tbl => tbl.Room.Category.CategoryName))
                .ForMember(a => a.RoomName, option => option.MapFrom(tbl => tbl.Room.RoomName))
                .ForMember(a => a.RoomPrice, option => option.MapFrom(tbl => tbl.Room.RoomPrice))
                .ForMember(a => a.CategoryId, option => option.MapFrom(tbl => tbl.Room.CategoryId))
                ;
        }

        private int CalculateAge(DateTime? theDateTime)
        {
            if (theDateTime != null)
            {
                DateTime date = (DateTime)theDateTime;
                var age = DateTime.Today.Year - date.Year;
                if (date.AddYears(age) > DateTime.Today)
                    age--;
                return age;
            }
            return 0;
        }

        private Double CalculateTotalPrice(IEnumerable<BookingDetailInput> price)
        {
            Double totalPrice = 0;
            if (price != null)
            {
                foreach (BookingDetailInput input in price)
                {
                    totalPrice = (double)(totalPrice + input.Price);
                }
                return totalPrice;
            }
            else
            {
                return totalPrice;
            }
        }
    }
}