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
            CreateMap<UserUpdateInput, User>();
            CreateMap<User, UserInfoOutput>().ForMember(a => a.Age, condition => condition.MapFrom(d => CalculateAge(d.Birthday)));
            CreateMap<BookingRequestInput, Booking>();
            CreateMap<RoomRequestInput, RoomRequest>();


            CreateMap<Booking, BookingInfoOutput>()
                .ForMember(a => a.HotelAddress, option => option.MapFrom(tbl => tbl.Hotel.HotelAddress))
                .ForMember(a => a.HotelName, option => option.MapFrom(tbl => tbl.Hotel.HotelName))
                .ForMember(a => a.TotalDate, option => option.MapFrom(tbl => CalculateDate(tbl.EndDate, tbl.StartDate)))
                ;

            CreateMap<BookingDetail, BookingDetailInfoOutput>()
                .ForMember(a => a.CategoryName, option => option.MapFrom(tbl => tbl.Room.Category.CategoryName))
                .ForMember(a => a.RoomName, option => option.MapFrom(tbl => tbl.Room.RoomName))
                .ForMember(a => a.RoomName, option => option.MapFrom(tbl => tbl.Room.Price))
                .ForMember(a => a.CategoryId, option => option.MapFrom(tbl => tbl.Room.CategoryId))

                ;
            CreateMap<BookingDetailInfoInput, BookingDetail>();
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
