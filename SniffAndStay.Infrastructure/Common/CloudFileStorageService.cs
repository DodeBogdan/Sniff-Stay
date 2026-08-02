using SniffAndStay.Application.Interfaces.Common;

namespace SniffAndStay.Infrastructure.Common
{
    public class CloudFileStorageService : IFileStorageService
    {
        public Task DeleteFileAsync(string fileName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Stream?> GetFileAsync(string fileName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SaveFileAsync(string fileName, Stream fileContent, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
