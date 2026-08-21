using MediatR;
using SniffAndStay.Application.Interfaces.Common;
using SniffAndStay.Application.Interfaces.Security;
using SniffAndStay.Application.Users.Response;
using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Application.Users.Queries
{
    public record GetUserProfileDetailsQuery() : IRequest<UserProfileResponse>;
    public class GetUserProfileDetailsQueryHandler : IRequestHandler<GetUserProfileDetailsQuery, UserProfileResponse>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserService _userService;

        public GetUserProfileDetailsQueryHandler(
            ICurrentUserService currentUserService,
            IUserService userService)
        {
            _currentUserService = currentUserService;
            _userService = userService;
        }

        public async Task<UserProfileResponse> Handle(GetUserProfileDetailsQuery request, CancellationToken cancellationToken)
        {
            User user = await _userService.GetUserAsync(_currentUserService.UserId, IncludeType.Details, cancellationToken);

            return UserProfileResponse.FromUserDetails(user);
        }
    }
}
