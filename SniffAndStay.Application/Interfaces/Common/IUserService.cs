using SniffAndStay.Application.Interfaces.Security;
using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Application.Interfaces.Common
{
    public interface IUserService
    {
        Task<User> GetUserAsync(Guid? userId, IncludeType includeType, CancellationToken cancellationToken = default);
    }
}
