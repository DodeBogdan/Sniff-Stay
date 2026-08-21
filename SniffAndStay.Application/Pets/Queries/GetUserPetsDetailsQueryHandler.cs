using MediatR;
using Microsoft.EntityFrameworkCore;
using SniffAndStay.Application.Exceptions;
using SniffAndStay.Application.Interfaces.Persistence;
using SniffAndStay.Application.Interfaces.Security;
using SniffAndStay.Application.Pets.Response;

namespace SniffAndStay.Application.Pets.Queries
{
    public record GetUserPetsDetailsQuery() : IRequest<IEnumerable<PetResponse>>;

    public class GetUserPetsDetailsQueryHandler : IRequestHandler<GetUserPetsDetailsQuery, IEnumerable<PetResponse>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICurrentUserService _currentUserService;

        public GetUserPetsDetailsQueryHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService)
        {
            _applicationDbContext = applicationDbContext;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<PetResponse>> Handle(GetUserPetsDetailsQuery request, CancellationToken cancellationToken)
        {
            Guid userId = _currentUserService.UserId
                ?? throw new InvalidUserException("Invalid user.");

            List<PetResponse> pets = await _applicationDbContext.Pets
                .Where(p => p.UserId == userId)
                .Select(p => PetResponse.FromPet(p))
                .ToListAsync(cancellationToken);

            return pets;
        }
    }
}
