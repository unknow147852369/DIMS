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

        public GenerateQrImageStringService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public string GenerateQrString(QrInput qri)
        {
            var conntent = createQrContent(qri);
            var MyQRWithLogo = QRCodeWriter.CreateQrCodeWithLogo(conntent, "logo.png", 500);
            MyQRWithLogo.ChangeBarCodeColor(System.Drawing.Color.DarkGreen);
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
                new Claim("BookingdetailId",qri.BookingDetailId.ToString()),
                new Claim("userId",qri.UserId.ToString()),
                new Claim(ClaimTypes.Role,qri.RoomId.ToString()),
                new Claim(ClaimTypes.NameIdentifier,qri.BookingId.ToString())
            };
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDes = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
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

            bookingID = tokenS.Claims.First(claim => claim.Type == "nameid").Value;

            RoomID = tokenS.Claims.First(claim => claim.Type == "role").Value;




        }
    }
}
