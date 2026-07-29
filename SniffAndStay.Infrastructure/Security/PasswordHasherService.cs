using Microsoft.AspNetCore.Identity;
using SniffAndStay.Application.Interfaces.Security;

namespace SniffAndStay.Infrastructure.Security
{
    public class PasswordHasherService : IPasswordHasherService
    {
        private readonly IPasswordHasher<object> _hasher;

        public PasswordHasherService(IPasswordHasher<object> hasher)
        {
            _hasher = hasher;
        }

        public string Hash(string password)
        {
            return _hasher.HashPassword(
                new object(),
                password);
        }

        public bool Verify(string password, string passwordHash)
        {
            var result = _hasher.VerifyHashedPassword(
                new object(),
                passwordHash,
                password);

            return result == PasswordVerificationResult.Success;
        }
    }
}
