using MediatR;
using Microsoft.Extensions.Options;
using SniffAndStay.Application.Common.Exceptions;
using SniffAndStay.Application.Configuration;
using SniffAndStay.Application.Interfaces.Common;
using SniffAndStay.Application.Interfaces.Security;
using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Application.Profile.Queries
{
    public record GetUserProfilePictureQuery() : IRequest<Stream?>;
    public class GetUserProfilePictureQueryHandler : IRequestHandler<GetUserProfilePictureQuery, Stream?>
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly FileStorageConfig _fileStorageConfig;
        private readonly ICurrentUserService _currentUserService;

        public GetUserProfilePictureQueryHandler(
            IFileStorageService fileStorageService,
            IOptions<FileStorageConfig> fileStorageConfig,
            ICurrentUserService currentUserService)
        {
            _fileStorageService = fileStorageService;
            _fileStorageConfig = fileStorageConfig.Value;
            _currentUserService = currentUserService;
        }

        public async Task<Stream?> Handle(GetUserProfilePictureQuery request, CancellationToken cancellationToken)
        {
            User user = await _currentUserService.GetUserWithDetails(cancellationToken);

            if (user.UserDetails == null || string.IsNullOrEmpty(user.UserDetails.ProfilePicture))
            {
                throw new NotFoundException("User profile picture not found.");
            }

            string fullPath = Path.Combine(_fileStorageConfig.ProfilePicturePath, user.UserDetails.ProfilePicture);
            return await _fileStorageService.GetFileAsync(fullPath, cancellationToken);
        }
    }
}
