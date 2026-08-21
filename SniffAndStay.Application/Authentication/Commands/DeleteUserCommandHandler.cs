using MediatR;
using Microsoft.EntityFrameworkCore;
using SniffAndStay.Application.Interfaces.Persistence;
using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Application.Authentication.Commands
{
    public record DeleteUserCommand(string? Email, bool ShouldDeleteAllUsers = false) : IRequest;

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteUserCommandHandler(
            IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            User? user = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (request.ShouldDeleteAllUsers)
            {
                await _context.Users.ExecuteDeleteAsync(cancellationToken);
            }
            else if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
