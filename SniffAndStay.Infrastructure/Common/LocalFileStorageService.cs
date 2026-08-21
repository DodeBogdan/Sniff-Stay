using SniffAndStay.Application.Common.Extensions;
using SniffAndStay.Application.Interfaces.Common;

namespace SniffAndStay.Infrastructure.Common
{
    public class LocalFileStorageService : IFileStorageService
    {
        public Task<Stream?> GetFileAsync(string filePath, CancellationToken cancellationToken)
        {
            if (filePath.IsNullOrEmpty())
            {
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(nameof(filePath));
            }

            Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return Task.FromResult<Stream?>(fileStream);
        }

        public Task DeleteFileAsync(string filePath, CancellationToken cancellationToken)
        {
            if (filePath.IsNullOrEmpty())
            {
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return Task.CompletedTask;
        }

        public async Task SaveFileAsync(string filePath, Stream fileContent, CancellationToken cancellationToken)
        {
            string filePathOnly = Path.GetDirectoryName(filePath)
                ?? throw new ArgumentException("Invalid file path.", nameof(filePath));

            string fileName = Path.GetFileName(filePath) ??
                throw new ArgumentException("Invalid file name.", nameof(filePath));

            if (filePathOnly.IsNullOrEmpty())
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

            if (!Directory.Exists(filePathOnly))
            {
                Directory.CreateDirectory(filePathOnly);
            }

            using var fileStream = new FileStream(filePath, FileMode.Create);
            await fileContent.CopyToAsync(fileStream, cancellationToken);
        }
    }
}
