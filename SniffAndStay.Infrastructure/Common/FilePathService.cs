using Microsoft.Extensions.Options;
using SniffAndStay.Application.Common.Configurations;
using SniffAndStay.Application.Interfaces.Common;

namespace SniffAndStay.Infrastructure.Common
{
    public class FilePathService : IFilePathService
    {
        private readonly FileStorageConfig _config;

        public FilePathService(IOptions<FileStorageConfig> options)
            => _config = options.Value;

        public string ResolveFileName(Guid userId, string fileName)
        {
            return Path.Combine(userId.ToString(), fileName);
        }

        public string ResolveFilePath(string fileName, FileType fileType)
        {
            return fileType switch
            {
                FileType.ProfilePicture => Path.Combine(BuildFullPath(_config.ProfilePicturePath), fileName),
                FileType.PetPicture => Path.Combine(BuildFullPath(_config.PetPicturePath), fileName),
                FileType.ReviewPicture => Path.Combine(BuildFullPath(_config.ReviewPicturePath), fileName),
                FileType.Other => throw new NotImplementedException(),
                _ => throw new InvalidDataException(),
            };
        }
        private string BuildFullPath(string relativePath)
            => Path.Combine(_config.UseLocalStorage ? _config.LocalPath : _config.CloudPath, relativePath);
    }
}
