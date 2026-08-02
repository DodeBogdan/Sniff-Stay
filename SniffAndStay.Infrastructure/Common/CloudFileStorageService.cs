using SniffAndStay.Application.Interfaces.Common;

namespace SniffAndStay.Infrastructure.Common
{
    public class CloudFileStorageService : IFileStorageService
    {
        public Task DeleteFileAsync(string filePath, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Stream?> GetFileAsync(string filePath, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SaveFileAsync(string filePath, Stream fileContent, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
