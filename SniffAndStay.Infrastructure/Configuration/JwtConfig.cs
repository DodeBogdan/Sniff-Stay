namespace SniffAndStay.Infrastructure.Configuration
{
    public class JwtConfig
    {
        public const string SectionName = "Jwt";

        public string Key { get; set; } = default!;
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public int ExpirationMinutes { get; set; }
    }
}
