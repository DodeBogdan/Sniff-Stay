using System;
using System.Collections.Generic;
using System.Text;

namespace SniffAndStay.Application.Interfaces.Security
{
    public interface IJwtService
    {
        string GenerateToken(Guid userId, string email);
    }
}
