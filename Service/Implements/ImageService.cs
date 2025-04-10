using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Services.Common;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Drawing.Imaging;
using System.Drawing;
namespace Services.Implements
{
    public class ImageService : IImageService
    {
        private readonly CloudinaryDotNet.Cloudinary _cloudinary;

        public ImageService(IOptions<CloudinarySetting> options)
        {
            var account = new CloudinaryDotNet.Account(options.Value.CloudName, options.Value.ApiKey, options.Value.ApiSecret);
            _cloudinary = new CloudinaryDotNet.Cloudinary(account);
        }

        public async Task<ImageUploadResult> UploadImageAsync(IFormFile file, string folderName)
        {
            var result = await _cloudinary.UploadAsync(
                new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    DisplayName = file.FileName,
                    Folder = folderName
                }
            );
            if (result != null && result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return result;
            }
            return null;
        }

        public async Task<string> GenerateAndUploadAvatarAsync(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Full name is required.");

            // Extract initials
            string initials = GetInitials(fullName);
            if (string.IsNullOrEmpty(initials))
                throw new ArgumentException("Invalid name format.");

            // Generate avatar image as byte array
            byte[] imageBytes = GenerateAvatarImage(initials, fullName);

            // Upload avatar to Cloudinary
            return await UploadToCloudinaryAsync(imageBytes, fullName);
        }

        private byte[] GenerateAvatarImage(string initials, string seed)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (Bitmap bmp = new Bitmap(100, 100)) // Avatar size 100x100
                using (Graphics graphics = Graphics.FromImage(bmp))
                {
                    graphics.Clear(GetRandomColor(seed)); // Set random background color
                    using (Font font = new Font("Arial", 40, FontStyle.Bold))
                    using (SolidBrush brush = new SolidBrush(Color.White))
                    {
                        // Measure text size
                        SizeF textSize = graphics.MeasureString(initials, font);
                        float x = (bmp.Width - textSize.Width) / 2;
                        float y = (bmp.Height - textSize.Height) / 2;

                        // Draw initials
                        graphics.DrawString(initials, font, brush, x, y);
                    }

                    // Save image to stream
                    bmp.Save(stream, ImageFormat.Png);
                }

                return stream.ToArray();
            }
        }

        private async Task<string> UploadToCloudinaryAsync(byte[] imageBytes, string fullName)
        {
            using (MemoryStream stream = new MemoryStream(imageBytes))
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(fullName + ".png", stream),
                    DisplayName = fullName,
                    Folder = "avatars"
                };

                var result = await _cloudinary.UploadAsync(uploadParams);
                return result.SecureUrl.ToString(); // Return image URL
            }
        }

        private string GetInitials(string fullName)
        {
            string[] names = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (names.Length == 0) return string.Empty;

            string firstInitial = names[0][0].ToString().ToUpper();
            string lastInitial = names.Length > 1 ? names[^1][0].ToString().ToUpper() : "";

            return firstInitial + lastInitial;
        }

        private Color GetRandomColor(string seed)
        {
            int hash = seed.GetHashCode();
            Random rand = new Random(hash);

            int r = rand.Next(50, 200);
            int g = rand.Next(50, 200);
            int b = rand.Next(50, 200);

            return Color.FromArgb(r, g, b);
        }
        public async Task<bool> DeleteImageAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return false;

            try
            {
                // Convert URL to URI
                var uri = new Uri(imageUrl);

                // Extract path segments (folder and file name)
                string[] segments = uri.AbsolutePath.Split('/');

                if (segments.Length < 3)
                    return false; // Invalid format, return false

                // Extract folder name and public ID
                string folderName = segments[segments.Length - 2]; // Second last segment (folder)
                string fileName = segments.Last().Split('.')[0]; // Last segment (file without extension)
                string publicId = $"{folderName}/{fileName}"; // Format: folder/fileName

                // Delete from Cloudinary
                var deleteParams = new DeletionParams(publicId);
                var result = await _cloudinary.DestroyAsync(deleteParams);

                return result.Result == "ok"; // Return true if deletion was successful
            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}
