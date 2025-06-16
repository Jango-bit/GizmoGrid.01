//using CloudinaryDotNet;
//using CloudinaryDotNet.Actions;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using System;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;

//namespace GizmoGrid._01.Services.ImageUploader
//{
//    public class ImageUploaderService : IImageUploader
//    {
//        private readonly Cloudinary _cloudinary;
//        private readonly ILogger<ImageUploaderService> _logger;
//        private readonly long _maxFileSize;
//        private readonly string[] _allowedExtensions;

//        public ImageUploaderService(IConfiguration configuration, ILogger<ImageUploaderService> logger)
//        {
//            _logger = logger;

//            var cloudName = configuration["Cloudinary:CloudName"];
//            var apiKey = configuration["Cloudinary:ApiKey"];
//            var apiSecret = configuration["Cloudinary:ApiSecret"];

//            if (string.IsNullOrWhiteSpace(cloudName) || string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(apiSecret))
//            {
//                _logger.LogError("Cloudinary configuration is missing or invalid.");
//                throw new CloudinaryConfigurationException("Cloudinary configuration is missing or invalid.");
//            }

//            _maxFileSize = 5 * 1024 * 1024; // 5MB
//            _allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

//            var account = new Account(cloudName, apiKey, apiSecret);
//            _cloudinary = new Cloudinary(account);
//            _cloudinary.Api.Secure = true;
//        }

//        public async Task<string> UploadImage(IFormFile file)
//        {
//            if (file == null || file.Length == 0) return null;

//            using (var stream = file.OpenReadStream())
//            {
//                var uploadParams = new ImageUploadParams
//                {
//                    File = new FileDescription(file.FileName, stream),
//                    Transformation = new Transformation().Height(500).Width(500).Crop("fill")
//                };
//                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
//                return uploadResult.SecureUrl.ToString();
//            }
//        }


//        public async Task<bool> DeleteImageAsync(string publicId)
//        {
//            if (string.IsNullOrWhiteSpace(publicId))
//            {
//                _logger.LogError("Public ID is null or empty.");
//                throw new ArgumentException("Public ID cannot be null or empty.");
//            }

//            var deletionParams = new DeletionParams(publicId);
//            var result = await _cloudinary.DestroyAsync(deletionParams);

//            if (result.Result == "ok")
//            {
//                _logger.LogInformation("Image deleted successfully from Cloudinary.");
//                return true;
//            }

//            _logger.LogError("Failed to delete image from Cloudinary: " + result.Result);
//            return false;
//        }
//    }

//    public class CloudinaryUploadException : Exception
//    {
//        public CloudinaryUploadException(string message) : base(message) { }
//    }

//    public class CloudinaryConfigurationException : Exception
//    {
//        public CloudinaryConfigurationException(string message) : base(message) { }
//    }
//}

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GizmoGrid._01.Services.ImageUploader
{
    public class ImageUploaderService : IImageUploader
    {
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<ImageUploaderService> _logger;
        private readonly long _maxFileSize;
        private readonly string[] _allowedExtensions;

        public ImageUploaderService(IConfiguration configuration, ILogger<ImageUploaderService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var cloudName = configuration["Cloudinary:CloudName"];
            var apiKey = configuration["Cloudinary:ApiKey"];
            var apiSecret = configuration["Cloudinary:ApiSecret"];

            if (string.IsNullOrWhiteSpace(cloudName) || string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(apiSecret))
            {
                _logger.LogError("Cloudinary configuration is missing or invalid.");
                throw new CloudinaryConfigurationException("Cloudinary configuration is missing or invalid.");
            }

            _maxFileSize = 5 * 1024 * 1024; // 5MB
            _allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
        }

        public async Task<ImageUploadResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("No file provided for upload.");
                return new ImageUploadResult { Url = null, ImageSize = null };
            }

            if (file.Length > _maxFileSize)
            {
                _logger.LogError($"File size {file.Length} exceeds maximum limit {_maxFileSize}.");
                throw new CloudinaryUploadException($"File size {file.Length} exceeds 5MB.");
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
            {
                _logger.LogError($"Invalid file extension {extension}.");
                throw new CloudinaryUploadException($"Invalid extension. Allowed: {string.Join(", ", _allowedExtensions)}.");
            }

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation().Height(500).Width(500).Crop("fill")
                    };
                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    _logger.LogInformation($"Image uploaded successfully: {uploadResult.SecureUrl}");
                    return new ImageUploadResult
                    {
                        Url = uploadResult.SecureUrl.ToString(),
                        ImageSize = 1.0f // Default scale
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image.");
                throw new CloudinaryUploadException("Failed to upload image.");
            }
        }

        public async Task<bool> DeleteImageAsync(string publicId)
        {
            if (string.IsNullOrWhiteSpace(publicId))
            {
                _logger.LogError("Public ID is null or empty.");
                throw new ArgumentException("Public ID cannot be null or empty.");
            }

            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);

            if (result.Result == "ok")
            {
                _logger.LogInformation("Image deleted successfully from Cloudinary.");
                return true;
            }

            _logger.LogError($"Failed to delete image from Cloudinary: {result.Result}");
            return false;
        }
    }

    public class CloudinaryUploadException : Exception
    {
        public CloudinaryUploadException(string message) : base(message) { }
    }

    public class CloudinaryConfigurationException : Exception
    {
        public CloudinaryConfigurationException(string message) : base(message) { }
    }

    public class ImageUploadResult
    {
        public string Url { get; set; }
        public float? ImageSize { get; set; }
    }

    public interface IImageUploader
    {
        Task<ImageUploadResult> UploadImage(IFormFile file);
        Task<bool> DeleteImageAsync(string publicId);
    }
}