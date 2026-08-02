using MediatR;
using Microsoft.EntityFrameworkCore;
using SniffAndStay.Application.Interfaces.Persistence;
using SniffAndStay.Application.Pets.Response;

namespace SniffAndStay.Application.Pets.Queries
{
    public record GetUserPetsDetailsQuery(Guid UserId) : IRequest<IEnumerable<PetResponse>>;

    public class GetUserPetsDetailsQueryHandler : IRequestHandler<GetUserPetsDetailsQuery, IEnumerable<PetResponse>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public GetUserPetsDetailsQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<PetResponse>> Handle(GetUserPetsDetailsQuery request, CancellationToken cancellationToken)
        {
            if (!await _applicationDbContext.Users.AnyAsync(user => Equals(user.Id, request.UserId), cancellationToken))
            {
                throw new InvalidOperationException("User not found");
            }

            //TODO: Add checks for user preferences and privacy settings here if needed

            List<PetResponse> pets = await _applicationDbContext.Pets
                .Where(p => p.UserId == request.UserId)
                .Select(p => PetResponse.FromPet(p))
                .ToListAsync(cancellationToken);

            return pets;
        }
    }
}
