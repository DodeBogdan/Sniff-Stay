using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
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
            switch (fileType)
            {
                case FileType.ProfilePicture:
                    return Path.Combine(BuildFullPath(_config.ProfilePicturePath), fileName);
                case FileType.PetPicture:
                    return Path.Combine(BuildFullPath(_config.PetPicturePath), fileName);
                case FileType.ReviewPicture:
                    return Path.Combine(BuildFullPath(_config.ReviewPicturePath), fileName);
                case FileType.Other:
                    throw new NotImplementedException();
                default:
                    throw new InvalidDataException();
            }
        }
        private string BuildFullPath(string relativePath)
            => Path.Combine(_config.UseLocalStorage ? _config.LocalPath : _config.CloudPath, relativePath);
    }
}
