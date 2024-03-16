namespace Properly.Services.Data
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;
    using Properly.Services.Data.Contracts;

    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            byte[] imageBytes = await GetBytes(imageFile);
            string fileName = imageFile.FileName;

            using var memoryStream = new MemoryStream();
            var uploadParams = new ImageUploadParams()
            {
                Folder = "Properly",
                File = new FileDescription(Guid.NewGuid().ToString(), memoryStream),
                UseFilename = true,
                UniqueFilename = false,
            };

            var uploadResponse = await this.cloudinary.UploadAsync(uploadParams);

            return uploadResponse.SecureUrl.ToString();
        }

        public string GetImageUrl(string imagePublicId) 
        {
            throw new System.NotImplementedException();
        }

        private static async Task<byte[]> GetBytes(IFormFile formFile)
        {
            await using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
