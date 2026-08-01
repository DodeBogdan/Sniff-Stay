using Microsoft.Extensions.Options;
using SniffAndStay.Application.Common.Extensions;
using SniffAndStay.Application.Interfaces.Common;
using SniffAndStay.Infrastructure.Configuration;

namespace SniffAndStay.Infrastructure.Common
{
    public class LocalFileStorageService : IFileStorageService
    {
        public Task<Stream?> GetFileAsync(string fullPath, CancellationToken cancellationToken)
        {
            if (fullPath.IsNullOrEmpty())
            {
                throw new ArgumentException("File path cannot be null or empty.", nameof(fullPath));
            }

            Stream fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            return Task.FromResult<Stream?>(fileStream);
        }

        public Task DeleteFileAsync(string fullPath, CancellationToken cancellationToken)
        {
            if (fullPath.IsNullOrEmpty())
            {
                throw new ArgumentException("File path cannot be null or empty.", nameof(fullPath));
            }

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
            
            return Task.CompletedTask;
        }

        public async Task SaveFileAsync(string filePath, string fileName, Stream fileContent, CancellationToken cancellationToken)
        {
            if (filePath.IsNullOrEmpty())
            {
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
            }

            if (fileName.IsNullOrEmpty())
            {
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
            }

            if (fileContent == null || fileContent.Length == 0)
            {
                throw new ArgumentException("File content cannot be null or empty.", nameof(fileContent));
            }

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            var fullFilePath = Path.Combine(filePath, fileName);

            using var fileStream = new FileStream(fullFilePath, FileMode.Create);
            await fileContent.CopyToAsync(fileStream, cancellationToken);
        }


    }
}
