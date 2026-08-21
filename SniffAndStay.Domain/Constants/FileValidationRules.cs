using System;
using System.Collections.Generic;
using System.Text;

namespace SniffAndStay.Domain.Constants
{
    public static class FileValidationRules
    {
        public const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB
        public const int MaxNameLength = 100;

        public static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        public static readonly string[] AllowedContentTypes = { "image/jpeg", "image/png", "image/webp" };
    }
}
