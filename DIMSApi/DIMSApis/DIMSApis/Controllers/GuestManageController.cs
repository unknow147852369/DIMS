using AutoMapper;
using DIMSApis.Interfaces;
using DIMSApis.Models.Helper;
using Microsoft.AspNetCore.Mvc;

namespace DIMSApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestManageController : ControllerBase
    {
        private readonly IUserManage _usermanage;
        private readonly IMapper _mapper;

        public GuestManageController(IUserManage usermanage, IMapper mapper)
        {
            _usermanage = usermanage;
            _mapper = mapper;
        }

        [HttpGet("Search-Hotel")]
        public async Task<IActionResult> GetListSearchHotel(string Location, string LocationName, DateTime ArrivalDate, int TotalNight)
        {
            var Hotel = await _usermanage.GetListSearchHotel(Location, LocationName, ArrivalDate, TotalNight);
            if (Hotel == null) { return BadRequest(new DataRespone { Message = "Some field wrong" }); }
            else if (Hotel.Count() == 0) { return NotFound(new DataRespone { Message = "out of room" }); }
            return Ok(Hotel);
        }

        [HttpGet("Avaiable-Hotel-Cate")]
        public async Task<IActionResult> GetAllHotelCate(int? hotelId, DateTime ArrivalDate, int TotalNight, int peopleQuanity)
        {
            var HotelRoom = await _usermanage.GetListAvaiableHotelCate(hotelId, ArrivalDate, TotalNight, peopleQuanity);
            if (HotelRoom == null) { return BadRequest(new DataRespone { Message = "Some field wrong" }); }
            return Ok(HotelRoom);
        }

        [HttpGet("Search-Province")]
        public async Task<IActionResult> SearchProvince(string ProvinceName)
        {
            var provinces = await _usermanage.SearchProvince(ProvinceName);
            if (provinces == null || provinces.Count() == 0) { return NotFound(new DataRespone { Message = "not found" }); }
            return Ok(provinces);
        }

        [HttpGet("Search-Ward")]
        public async Task<IActionResult> SearchWard(string WardName)
        {
            var wards = await _usermanage.SearchWard(WardName);
            if (wards == null || wards.Count() == 0) { return NotFound(new DataRespone { Message = "not found" }); }
            return Ok(wards);
        }

        [HttpGet("Search-District")]
        public async Task<IActionResult> SearchDistrict(string DistrictName)
        {
            var districts = await _usermanage.SearchDistrict(DistrictName);
            if (districts == null || districts.Count() == 0) { return NotFound(new DataRespone { Message = "not found" }); }
            return Ok(districts);
        }

        [HttpGet("all-District")]
        public async Task<IActionResult> ListAllDistrict()
        {
            var districts = await _usermanage.ListAllDistrict();
            return Ok(districts);
        }

        [HttpGet("Search-Loaction")]
        public async Task<IActionResult> SearchLocation(string LocationName)
        {
            var search = await _usermanage.SearchLocation(LocationName);
            if (search.Areas.Count() == 0 && search.Hotels.Count() == 0) { return NotFound(new DataRespone { Message = "not found" }); }
            return Ok(search);
        }

        [HttpGet("No-Mark_Colum-CHEAT")]
        public async Task<IActionResult> CreateNoMarkColumCHEAT()
        {
            var districts = await _usermanage.CreateNoMarkColumCHEAT();
            if (districts == null) { return NotFound("not found"); }
            return Ok(districts);
        }
    }
}