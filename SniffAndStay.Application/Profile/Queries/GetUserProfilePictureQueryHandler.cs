using MediatR;
using SniffAndStay.Application.Common.Exceptions;
using SniffAndStay.Application.Interfaces.Common;
using SniffAndStay.Application.Interfaces.Security;
using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Application.Profile.Queries
{
    public record GetUserProfilePictureQuery() : IRequest<Stream?>;
    public class GetUserProfilePictureQueryHandler : IRequestHandler<GetUserProfilePictureQuery, Stream?>
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly ICurrentUserService _currentUserService;

        public GetUserProfilePictureQueryHandler(
            IFileStorageService fileStorageService,
            ICurrentUserService currentUserService)
        {
            _fileStorageService = fileStorageService;
            _currentUserService = currentUserService;
        }

        public async Task<Stream?> Handle(GetUserProfilePictureQuery request, CancellationToken cancellationToken)
        {
            User user = await _currentUserService.GetUserWithDetails(cancellationToken);

            if (user.UserDetails == null || string.IsNullOrEmpty(user.UserDetails.ProfilePicture))
            {
                throw new NotFoundException("User profile picture not found.");
            }

            return await _fileStorageService.GetFileAsync(Path.Combine(user.Id.ToString(), user.UserDetails.ProfilePicture), cancellationToken);
        }
    }
}
