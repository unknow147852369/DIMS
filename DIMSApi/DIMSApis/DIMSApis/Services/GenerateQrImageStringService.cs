using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using IronBarCode;
using Microsoft.IdentityModel.Tokens;
using QRCoder;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DIMSApis.Services
{
    public class GenerateQrImageStringService : IGenerateQr
    {
        private readonly SymmetricSecurityKey _key;
        private readonly IOtherService _otherService;
        private readonly ICloudinaryService _cloudinary;

        public GenerateQrImageStringService(IConfiguration config, IOtherService otherService, ICloudinaryService cloudinary)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            _otherService = otherService;
            _cloudinary = cloudinary;
        }
        public  string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public  string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }


        private string createQrContent(QrInput qri, string randomString)
        {
            var contentQr = qri.HotelId + "+" + qri.BookingId + "+" + qri.UserId + "+" + qri.RoomId + "+" + qri.RoomName + "+" + randomString;
            var returnItem = Base64Encode(contentQr);
            return returnItem;

        }

        public void GetQrDetail(string QrContent, out string bookingID, out string RoomID, out string RandomString)
        {
            try
            {
                var token =QrContent;
                var item = Base64Decode(token);
                var ls = item.Split('+').ToList();
                bookingID = ls[1];
                RoomID = ls[3];
                RandomString = ls[5];
            }
            catch (Exception ex)
            {
                bookingID = "";
                RoomID = "";
                RandomString = "";
            }
        }

        private string createMainQrContent(Booking bookingFullDetail,string randomString)
        {
            var contentQr = bookingFullDetail.HotelId + "+" + bookingFullDetail.BookingId + "+" + bookingFullDetail.UserId + "+" + randomString ;
            var returnItem = Base64Encode(contentQr);
            return returnItem;
        }

        public void GetMainQrDetail(VertifyMainQrInput qri, out string bookingID, out string HotelId,out string RandomString)
        {
            try
            {
                var token = qri.QrContent;
                var item = Base64Decode(token);
                var ls = item.Split('+').ToList();
                bookingID = ls[1];
                HotelId = ls[0];
                RandomString = ls[3];
            }
            catch (Exception ex)
            {
                bookingID = "";
                HotelId = "";
                RandomString = "";
            }
        }

        public void GetMainQrUrlContent(Booking bookingFullDetail, string randomString, out string content, out string link)
        {
            try
            {
                content = createMainQrContent(bookingFullDetail,randomString);

                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
                PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
                byte[] vs = qrCode.GetGraphic(20);
                link = _cloudinary.CloudinaryUploadPhotoQr(vs);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void GetQrDetailUrlContent(QrInput qri, string randomString, out string content, out string link)
        {
            try
            {
                content = createQrContent(qri,randomString);

                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
                PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
                byte[] vs = qrCode.GetGraphic(20);

                link = _cloudinary.CloudinaryUploadPhotoQr(vs);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}