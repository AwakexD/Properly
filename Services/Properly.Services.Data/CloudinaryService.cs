﻿using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Properly.Services.Data
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;
    using Properly.Services.Data.Contracts;
    using static System.Runtime.InteropServices.JavaScript.JSType;

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

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }

            return true;
        }

        private static async Task<byte[]> GetBytes(IFormFile formFile)
        {
            await using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
