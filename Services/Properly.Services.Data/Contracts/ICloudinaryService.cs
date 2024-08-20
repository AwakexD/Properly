namespace Properly.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile imageFile);

        Task<bool> DeleteImageAsync(string imagePublicId);
    }
}
