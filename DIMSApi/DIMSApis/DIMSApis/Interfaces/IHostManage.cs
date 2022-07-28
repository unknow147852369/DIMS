using DIMSApis.Models.Data;
using DIMSApis.Models.Helper;
using DIMSApis.Models.Input;
using DIMSApis.Models.Output;

namespace DIMSApis.Interfaces
{
    public interface IHostManage
    {
        Task<string> AddItemForExtraFee(ICollection<ExtraFeeMenuDetailInput> ex);
        Task<string> DeleteItemForExtraFee(int BookingDetailId,int BookingDetailMenuId);
        Task<string> AddProblemForExtraFee(ICollection<ProblemExtraFeeInput> prEx);
        Task<string> UpdateCleanStatus(int RoomID);
        Task<string> AddInboundUser(checkInInput checkIn);
        Task<string> CheckOutLocal(int hotelId,int bookingID);
        Task<IEnumerable<NewInboundUser>> GetAllInboundUserBookingInfo(int hotelId);
        Task<string> LocalPaymentFinal(LocalPaymentInput ppi, int userId);
        Task<string> CheckRoomDateBooking(CheckRoomDateInput chek);
        Task<string> AddItemMenu(ICollection<ItemMenuInput> item);
        Task<string> UpdateItemMenu(int MenuID, ItemMenuInput item);
        Task<BookingDetail> GetUserMenu(int BookingDetailID);
        Task<IEnumerable<HotelListMenuOutput>> GetListMenu(int hotelID);



        Task<IEnumerable<HotelOutput>> GetListAllHotel(int userId);

        Task<IEnumerable<AHotelAllRoomStatusOutput>> GetListAHotelAllRoomStatusSearch(int userId, int hotelId, DateTime ArrivalDate, int totalnight);

        Task<IEnumerable<AHotelAllRoomStatusOutput>> GetListAHotelAllRoomStatusToday(int userId, int hotelId, DateTime today);

        Task<IEnumerable<AHotelAllRoomStatusOutput>> GetListAHotelAllRoomStatusCheckOut(int userId, int hotelId, DateTime today);
        Task<IEnumerable<AHotelAllRoomStatusOutput>> GetListAHotelOnlyRoomStatus13Search(int userId, int hotelId, DateTime today, int totalnight);

        Task<HotelCateInfoOutput> GetAHotelAllInfo(int hotelId, int userId);

        Task<RoomDetailInfoOutput> GetADetailRoom(int userId, int RoomId, DateTime today);


        Task<FullRoomMoneyDetailSumaryOutput> GetFullRoomMoneyDetailByFilter(int hotelID, DateTime startDate, DateTime endDate);
        Task<FullRoomMoneyDetailSumaryOutput> GetFullRoomMoneyNotCheckOutDetailByDate(int hotelId, DateTime startDate, DateTime endDate);

        Task<Pagination<Booking>> HostgetListBookingByPage<T>(int hotelID,int CurrentPage,int pageSize) where T : class;

        Task<ABookingFullOutput> HostGetABookingFullDetail(int bookingID);
    }
}