namespace SniffAndStay.Application.Interfaces.Common
{
    public interface IFileStorageService
    {
        Task<Stream?> GetFileAsync(string fullPath, CancellationToken cancellationToken);
        Task DeleteFileAsync(string fullPath, CancellationToken cancellationToken);
        Task SaveFileAsync(string filePath, string fileName, Stream fileContent, CancellationToken cancellationToken);
    }
}
