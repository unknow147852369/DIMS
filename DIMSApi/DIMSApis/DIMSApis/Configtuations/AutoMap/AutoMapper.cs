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
            CreateMap<UserUpdateInput, User>();
            CreateMap<User, UserInfoOutput>().ForMember(a => a.Age, condition => condition.MapFrom(d => CalculateAge(d.Birthday)));
            CreateMap<BookingDetailInput, Booking>();
            CreateMap<BookingDetailInput, BookingDetail>()
                .ForMember(a => a.StartDate, option => option.MapFrom(tbl => tbl.StartDate))
                .ForMember(a => a.EndDate, option => option.MapFrom(tbl => tbl.EndDate))
                .ForMember(a => a.Status, option => option.MapFrom(tbl => 1))
                ;
            CreateMap<Qr, QrOutput>()
                .ForMember(a => a.QrStringImage, option => option.MapFrom(tbl => Encoding.UTF8.GetString(tbl.QrString)))
                ;

            CreateMap<BookingDetail, QrInput>()
                .ForMember(a => a.UserId, option => option.MapFrom(tbl =>tbl.Booking.UserId))
                .ForMember(a => a.RoomName, option => option.MapFrom(tbl =>tbl.Room.RoomName))
                ;
            CreateMap<QrInput, Qr>()
                .ForMember(a => a.Status, option => option.MapFrom(tbl => 0))
                ;

            CreateMap<Booking, BookingInfoOutput>()
                .ForMember(a => a.HotelAddress, option => option.MapFrom(tbl => tbl.Hotel.HotelAddress))
                .ForMember(a => a.HotelName, option => option.MapFrom(tbl => tbl.Hotel.HotelName))
                .ForMember(a => a.TotalDate, option => option.MapFrom(tbl => CalculateDate(tbl.EndDate, tbl.StartDate)))
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
        private int CalculateDate(DateTime? start, DateTime? end)
        {
            if (start != null || end != null || start < end)
            {
                DateTime startdate = (DateTime)start;
                DateTime enddate = (DateTime)end;
                var duration = enddate.Date.Hour - startdate.Date.Hour;
                return duration;
            }
            return 0;
        }
    }
}
