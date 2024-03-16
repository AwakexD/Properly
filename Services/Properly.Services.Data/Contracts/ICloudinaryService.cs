namespace Properly.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile imageFile);

        string GetImageUrl(string imagePublicId);
    }
}
