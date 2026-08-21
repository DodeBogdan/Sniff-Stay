using FluentValidation;
using SniffAndStay.Domain.Constants;

namespace SniffAndStay.Application.Common.Validators
{
    public static class ImageValidationExtensions
    {
        public static IRuleBuilderOptions<T, string?> MustBeValidImageFileName<T>(
            this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .Must(fileName => fileName is null ||
                    FileValidationRules.AllowedExtensions.Contains(Path.GetExtension(fileName).ToLowerInvariant()))
                .WithMessage($"Invalid extension: {string.Join(", ", FileValidationRules.AllowedExtensions)}");
        }

        public static IRuleBuilderOptions<T, string?> MustBeValidImageContentType<T>(
            this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .Must(contentType => contentType is null || FileValidationRules.AllowedContentTypes.Contains(contentType.ToLowerInvariant()))
                .WithMessage($"Invalid content type: {string.Join(", ", FileValidationRules.AllowedContentTypes)}");
        }

        public static IRuleBuilderOptions<T, Stream?> MustNotExceedMaxImageSize<T>(
            this IRuleBuilder<T, Stream?> ruleBuilder)
        {
            return ruleBuilder
                .Must(stream => stream is null || stream.Length <= FileValidationRules.MaxFileSizeBytes)
                .WithMessage($"File size exceeds the limit of {FileValidationRules.MaxFileSizeBytes / 1024 / 1024} MB.");
        }
    }
}
