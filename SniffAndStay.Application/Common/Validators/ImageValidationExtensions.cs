using FluentValidation;

namespace SniffAndStay.Application.Common.Validators
{
    public static class ImageValidationExtensions
    {
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };
        private static readonly string[] AllowedContentTypes = { "image/jpeg", "image/png" };

        private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB

        public static IRuleBuilderOptions<T, string?> MustBeValidImageFileName<T>(
            this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .Must(fileName => fileName is null ||
                    AllowedExtensions.Contains(Path.GetExtension(fileName).ToLowerInvariant()))
                .WithMessage($"Invalid extension: {string.Join(", ", AllowedExtensions)}");
        }

        public static IRuleBuilderOptions<T, string?> MustBeValidImageContentType<T>(
            this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .Must(contentType => contentType is null || AllowedContentTypes.Contains(contentType.ToLowerInvariant()))
                .WithMessage($"Invalid content type: {string.Join(", ", AllowedContentTypes)}");
        }

        public static IRuleBuilderOptions<T, Stream?> MustNotExceedMaxImageSize<T>(
            this IRuleBuilder<T, Stream?> ruleBuilder)
        {
            return ruleBuilder
                .Must(stream => stream is null || stream.Length <= MaxFileSizeBytes)
                .WithMessage($"File size exceeds the limit of {MaxFileSizeBytes / 1024 / 1024} MB.");
        }
    }
}
