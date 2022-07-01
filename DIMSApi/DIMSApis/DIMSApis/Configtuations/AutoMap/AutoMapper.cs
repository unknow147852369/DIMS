using AutoMapper;
using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;
using System.Text;

namespace DIMSApis.Configtuations.AutoMap
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
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
                .ForMember(a => a.StartDate, option => option.MapFrom(tbl => tbl.ArrivalDate.Date.Add(new TimeSpan(12, 00, 0))))
                .ForMember(a => a.EndDate, option => option.MapFrom(tbl => tbl.ArrivalDate.Date.AddDays((double)(tbl.TotalNight - 1)).Add(new TimeSpan(16, 00, 0))));
                ;
            CreateMap<PaymentProcessingDetailInput, BookingDetail>()
                ;
            //
            CreateMap<Hotel, AHotelOutput>()
                .ForMember(a => a.TotalRoom, option => option.MapFrom(tbl => tbl.Rooms.Count))
                .ForMember(a => a.WardName, option => option.MapFrom(tbl => tbl.WardNavigation.Name))
                .ForMember(a => a.ProvinceName, option => option.MapFrom(tbl => tbl.ProvinceNavigation.Name))
                .ForMember(a => a.DistrictName, option => option.MapFrom(tbl => tbl.DistrictNavigation.Name))
                ;
            CreateMap<Room, AllRoomOutput>()
                .ForMember(a => a.CategoryName, option => option.MapFrom(tbl => tbl.Category.CategoryName))
                .ForMember(a => a.Quanity, option => option.MapFrom(tbl => tbl.Category.Quanity))
                ;
            //
            CreateMap<NewInboundUser, InboundUser>();


            //

            CreateMap<NewOtpInput, Otp>();

            //
            CreateMap<Category, HotelCateOutput>()
                .ForMember(a => a.CatePhotos, option => option.MapFrom(tbl => tbl.Photos))
                ;

            CreateMap<Room, HotelCateRoomOutput>()
                ;
   
            CreateMap<Photo, HotelCatePhotosOutput>()
                ;
            CreateMap<Hotel, HotelCateInfoOutput>()
                .ForMember(a => a.WardName, option => option.MapFrom(tbl => tbl.WardNavigation.Name))
                .ForMember(a => a.ProvinceName, option => option.MapFrom(tbl => tbl.ProvinceNavigation.Name))
                .ForMember(a => a.DistrictName, option => option.MapFrom(tbl => tbl.DistrictNavigation.Name))
                .ForMember(a => a.HotelTypeName, option => option.MapFrom(tbl => tbl.HotelType.HotelTypeName))
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
                //.ForMember(a => a.SmallPrice, option => option.MapFrom(tbl => tbl.Rooms.Min(r => r.Price)))
                .ForMember(a => a.ProvinceName, option => option.MapFrom(tbl => tbl.ProvinceNavigation.Name))
                .ForMember(a => a.DistrictName, option => option.MapFrom(tbl => tbl.DistrictNavigation.Name))
                ;
            CreateMap<Photo, HotelPhotosOutput>();

            //
            CreateMap<HotelInput, Hotel>()
                .ForMember(a => a.CreateDate, option => option.MapFrom(tbl => DateTime.Now))
                .ForMember(a => a.Status, option => option.MapFrom(tbl => 0))
                ;
            CreateMap<Hotel, Photo>()
                 .ForMember(a => a.HotelId, option => option.MapFrom(tbl => tbl.HotelId))
                ;
            CreateMap<PhotosInput, Photo>()
                .ForMember(a => a.CreateDate, option => option.MapFrom(tbl => DateTime.Now))
                .ForMember(a => a.Status, option => option.MapFrom(tbl => 1))
                ;
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
                .ForMember(a => a.Condition, option => option.MapFrom(tbl => "WAIT"))
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
                .ForMember(a => a.HotelAddress, option => option.MapFrom(tbl => tbl.Hotel.HotelAddress))
                .ForMember(a => a.HotelName, option => option.MapFrom(tbl => tbl.Hotel.HotelName))
                .ForMember(a => a.TotalDate, option => option.MapFrom(tbl => (tbl.EndDate - tbl.StartDate).Value.TotalDays))
                ;

            CreateMap<BookingDetail, BookingDetailInfoOutput>()
                .ForMember(a => a.CategoryName, option => option.MapFrom(tbl => tbl.Room.Category.CategoryName))
                .ForMember(a => a.RoomName, option => option.MapFrom(tbl => tbl.Room.RoomName))
                //.ForMember(a => a.RoomPrice, option => option.MapFrom(tbl => tbl.Room.Price))
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