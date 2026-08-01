namespace SniffAndStay.Application.Configuration
{
    public class FileStorageConfig
    {
        public const string SectionName = "FileStorage";

        public string ProfilePicturePath { get; set; } = default!;
        public string PetPicturePath { get; set; } = default!;
        public string ReviewPicturePath { get; set; } = default!;
    }
}
