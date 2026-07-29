using System;
using System.Collections.Generic;
using System.Text;

namespace SniffAndStay.Application.Interfaces.Security
{
    public interface IPasswordHasherService
    {
        string Hash(string password);

        bool Verify(string password, string passwordHash);
    }
}
