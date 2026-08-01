namespace SniffAndStay.Infrastructure.Common
{
    public interface IFileStorageConfig
    {
        string GetProfilePicturePath();
        string GetPetPicturePath();
        string GetReviewPicturePath();
    }
}