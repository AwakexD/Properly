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

            using var memoryStream = new MemoryStream(imageBytes);
            memoryStream.Position = 0;

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

        public async Task<bool> DeleteImageAsync(string imagePublicId)
        {
            if (string.IsNullOrEmpty(imagePublicId))
            {
                throw new ArgumentException("Image public ID cannot be null or empty.", nameof(imagePublicId));
            }

            var deleteParams = new DeletionParams(imagePublicId) { ResourceType = ResourceType.Image };
            var deleteResponse = await this.cloudinary.DestroyAsync(deleteParams);

            return deleteResponse.Result == "ok";
        }

        private static async Task<byte[]> GetBytes(IFormFile formFile)
        {
            await using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
