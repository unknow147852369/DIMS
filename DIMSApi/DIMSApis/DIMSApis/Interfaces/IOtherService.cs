namespace DIMSApis.Interfaces
{
    public interface IOtherService
    {
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);

        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);

        string RandomString(int length);

        string RemoveMark(string inputString);

        DateTime GetEndDate(DateTime startDate, int night);
    }
}