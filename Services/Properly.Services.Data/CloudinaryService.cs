namespace Properly.Services.Data
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Properly.Common;
    using Properly.Services.Data.Contracts;

    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinary;
        private readonly ILogger<CloudinaryService> logger;

        public CloudinaryService(Cloudinary cloudinary,
            ILogger<CloudinaryService> logger)
        {
            this.cloudinary = cloudinary;
            this.logger = logger;
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            try
            {
                byte[] imageBytes = await GetBytes(imageFile);
                string fileName = imageFile.FileName;

                using var memoryStream = new MemoryStream(imageBytes);
                memoryStream.Position = 0;

                var uploadParams = new ImageUploadParams()
                {
                    Folder = GlobalConstants.CloudinaryDefaultFolderPath,
                    File = new FileDescription(Guid.NewGuid().ToString(), memoryStream),
                    UseFilename = true,
                    UniqueFilename = false,
                };

                var uploadResponse = await this.cloudinary.UploadAsync(uploadParams);

                return uploadResponse.SecureUrl.ToString();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error uploading image.");
                throw;
            }
        }

        public async Task<bool> DeleteImageAsync(List<string> imagePublicIds)
        {
            if (!imagePublicIds.Any())
            {
                throw new ArgumentException("Image public IDs cannot be null or empty.", nameof(imagePublicIds));
            }

            var deleteParams = new DelResParams
            {
                PublicIds = imagePublicIds,
                Type = "upload",
                ResourceType = ResourceType.Image,
            };

            var response = await this.cloudinary.DeleteResourcesAsync(deleteParams);

            if (response.Error != null || response.Deleted.Values.Contains("not_found"))
            {
               logger.LogError($"Error deleting images: {response.Error?.Message ?? "One or more images were not found"}");
                return false;
            }

            return response.StatusCode == HttpStatusCode.OK;
        }

        private static async Task<byte[]> GetBytes(IFormFile formFile)
        {
            await using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
