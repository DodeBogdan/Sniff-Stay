using MediatR;
using Microsoft.EntityFrameworkCore;
using SniffAndStay.Application.Interfaces.Persistence;
using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Application.Authentication.Commands
{
    public record DeleteUserCommand(string? Email) : IRequest;

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

            if (user == null)
            {
                await _context.Users.ExecuteDeleteAsync(cancellationToken);
            }
            else
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
