using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Application.Interfaces.Security
{
    public enum IncludeType
    {
        None,
        Details,
        Pets,
        All
    }

    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        string? Email { get; }
        string? Role { get; }
        bool IsAuthenticated { get; }
    }
}
