﻿using DIMSApis.Models.Data;
using DIMSApis.Models.Input;

namespace DIMSApis.Interfaces
{
    public interface IQrManage
    {
        Task<IEnumerable<Qr>> getListQrString(int bookingID);

        Task<string> vertifyQrContent(VertifyQrInput qrIn);

        Task<Booking> vertifyMainQrCheckIn(VertifyMainQrInput qrIn);

        Task<string> getStringToCheckRoom(int hotel, string roomName);



        Task<string> CheckInOnline(int hotelId, int BookingID);
        Task<string> CheckOutOnline(int hotelId, int BookingID);
    }
}