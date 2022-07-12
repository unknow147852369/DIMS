using DIMSApis.Models.Data;
using DIMSApis.Models.Input;

namespace DIMSApis.Interfaces
{
    public interface IFireBaseService
    {
        Task<string> GetlinkImage(QrInput qrInput, string imagePath, string imageName);

        bool RemoveDirectories(string imagePath);

        void createFilePath(QrInput qrInput, out string imagePath, out string imageName);

        void createFileMainPath(Booking bookingFullDetail, out string imageMainPath, out string imageMainName);

        Task<string> GetlinkMainImage(Booking bookingFullDetail, string imageMainPath, string imageMainName);
    }
}