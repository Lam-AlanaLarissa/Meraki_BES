using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IImageService
    {
        public Task<ImageUploadResult> UploadImageAsync(IFormFile file, string folderName);
        public Task<string> GenerateAndUploadAvatarAsync(string fullName);
        public Task<bool> DeleteImageAsync(string imageUrl);
    }
}
