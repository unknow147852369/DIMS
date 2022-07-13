using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DIMSApis.Interfaces;
using DIMSApis.Models.Helper;
using Microsoft.Extensions.Options;

namespace DIMSApis.Services
{
    public class CloudinaryService: ICloudinaryService
    {
        private readonly CloudinarySettings _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> cloudinary)
        {
            _cloudinary = cloudinary.Value;
        }

        public string CloudinaryUploadPhotoQr(byte[] data)
        {
            try
            {
                Account account = new Account
                {
                    Cloud = _cloudinary.CloudName,
                    ApiKey = _cloudinary.APIKey,
                    ApiSecret = _cloudinary.APISecret,
                };

                Cloudinary cloudinary = new Cloudinary(account);
                Stream stream = new MemoryStream(data);
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(Guid.NewGuid().ToString(), stream),
                    PublicId = Guid.NewGuid().ToString(),
                };
                ImageUploadResult uploadResult = cloudinary.Upload(uploadParams);
                var returnUrl = cloudinary.Api.UrlImgUp.Transform(new Transformation().Crop("fill"))
                        .BuildUrl(String.Format("{0}.{1}", uploadResult.PublicId, uploadResult.Format));
                return returnUrl;
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
