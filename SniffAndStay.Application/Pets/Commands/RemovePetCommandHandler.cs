using MediatR;
using SniffAndStay.Application.Interfaces.Common;
using SniffAndStay.Application.Interfaces.Persistence;
using SniffAndStay.Application.Interfaces.Security;
using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Application.Pets.Commands
{
    public record RemovePetCommand(Guid PetId) : IRequest;
    public class RemovePetCommandHandler : IRequestHandler<RemovePetCommand>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserService _userService;

        public RemovePetCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IUserService userService)
        {
            _applicationDbContext = applicationDbContext;
            _currentUserService = currentUserService;
            _userService = userService;
        }

        public async Task Handle(RemovePetCommand request, CancellationToken cancellationToken)
        {
            User user = await _userService
                .GetUserAsync(_currentUserService.UserId, IncludeType.None, cancellationToken);

            Pet pet = _applicationDbContext.Pets.FirstOrDefault(p => p.Id.Equals(request.PetId))
                ?? throw new InvalidDataException("Pet not found");

            if (pet.UserId != user.Id)
            {
                throw new UnauthorizedAccessException("You are not authorized to remove this pet");
            }

            _applicationDbContext.Pets.Remove(pet);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
