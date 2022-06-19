using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using DIMSApis.Models.Input;
using Firebase.Auth;
using Firebase.Storage;
using IronBarCode;
using Microsoft.Extensions.Options;

namespace DIMSApis.Services
{
    public class FireBaseService : IFireBaseService
    {
        private readonly FireBaseSettings _firebase;

        public FireBaseService(IOptions<FireBaseSettings> firebase)
        {
            _firebase = firebase.Value;
        }

        public void createFilePath( QrInput qrInput, out string imagePath, out string imageName)
        {
            imageName = $"qr{qrInput.HotelId}-{qrInput.UserId}-{qrInput.BookingId}-{qrInput.RoomId}-{qrInput.RoomName}.png";
            imagePath = $"Material/{qrInput.BookingId}/";
            if (!(Directory.Exists(imagePath)))
            {
                Directory.CreateDirectory(imagePath);
            }
        }

        public async Task<string> GetlinkImage(QrInput qrInput, string imagePath ,string imageName)
        {
            var fullPath = imagePath + imageName;
            FileStream ms = new FileStream($@"{fullPath}", FileMode.Open);

            //
            var auth = new FirebaseAuthProvider(new FirebaseConfig(_firebase.ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(_firebase.AuthEmail, _firebase.AuthPassword);

            // you can use CancellationTokenSource to cancel the upload midway
            var cancellation = new CancellationTokenSource();



            var task = new FirebaseStorage(
                _firebase.Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true 
                        })
                .Child(qrInput.HotelId.ToString())
                .Child(qrInput.UserId.ToString())
                .Child(qrInput.BookingId.ToString())
                .Child(imageName)
                .PutAsync(ms, cancellation.Token);

            task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");
            var link = await task;
            ms.Close();

           return link;
        }

        public bool RemoveDirectories(string imagePath)
        {
            //Delete all files from the Directory
            string[] files = Directory.GetFiles($@"{imagePath}");
            foreach (string b in files)
            {
                System.IO.File.Delete(b);
            }
            Directory.Delete($@"{imagePath}");
            return true;
        }
    }
}
