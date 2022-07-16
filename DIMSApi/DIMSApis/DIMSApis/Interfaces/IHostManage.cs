using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;

namespace DIMSApis.Interfaces
{
    public interface IHostManage
    {
        Task<string> UpdateHotelMainPhoto(int photoID, int hotelID);
        Task<string> AddItemForExtraFee(ICollection<ExtraFeeMenuDetailInput> ex);
        Task<string> UpdateCleanStatus(int RoomID);
        Task<string> AddInboundUser(checkInInput checkIn);
        Task<string> CheckOutLocal(int hotelId,int bookingID);
        Task<string> LocalPaymentFinal(LocalPaymentInput ppi, int userId);
        Task<string> CheckRoomDateBooking(CheckRoomDateInput chek);
        Task<string> AddItemMenu(ICollection<ItemMenuInput> item);
        Task<string> UpdateItemMenu(int MenuID, ItemMenuInput item);
        Task<IEnumerable<BookingDetailMenu>> GetUserMenu(int BookingDetailID);
        Task<IEnumerable<HotelListMenuOutput>> GetListMenu(int hotelID);

        Task<IEnumerable<HotelPhotosOutput>> GetListHotelPhotos(int userId, int hotelId);

        Task<IEnumerable<HotelOutput>> GetListAllHotel(int userId);

        Task<IEnumerable<AHotelAllRoomStatusOutput>> GetListAHotelAllRoomStatusSearch(int userId, int hotelId, DateTime ArrivalDate, int totalnight);

        Task<IEnumerable<AHotelAllRoomStatusOutput>> GetListAHotelAllRoomStatusToday(int userId, int hotelId, DateTime today);

        Task<IEnumerable<AHotelAllRoomStatusOutput>> GetListAHotelAllRoomStatusCheckOut(int userId, int hotelId, DateTime today);

        Task<HotelCateInfoOutput> GetAHotelAllInfo(int hotelId, int userId);

        Task<RoomDetailInfoOutput> GetADetailRoom(int userId, int RoomId, DateTime today);
    }
}