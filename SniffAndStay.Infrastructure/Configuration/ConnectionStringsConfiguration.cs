namespace SniffAndStay.Infrastructure.Configuration
{
    internal class ConnectionStringsConfiguration
    {
        public const string SectionName = "ConnectionStrings";

        public string DatabaseConnection { get; set; } = default!;
        public string LoggingConnection { get; set; } = default!;
    }
}
