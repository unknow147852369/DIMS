using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using DIMSApis.Models.Helper;
using DIMSApis.Models.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DIMSApis.Controllers
{
    [Authorize(Roles = "HOST")]
    [Route("api/[controller]")]
    [ApiController]
    public class HotelManageController : ControllerBase
    {
        private readonly IHotelManage _hotel;

        public HotelManageController(IHotelManage hotel)
        {
            _hotel = hotel;
        }
        [HttpGet("A-Full-Hotel-Detail")]
        public async Task<IActionResult> GetFullHotelDetail(int hotelId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _hotel.GetFullHotelDetail(hotelId);
            if (check == null) { return BadRequest(new DataRespone { Message = "Empty" }); }
            return Ok(check);
        }
        //

        [HttpGet("List-Hotels")]
        public async Task<IActionResult> GetListHotels()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _hotel.GetListHotels(userId);
            if (check == null) { return BadRequest(new DataRespone { Message = "Empty" }); }
            return Ok(check);
        }
        [HttpGet("List-Hotel-Requests")]
        public async Task<IActionResult> GetListHotelRequests()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _hotel.GetListHotelRequests(userId);
            if (check == null) { return BadRequest(new DataRespone { Message = "Empty" }); }
            return Ok(check);
        }
        [HttpPost("Send-A-Hotel-Add-Request")]
        public async Task<IActionResult> SendAHotelAddRequest(HotelRequestAddInput newHotel)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _hotel.SendAHotelAddRequest(userId,newHotel);
            if (check.Equals("1"))
            {
                return Ok(new DataRespone { Message = "Send request success" });
            }
            else if (check.Equals("3"))
            {
                return Ok(new DataRespone { Message = "Send request success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "Send request fail " +check});
            }
        }
        [HttpPut("Send-A-Hotel-Update-Request")]
        public async Task<IActionResult> SendAHotelUpdateRequest(HotelRequestUpdateInput newHotel)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _hotel.SendAHotelUpdateRequest(userId, newHotel);
            if (check.Equals("1"))
            {
                return Ok(new DataRespone { Message = "Send request success" });
            }
            else if (check.Equals("3"))
            {
                return Ok(new DataRespone { Message = "Send request success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "Send request fail " + check });
            }
        }
        [HttpDelete("Remove-A-Hotel-Request")]
        public async Task<IActionResult> RemoveARequest(int hotelRequestId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _hotel.RemoveARequest(hotelRequestId);
            if (check.Equals("1"))
            {
                return Ok(new DataRespone { Message = "Remove request success" });
            }
            else if (check.Equals("3"))
            {
                return Ok(new DataRespone { Message = "Remove request success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "Remove request fail " + check });
            }
        }


        //
        [HttpGet("List-A-Hotel-Photos")]
        public async Task<IActionResult> GetListHotelPhotos(int hotelId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var HotelRoom = await _hotel.GetListHotelPhotos(hotelId);
            if (HotelRoom == null) { return BadRequest(new DataRespone { Message = "Not found" }); }
            return Ok(HotelRoom);
        }

        [HttpPut("Update-Hotel-MainPhoto")]
        public async Task<IActionResult> UpdateHotelMainPhoto(int hotelId, int photoId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _hotel.UpdateHotelMainPhoto(photoId, hotelId);
            if (check.Equals("1"))
            {
                return Ok(new DataRespone { Message = "Update success" });
            }
            else if (check.Equals("3"))
            {
                return Ok(new DataRespone { Message = "Update success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "Update fail" });
            }
        }
        [HttpPost("Add-A-Hotel-Photos")]
        public async Task<IActionResult> AddAHotelPhotos(ICollection<NewHotelPhotosInput> newPhotos)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var HotelCate = await _hotel.AddAHotelPhotos( newPhotos);
            if (HotelCate == null) { return BadRequest("Not Found"); }
            if (HotelCate.Equals("1"))
            {
                return Ok(new DataRespone { Message = "Add success" });
            }
            else if (HotelCate.Equals("3"))
            {
                return Ok(new DataRespone { Message = "Add success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "Add fail " });
            }
        }
        [HttpDelete("Remove-A-Hotel-Photo")]
        public async Task<IActionResult> RemoveAHotelPhotos(int PhotoId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var HotelCate = await _hotel.RemoveAHotelPhotos(PhotoId);
            if (HotelCate == null) { return BadRequest("Not Found"); }
            if (HotelCate.Equals("1"))
            {
                return Ok(new DataRespone { Message = "Remove success" });
            }
            else if (HotelCate.Equals("3"))
            {
                return Ok(new DataRespone { Message = "Remove success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "Remove fail" });
            }
        }
        //

        [HttpGet("List-A-Hotel-Cates")]
        public async Task<IActionResult> GetListHotelCates(int hotelId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var HotelCate = await _hotel.GetListHotelCates(hotelId);
            if (HotelCate == null) { return BadRequest(new DataRespone { Message = "Not found any cate" }); }
            return Ok(HotelCate);
        }

        [HttpPut("Update-Hotel-Cate")]
        public async Task<IActionResult> UpdateHotelCate(NewUpdateHotelCateInput newCate)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _hotel.UpdateHotelCate(newCate);
            if (check.Equals("1"))
            {
                return Ok(new DataRespone { Message = "Update success" });
            }
            else if (check.Equals("3"))
            {
                return Ok(new DataRespone { Message = "Update success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "Update fail" } +check);
            }
        }
        [HttpPost("Add-A-Hotel-Cates")]
        public async Task<IActionResult> AddAHotelCates(ICollection<NewHotelCateInpput> newCates)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var HotelCate = await _hotel.AddAHotelCates(newCates);
            if (HotelCate == null) { return BadRequest("Not Found"); }
            if (HotelCate.Equals("1"))
            {
                return Ok(new DataRespone { Message = "Add success" });
            }
            else if (HotelCate.Equals("3"))
            {
                return Ok(new DataRespone { Message = "Add success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "Add fail" });
            }
        }
        [HttpDelete("Remove-A-Hotel-Cate")]
        public async Task<IActionResult> RemoveAHotelCate(int categoriID)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var HotelCate = await _hotel.RemoveAHotelCate(categoriID);
            if (HotelCate == null) { return BadRequest("Not Found"); }
            if (HotelCate.Equals("1"))
            {
                return Ok(new DataRespone { Message = "Remove success" });
            }
            else if (HotelCate.Equals("3"))
            {
                return Ok(new DataRespone { Message = "Remove success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "Remove fail" +HotelCate} );
            }
        }
        //
        [HttpGet("List-A-cate-Photos")]
        public async Task<IActionResult> GetListCatePhotos(int hotelId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _hotel.GetListCatePhotos(hotelId);
            if (check == null) { return BadRequest(new DataRespone { Message = "Not found" }); }
            return Ok(check);
        }

        [HttpPut("Update-cate-MainPhoto")]
        public async Task<IActionResult> UpdateCateMainPhoto(int hotelId,int cateId, int photoId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _hotel.UpdateCateMainPhoto(photoId, hotelId,cateId);
            if (check.Equals("1"))
            {
                return Ok(new DataRespone { Message = "Update success" });
            }
            else if (check.Equals("3"))
            {
                return Ok(new DataRespone { Message = "Update success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "Update fail" +check });
            }
        }
        [HttpPost("Add-A-cate-Photos")]
        public async Task<IActionResult> AddACatePhotos(NewCatePhotosInput newPhotos)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _hotel.AddACatePhotos(newPhotos);
            if (check == null) { return BadRequest("Not Found"); }
            if (check.Equals("1"))
            {
                return Ok(new DataRespone { Message = "Add success" });
            }
            else if (check.Equals("3"))
            {
                return Ok(new DataRespone { Message = "Add success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "Add fail " +check});
            }
        }
        [HttpDelete("Remove-A-cate-Photo")]
        public async Task<IActionResult> RemoveACatePhotos(int PhotoId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _hotel.RemoveACatePhotos(PhotoId);
            if (check == null) { return BadRequest("Not Found"); }
            if (check.Equals("1"))
            {
                return Ok(new DataRespone { Message = "Remove success" });
            }
            else if (check.Equals("3"))
            {
                return Ok(new DataRespone { Message = "Remove success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "Remove fail" +check });
            }
        }
        //
        [HttpGet("List-Rooms")]
        public async Task<IActionResult> GetListRoom(int hotelId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _hotel.GetListRoom(hotelId);
            if (check == null) { return BadRequest(new DataRespone { Message = "Not found" }); }
            return Ok(check);
        }

        [HttpPut("Update-A-Room")]
        public async Task<IActionResult> UpdateARoom(NewUpdateRoomInput newRoom)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _hotel.UpdateARoom(newRoom);
            if (check.Equals("1"))
            {
                return Ok(new DataRespone { Message = "Update success" });
            }
            else if (check.Equals("3"))
            {
                return Ok(new DataRespone { Message = "Update success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "Update fail" + check });
            }
        }
        [HttpPost("Add-Rooms")]
        public async Task<IActionResult> AddRooms(NewRoomFirstInput newRoom)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _hotel.AddRooms(newRoom);
            if (check == null) { return BadRequest("Not Found"); }
            if (check.Equals("1"))
            {
                return Ok(new DataRespone { Message = "Add success" });
            }
            else if (check.Equals("3"))
            {
                return Ok(new DataRespone { Message = "Add success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "Add fail " + check });
            }
        }
        [HttpDelete("Remove-A-Room")]
        public async Task<IActionResult> RemoveARoom(int RoomId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _hotel.RemoveARoom(RoomId);
            if (check == null) { return BadRequest("Not Found"); }
            if (check.Equals("1"))
            {
                return Ok(new DataRespone { Message = "Remove success" });
            }
            else if (check.Equals("3"))
            {
                return Ok(new DataRespone { Message = "Remove success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "Remove fail" + check });
            }
        }
        //
        [HttpGet("List-Vouchers")]
        public async Task<IActionResult> GetListVouchers(int hotelId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _hotel.GetListVouchers(hotelId);
            if (check == null) { return BadRequest(new DataRespone { Message = "Not found" }); }
            return Ok(check);
        }

        [HttpPut("Update-A-Voucher")]
        public async Task<IActionResult> UpdateAVoucher(AHotelVouchersInput newVoucher)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _hotel.UpdateAVoucher(newVoucher);
            if (check.Equals("1"))
            {
                return Ok(new DataRespone { Message = "Update success" });
            }
            else if (check.Equals("3"))
            {
                return Ok(new DataRespone { Message = "Update success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "Update fail" + check });
            }
        }
        [HttpPost("Add-A-Voucher")]
        public async Task<IActionResult> AddVoucher(AhotelVoucherCreate newVoucher)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _hotel.AddVoucher(newVoucher);
            if (check == null) { return BadRequest("Not Found"); }
            if (check.Equals("1"))
            {
                return Ok(new DataRespone { Message = "Add success" });
            }
            else if (check.Equals("3"))
            {
                return Ok(new DataRespone { Message = "Add success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "Add fail " + check });
            }
        }
        [HttpDelete("Remove-A-Vocuher")]
        public async Task<IActionResult> RemoveAVoucher(int VoucherId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _hotel.RemoveAVoucher(VoucherId);
            if (check == null) { return BadRequest("Not Found"); }
            if (check.Equals("1"))
            {
                return Ok(new DataRespone { Message = "Remove success" });
            }
            else if (check.Equals("3"))
            {
                return Ok(new DataRespone { Message = "Remove success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "Remove fail" + check });
            }
        }
        //
        [HttpGet("List-SpecialPrices")]
        public async Task<IActionResult> GetListSpecialPrice(int hotelId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _hotel.GetListSpecialPrice(hotelId);
            if (check == null) { return BadRequest(new DataRespone { Message = "Not found" }); }
            return Ok(check);
        }

        [HttpPut("Update-A-SpecialPrice")]
        public async Task<IActionResult> UpdateASpecialPrice(ICollection<NewCategorySpecialPriceUpdateInput> newSpecialPrice)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _hotel.UpdateASpecialPrice(newSpecialPrice);
            if (check.Equals("1"))
            {
                return Ok(new DataRespone { Message = "Update success" });
            }
            else if (check.Equals("3"))
            {
                return Ok(new DataRespone { Message = "Update success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "Update fail" + check });
            }
        }
        [HttpPost("Add-A-SpecialPrice")]
        public async Task<IActionResult> AddVoucher(ICollection<NewCategorySpecialPriceSecondInput> newSpecialPrice)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _hotel.AddSpecialPrice(newSpecialPrice);
            if (check == null) { return BadRequest("Not Found"); }
            if (check.Equals("1"))
            {
                return Ok(new DataRespone { Message = "Add success" });
            }
            else if (check.Equals("3"))
            {
                return Ok(new DataRespone { Message = "Add success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "Add fail " + check });
            }
        }
        [HttpDelete("Remove-A-SpecialPrice")]
        public async Task<IActionResult> RemoveASpecialPrice(ICollection<newSpecialPriceIDInput> SpecialPrice)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _hotel.RemoveASpecialPrice(SpecialPrice);
            if (check == null) { return BadRequest("Not Found"); }
            if (check.Equals("1"))
            {
                return Ok(new DataRespone { Message = "Remove success" });
            }
            else if (check.Equals("3"))
            {
                return Ok(new DataRespone { Message = "Remove success" });
            }
            else
            {
                return BadRequest(new DataRespone { Message = "Remove fail" + check });
            }
        }
    }
}
