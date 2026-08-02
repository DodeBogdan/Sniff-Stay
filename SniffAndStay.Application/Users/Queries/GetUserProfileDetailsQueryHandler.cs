using MediatR;
using SniffAndStay.Application.Interfaces.Security;
using SniffAndStay.Application.Users.Response;
using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Application.Users.Queries
{
    public record GetUserProfileDetailsQuery() : IRequest<UserProfileResponse>;
    public class GetUserProfileDetailsQueryHandler : IRequestHandler<GetUserProfileDetailsQuery, UserProfileResponse>
    {
        private readonly ICurrentUserService _currentUserService;

        public GetUserProfileDetailsQueryHandler(
            ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public async Task<UserProfileResponse> Handle(GetUserProfileDetailsQuery request, CancellationToken cancellationToken)
        {
            User user = await _currentUserService.GetUserAsync(IncludeType.Details, cancellationToken);

            return UserProfileResponse.FromUserDetails(user);
        }
    }
}
