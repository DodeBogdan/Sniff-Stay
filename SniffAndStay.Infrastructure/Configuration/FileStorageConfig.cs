namespace SniffAndStay.Application.Common.Configurations
{
    internal class FileStorageConfig
    {
        public const string SectionName = "FileStorage";

        public bool UseLocalStorage { get; set; }
        public string ProfilePicturePath { get; set; } = default!;
        public string PetPicturePath { get; set; } = default!;
        public string ReviewPicturePath { get; set; } = default!;
        public string LocalPath { get; set; } = default!;
        public string CloudPath { get; set; } = default!;
    }
}
