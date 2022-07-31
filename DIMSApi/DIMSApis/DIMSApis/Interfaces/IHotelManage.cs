using DIMSApis.Models.Input;
using DIMSApis.Models.Output;
using DIMSApis.Models.Data;

namespace DIMSApis.Interfaces
{
    public interface IHotelManage
    {
        Task<AHotelOutput> GetFullHotelDetail(int hotelId);

        Task<IEnumerable<Hotel>> GetListHotels(int userID);
        Task<IEnumerable<HotelRequest>> GetListHotelRequests(int userID);
        Task<string> SendAHotelAddRequest(int userId,HotelRequestAddInput newHotel);
        Task<string> SendAHotelUpdateRequest(int userId, HotelRequestUpdateInput newHotel);
        Task<string> RemoveARequest(int HotelRequestId);


        Task<string> RemoveAHotelPhotos(int photo);
        Task<string> AddAHotelPhotos(ICollection<NewHotelPhotosInput> newPhotos);
        Task<string> UpdateHotelMainPhoto(int photoID, int hotelID);
        Task<IEnumerable<HotelPhotosOutput>> GetListHotelPhotos(int hotelId);

        Task<string> RemoveAHotelCate(int CateId);
        Task<string> AddAHotelCates(ICollection<NewHotelCateInpput> newCate);
        Task<string> UpdateHotelCate(NewUpdateHotelCateInput newCate);
        Task<IEnumerable<Category>> GetListHotelCates( int hotelId);

        Task<string> RemoveACatePhotos(int photo);
        Task<string> AddACatePhotos(NewCatePhotosInput newPhotos);
        Task<string> UpdateCateMainPhoto(int photoID, int hotelID ,int cateID);
        Task<IEnumerable<Category>> GetListCatePhotos(int hotelId);

        Task<string> RemoveARoom(int roomId);
        Task<string> AddRooms(NewRoomFirstInput newRoom);
        Task<string> UpdateARoom(NewUpdateRoomInput newRoom);
        Task<IEnumerable<Room>> GetListRoom(int hotelId);


        Task<string> RemoveAVoucher(int voucherId);
        Task<string> AddVoucher(AhotelVoucherCreate newVoucher);
        Task<string> UpdateAVoucher(AHotelVouchersInput newVoucher);
        Task<IEnumerable<Voucher>> GetListVouchers(int hotelId);

        Task<string> RemoveASpecialPrice(ICollection<newSpecialPriceIDInput> SpecialPrice);
        Task<string> AddSpecialPrice(ICollection<NewCategorySpecialPriceSecondInput> newSpecialPrice);
        Task<string> UpdateASpecialPrice(ICollection<NewCategorySpecialPriceUpdateInput> newSpecialPrice);
        Task<IEnumerable<Category>> GetListSpecialPrice(int hotelId);
    }
}
