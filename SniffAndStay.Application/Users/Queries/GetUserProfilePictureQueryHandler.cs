using MediatR;
using SniffAndStay.Application.Exceptions;
using SniffAndStay.Application.Interfaces.Common;
using SniffAndStay.Application.Interfaces.Security;
using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Application.Users.Queries
{
    public record GetUserProfilePictureQuery() : IRequest<Stream?>;
    public class GetUserProfilePictureQueryHandler : IRequestHandler<GetUserProfilePictureQuery, Stream?>
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFilePathService _filePathService;
        public GetUserProfilePictureQueryHandler(
            IFileStorageService fileStorageService,
            ICurrentUserService currentUserService,
            IFilePathService filePathService)
        {
            _fileStorageService = fileStorageService;
            _currentUserService = currentUserService;
            _filePathService = filePathService;
        }

        public async Task<Stream?> Handle(GetUserProfilePictureQuery request, CancellationToken cancellationToken)
        {
            User user = await _currentUserService.GetUserAsync(IncludeType.Details, cancellationToken);

            if (user.UserDetails == null || string.IsNullOrEmpty(user.UserDetails.ProfilePicture))
            {
                throw new NotFoundException("User profile picture not found.");
            }

            string fileName = _filePathService.ResolveFileName(user.Id, user.UserDetails.ProfilePicture);
            string filePath = _filePathService.ResolveFilePath(fileName, FileType.ProfilePicture);

            return await _fileStorageService.GetFileAsync(filePath, cancellationToken);
        }
    }
}
