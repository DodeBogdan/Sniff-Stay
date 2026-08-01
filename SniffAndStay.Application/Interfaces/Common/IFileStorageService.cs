namespace SniffAndStay.Application.Interfaces.Common
{
    public interface IFileStorageService
    {
        Task<Stream?> GetFileAsync(string fileName, CancellationToken cancellationToken);
        Task DeleteFileAsync(string fileName, CancellationToken cancellationToken);
        Task SaveFileAsync(string fileName, Stream fileContent, CancellationToken cancellationToken);
    }
}
