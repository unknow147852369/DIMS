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
                .ForMember(a => a.Id, option => option.MapFrom(tbl => tbl.Id))
                .ForMember(a => a.Name, option => option.MapFrom(tbl => tbl.Name))
                .ForMember(a => a.Type, option => option.MapFrom(tbl => tbl.Type))
                ;
            CreateMap<Hotel, SearchLocationHotelOutput>()
                .ForMember(a => a.HotelId, option => option.MapFrom(tbl => tbl.HotelId))
                .ForMember(a => a.HotelName, option => option.MapFrom(tbl => tbl.HotelName))
                .ForMember(a => a.HotelAddress, option => option.MapFrom(tbl => tbl.HotelAddress))
                ;
            //
            CreateMap<Voucher, VoucherInfoOutput>()
                .ForMember(a => a.HotelName, option => option.MapFrom(tbl => tbl.Hotel.HotelName))
                ;
            //
            CreateMap<PaymentProcessingInput, Booking>()
                .ForMember(a => a.BookingDetails, option => option.MapFrom(tbl => tbl.BookingDetails))
                .ForMember(a => a.Status, option => option.MapFrom(tbl => 1))
                .ForMember(a => a.CreateDate, option => option.MapFrom(tbl => DateTime.Now))
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
            CreateMap<BookingDetail, InboundUser>();
            //

            CreateMap<NewOtpInput, Otp>();

            //
            CreateMap<Room, HotelCateOutput>()
                .ForMember(a => a.Photos, option => option.MapFrom(tbl => tbl.Category.Photos))
                .ForMember(a => a.Rooms, option => option.MapFrom(tbl => tbl.Category.Rooms))
                .ForMember(a => a.CategoryName, option => option.MapFrom(tbl => tbl.Category.CategoryName))
                .ForMember(a => a.Quanity, option => option.MapFrom(tbl => tbl.Category.Quanity))
                .ForMember(a => a.CateDescrpittion, option => option.MapFrom(tbl => tbl.Category.CateDescrpittion))
                ;
            CreateMap<Room, HotelCateRoomOutput>()
                ;
            CreateMap<Photo, HotelCatePhotosOutput>()
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
                .ForMember(a => a.WardName, option => option.MapFrom(tbl => tbl.WardNavigation.Name))
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
                .ForMember(a => a.Price, option => option.MapFrom(tbl => tbl.Price))
                .ForMember(a => a.StartDate, option => option.MapFrom(tbl => tbl.StartDate))
                .ForMember(a => a.EndDate, option => option.MapFrom(tbl => tbl.EndDate))
                .ForMember(a => a.Status, option => option.MapFrom(tbl => 0))
                ;

            //

            //
            CreateMap<Qr, QrOutput>()
                .ForMember(a => a.QrStringImage, option => option.MapFrom(tbl => Encoding.UTF8.GetString(tbl.QrString)))
                ;

            CreateMap<BookingDetail, QrInput>()
                .ForMember(a => a.UserId, option => option.MapFrom(tbl => tbl.Booking.UserId))
                .ForMember(a => a.RoomName, option => option.MapFrom(tbl => tbl.Room.RoomName))
                .ForMember(a => a.HotelId, option => option.MapFrom(tbl => tbl.Booking.HotelId))
                ;
            CreateMap<QrInput, Qr>()
                .ForMember(a => a.Status, option => option.MapFrom(tbl => 0))
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
                .ForMember(a => a.RoomPrice, option => option.MapFrom(tbl => tbl.Room.Price))
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