using Microsoft.Extensions.Options;
using SniffAndStay.Application.Common.Configurations;

namespace SniffAndStay.Infrastructure.Common
{
    internal class FileStorageConfigAdapter : IFileStorageConfig
    {
        private readonly FileStorageConfig _config;

        public FileStorageConfigAdapter(IOptions<FileStorageConfig> options)
            => _config = options.Value;

        public string GetProfilePicturePath() => BuildFullPath(_config.ProfilePicturePath);
        public string GetPetPicturePath() => BuildFullPath(_config.PetPicturePath);
        public string GetReviewPicturePath() => BuildFullPath(_config.ReviewPicturePath);

        private string BuildFullPath(string relativePath)
            => Path.Combine(_config.UseLocalStorage ? _config.LocalPath : _config.CloudPath, relativePath);
    }
}
