using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;

namespace SuperShop.Helpers
{
    public interface IBlobHelper
    {
        Task<Guid> UploadBlobAsync(IFormFile file, string containterName);

        Task<Guid> UploadBlobAsync(byte[] file, string containterName);
        Task<Guid> UploadBlobAsync(string image, string containterName);

    }
}
