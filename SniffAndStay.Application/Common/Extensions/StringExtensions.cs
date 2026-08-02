namespace SniffAndStay.Application.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string? value)
        {
            return value == null || string.IsNullOrEmpty(value);
        }
    }
}
