using DIMSApis.Interfaces;
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

        public GenerateQrImageStringService(IConfiguration config,IOtherService otherService)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            _otherService = otherService;
        }

        public string GenerateQrString(QrInput qri, string imagePath, string imageName)
        {
            var fullPath = imagePath + imageName;
            var conntent = createQrContent(qri);
            var MyQRWithLogo = QRCodeWriter.CreateQrCodeWithLogo(conntent, @"Material/images/logo.png", 500);
            MyQRWithLogo.ChangeBarCodeColor(System.Drawing.Color.DarkGreen).SaveAsPng($@"{fullPath}");

            byte[] vs = MyQRWithLogo.ToPngBinaryData();
            //
            string base64ImageRepresentation = Convert.ToBase64String(vs);
            if (base64ImageRepresentation != null)
            {
                return base64ImageRepresentation;
            }
            return null;
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
            var content = fomattoken ;
            return content;
        }

        public void GetQrDetail(VertifyQrInput qri, out string bookingID, out string RoomID)
        {
            var token = qri.QrContent;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;

            bookingID = tokenS.Claims.First(claim => claim.Type == "BookingId").Value;

            RoomID = tokenS.Claims.First(claim => claim.Type == "RoomId").Value;
        }
    }
}
