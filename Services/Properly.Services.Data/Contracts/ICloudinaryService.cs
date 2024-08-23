namespace Properly.Services.Data.Contracts
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Http;

    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile imageFile);

        Task<bool> DeleteImageAsync(List<string> imagePublicIds);
    }
}
