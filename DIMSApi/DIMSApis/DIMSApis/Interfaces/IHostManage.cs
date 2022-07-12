﻿using DIMSApis.Models.Input;
using DIMSApis.Models.Output;

namespace DIMSApis.Interfaces
{
    public interface IHostManage
    {
        Task<string> UpdateHotelMainPhoto(int photoID, int hotelID);
        Task<string> LocalPaymentFinal(LocalPaymentInput ppi, int userId);

        Task<IEnumerable<HotelPhotosOutput>> GetListHotelPhotos(int userId, int hotelId);

        Task<IEnumerable<HotelOutput>> GetListAllHotel(int userId);

        Task<IEnumerable<AHotelAllRoomStatusOutput>> GetListAHotelAllRoomStatusSearch(int userId, int hotelId, DateTime ArrivalDate, int totalnight);

        Task<IEnumerable<AHotelAllRoomStatusOutput>> GetListAHotelAllRoomStatusToday(int userId, int hotelId, DateTime today);

        Task<IEnumerable<AHotelAllRoomStatusOutput>> GetListAHotelAllRoomStatusCheckOut(int userId, int hotelId, DateTime today);

        Task<HotelCateInfoOutput> GetAHotelAllInfo(int hotelId, int userId);

        Task<RoomDetailInfoOutput> GetADetailRoom(int userId, int RoomId, DateTime today);
    }
}