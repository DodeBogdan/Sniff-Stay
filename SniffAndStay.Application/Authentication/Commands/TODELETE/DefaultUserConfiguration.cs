using System;
using System.Collections.Generic;
using System.Text;

namespace SniffAndStay.Application.Authentication.Commands.TODELETE
{
    public class DefaultUserConfiguration
    {
        public static string SectionName { get; set; } = "DefaultUser";
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
