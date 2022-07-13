using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using IronBarCode;
using Microsoft.IdentityModel.Tokens;
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

        public void GenerateQr(QrInput qri, string imagePath, string imageName)
        {
            var fullPath = imagePath + imageName;
            var conntent = createQrContent(qri);
            var MyQRWithLogo = QRCodeWriter.CreateQrCodeWithLogo(conntent, @"Material/images/logo.png", 500);
            MyQRWithLogo.ChangeBarCodeColor(System.Drawing.Color.DarkGreen).SaveAsPng($@"{fullPath}");

            //byte[] vs = MyQRWithLogo.ToPngBinaryData();

            //string base64ImageRepresentation = Convert.ToBase64String(vs);
            //if (base64ImageRepresentation != null)
            //{
            //    return base64ImageRepresentation;
            //}
            //return null;
        }

        public string createQrContent(QrInput qri)
        {
            var claims = new List<Claim>
            {
                new Claim("HotelId",qri.HotelId.ToString()),
                new Claim("BookingId",qri.BookingId.ToString()),
                new Claim("userId",qri.UserId.ToString()),
                new Claim("RoomId",qri.RoomId.ToString()),
                new Claim("RoomName",qri.RoomName.ToString()),
            };
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDes = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDes);
            var fomattoken = tokenHandler.WriteToken(token);
            var content = fomattoken;
            return content;
        }

        public void GetQrDetail(VertifyQrInput qri, out string bookingID, out string RoomID)
        {
            try
            {
                var token = qri.QrContent;
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token);
                var tokenS = jsonToken as JwtSecurityToken;

                bookingID = tokenS.Claims.First(claim => claim.Type == "BookingId").Value;

                RoomID = tokenS.Claims.First(claim => claim.Type == "RoomId").Value;
            }
            catch (Exception ex)
            {
                bookingID = "";
                RoomID = "";
            }
        }

        public string createMainQrContent(Booking bookingFullDetail)
        {
            var claims = new List<Claim>
            {
                new Claim("HotelId",bookingFullDetail.HotelId.ToString()),
                new Claim("BookingId",bookingFullDetail.BookingId.ToString()),
                new Claim("userId",bookingFullDetail.UserId.ToString()),
            };
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDes = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDes);
            var fomattoken = tokenHandler.WriteToken(token);
            var content = fomattoken;
            return content;
        }

        public void GetMainQrDetail(VertifyMainQrInput qri, out string bookingID, out string HotelId)
        {
            try
            {
                var token = qri.QrContent;
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token);
                var tokenS = jsonToken as JwtSecurityToken;

                bookingID = tokenS.Claims.First(claim => claim.Type == "BookingId").Value;
                HotelId = tokenS.Claims.First(claim => claim.Type == "HotelId").Value;
            }
            catch (Exception ex)
            {
                bookingID = "";
                HotelId = "";
            }
        }

        public void GetMainQrUrlContent(Booking bookingFullDetail, out string content, out string link)
        {
            try
            {
                content = createMainQrContent(bookingFullDetail); ;
                var MyQRWithLogo = QRCodeWriter.CreateQrCode(content, 500);
                MyQRWithLogo.ChangeBarCodeColor(System.Drawing.Color.Red);

                byte[] vs = MyQRWithLogo.ToPngBinaryData();

                link = _cloudinary.CloudinaryUploadPhotoQr(vs);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void GetQrDetailUrlContent(QrInput qri, out string content, out string link)
        {
            try
            {
                content = createQrContent(qri); ;
                var MyQRWithLogo = QRCodeWriter.CreateQrCode(content, 500);
                MyQRWithLogo.ChangeBarCodeColor(System.Drawing.Color.LightSeaGreen);

                byte[] vs = MyQRWithLogo.ToPngBinaryData();

                link = _cloudinary.CloudinaryUploadPhotoQr(vs);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void GenerateMainQr(Booking bookingFullDetail, string imageMainPath, string imageMainName)
        {
            throw new NotImplementedException();
        }
    }
}