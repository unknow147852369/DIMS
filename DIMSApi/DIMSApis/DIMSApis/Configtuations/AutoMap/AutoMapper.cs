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
    }
}
