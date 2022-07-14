using DIMSApis.Interfaces;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace DIMSApis.Services
{
    public class OtherService : IOtherService
    {
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            var hmac = new HMACSHA512(passwordSalt);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            if (hash.SequenceEqual(passwordHash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public string RandomString(int length)
        {
            Random random = new();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public DateTime GetEndDate(DateTime startDate, int night)
        {
            DateTime answer = startDate.AddDays(night).Add(new TimeSpan(12, 00, 0));
            return answer;
        }

        public string RemoveMark(string inputString)
        {
            try
            {
                if (inputString == null) inputString = "";
                var normalizedString = inputString.Normalize(NormalizationForm.FormD);
                var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

                for (int i = 0; i < normalizedString.Length; i++)
                {
                    char c = normalizedString[i];
                    var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                    if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    {
                        stringBuilder.Append(c);
                    }
                }

                return stringBuilder
                    .ToString()
                    .Normalize(NormalizationForm.FormC)
                    .Replace("đ", "d").Replace("Đ", "D")
                    .ToLower();
            }
            catch (Exception ex) { throw ex; }
        }
    }
}