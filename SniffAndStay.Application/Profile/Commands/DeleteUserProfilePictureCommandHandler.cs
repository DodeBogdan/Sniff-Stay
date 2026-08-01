using MediatR;
using SniffAndStay.Application.Common.Extensions;
using SniffAndStay.Application.Interfaces.Common;
using SniffAndStay.Application.Interfaces.Persistence;
using SniffAndStay.Application.Interfaces.Security;
using SniffAndStay.Application.Profile.Response;
using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Application.Profile.Commands
{
    public record DeleteUserProfilePictureCommand() : IRequest<UserProfileResponse>;
    public class DeleteUserProfilePictureCommandHandler : IRequestHandler<DeleteUserProfilePictureCommand, UserProfileResponse>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFileStorageService _fileStorageService;

        public DeleteUserProfilePictureCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IFileStorageService fileStorageService)
        {
            _applicationDbContext = applicationDbContext;
            _currentUserService = currentUserService;
            _fileStorageService = fileStorageService;
        }

        public async Task<UserProfileResponse> Handle(DeleteUserProfilePictureCommand request, CancellationToken cancellationToken)
        {
            User user = await _currentUserService.GetUserWithDetailsAsync(cancellationToken);

            if (user.UserDetails is null)
            {
                return UserProfileResponse.FromUserDetails(user);
            }

            if (!user.UserDetails.ProfilePicture.IsNullOrEmpty())
            {
                string filePathToDelete = Path.Combine(user.Id.ToString(), user.UserDetails.ProfilePicture!);
                await _fileStorageService.DeleteFileAsync(filePathToDelete, cancellationToken);
            }

            user.UserDetails.ProfilePicture = null;

            _applicationDbContext.Users.Update(user);
            await _applicationDbContext.SaveChangesAsync();
            return UserProfileResponse.FromUserDetails(user);
        }
    }
}
