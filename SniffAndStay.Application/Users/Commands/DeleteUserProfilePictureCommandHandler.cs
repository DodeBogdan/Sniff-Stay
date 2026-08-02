using MediatR;
using SniffAndStay.Application.Common.Extensions;
using SniffAndStay.Application.Interfaces.Common;
using SniffAndStay.Application.Interfaces.Persistence;
using SniffAndStay.Application.Interfaces.Security;
using SniffAndStay.Application.Users.Response;
using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Application.Users.Commands
{
    public record DeleteUserProfilePictureCommand(Guid? userId) : IRequest<UserProfileResponse>;
    public class DeleteUserProfilePictureCommandHandler : IRequestHandler<DeleteUserProfilePictureCommand, UserProfileResponse>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFileStorageService _fileStorageService;
        private readonly IFilePathService _filePathService;
        public DeleteUserProfilePictureCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IFileStorageService fileStorageService,
            IFilePathService filePathService)
        {
            _applicationDbContext = applicationDbContext;
            _currentUserService = currentUserService;
            _fileStorageService = fileStorageService;
            _filePathService = filePathService;
        }

        public async Task<UserProfileResponse> Handle(DeleteUserProfilePictureCommand request, CancellationToken cancellationToken)
        {
            User user = await _currentUserService.GetUserAsync(IncludeType.Details, cancellationToken);

            if (request.userId.HasValue && user.Id != request.userId && !string.Equals(_currentUserService.Role, "Admin"))
            {
                throw new UnauthorizedAccessException("You are not authorized to delete this user's profile picture.");
            }

            if (user.UserDetails is null || user.UserDetails.ProfilePicture.IsNullOrEmpty())
            {
                return UserProfileResponse.FromUserDetails(user);
            }

            string fileNameToDelete = _filePathService.ResolveFileName(user.Id, user.UserDetails.ProfilePicture!);
            string filePathToDelete = _filePathService.ResolveFilePath(fileNameToDelete, FileType.ProfilePicture);
            await _fileStorageService.DeleteFileAsync(filePathToDelete, cancellationToken);

            user.UserDetails.ProfilePicture = null;

            _applicationDbContext.Users.Update(user);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return UserProfileResponse.FromUserDetails(user);
        }
    }
}
