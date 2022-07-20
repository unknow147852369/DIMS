using DIMSApis.Models.Input;
using DIMSApis.Models.Output;
using DIMSApis.Models.Data;

namespace DIMSApis.Interfaces
{
    public interface IHotelManage
    {
        //Task<string> RemoveARoom(int roomId);
        //Task<string> AddRooms(NewRoomFirstInput newRoom);
        //Task<string> UpdateARoom(NewUpdateRoomInput newRoom);
        //Task<IEnumerable<Room>> GetListRoom(int hotelId);


        Task<string> RemoveAHotelPhotos(int photo);
        Task<string> AddAHotelPhotos(ICollection<NewHotelPhotosInput> newPhotos);
        Task<string> UpdateHotelMainPhoto(int photoID, int hotelID);
        Task<IEnumerable<HotelPhotosOutput>> GetListHotelPhotos(int hotelId);

        Task<string> RemoveAHotelCate(int CateId);
        Task<string> AddAHotelCates(ICollection<NewHotelCateInpput> newCate);
        Task<string> UpdateHotelCate(NewUpdateHotelCateInput newCate);
        Task<IEnumerable<Category>> GetListHotelCates( int hotelId);

        Task<string> RemoveACatePhotos(int photo);
        Task<string> AddACatePhotos(NewCatePhotosInput newPhotos);
        Task<string> UpdateCateMainPhoto(int photoID, int hotelID ,int cateID);
        Task<IEnumerable<Category>> GetListCatePhotos(int hotelId);

        Task<string> RemoveARoom(int roomId);
        Task<string> AddRooms(NewRoomFirstInput newRoom);
        Task<string> UpdateARoom(NewUpdateRoomInput newRoom);
        Task<IEnumerable<Room>> GetListRoom(int hotelId);
    }
}
