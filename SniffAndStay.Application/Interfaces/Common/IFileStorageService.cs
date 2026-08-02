namespace SniffAndStay.Application.Interfaces.Common
{
    public enum FileType
    {
        ProfilePicture,
        PetPicture,
        ReviewPicture,
        Other
    }

    public interface IFileStorageService
    {
        Task<Stream?> GetFileAsync(string filePath, CancellationToken cancellationToken);
        Task DeleteFileAsync(string filePath, CancellationToken cancellationToken);
        Task SaveFileAsync(string filePath, Stream fileContent, CancellationToken cancellationToken);
    }
}
