namespace SniffAndStay.Application.Authentication.Commands.BackdoorUser
{
    public class DefaultUserConfiguration
    {
        public static string SectionName { get; set; } = "DefaultUser";
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
