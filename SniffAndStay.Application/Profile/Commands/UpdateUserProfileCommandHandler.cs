using MediatR;
using SniffAndStay.Application.Common.Extensions;
using SniffAndStay.Application.Exceptions;
using SniffAndStay.Application.Interfaces.Common;
using SniffAndStay.Application.Interfaces.Persistence;
using SniffAndStay.Application.Interfaces.Security;
using SniffAndStay.Application.Profile.DTOs;
using SniffAndStay.Application.Profile.Response;
using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Application.Profile.Commands
{
    public record UpdateUserProfileCommand(UserProfileRequest UpdateProfileRequest, Stream? File, string? FileName) : IRequest<UserProfileResponse>;
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, UserProfileResponse>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFileStorageService _fileStorageService;

        public UpdateUserProfileCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IFileStorageService fileStorageService)
        {
            _applicationDbContext = applicationDbContext;
            _currentUserService = currentUserService;
            _fileStorageService = fileStorageService;
        }

        public async Task<UserProfileResponse> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            User user = await _currentUserService.GetUserWithDetailsAsync(cancellationToken)
                ?? throw new NotFoundException("User not found.");

            user.UserDetails ??= new UserDetails();

            if (!request.UpdateProfileRequest.FirstName.IsNullOrEmpty() && !string.Equals(user.UserDetails.FirstName, request.UpdateProfileRequest.FirstName, StringComparison.InvariantCultureIgnoreCase))
            {
                user.UserDetails.FirstName = request.UpdateProfileRequest.FirstName!;
            }

            if (!request.UpdateProfileRequest.LastName.IsNullOrEmpty() && !string.Equals(user.UserDetails.LastName, request.UpdateProfileRequest.LastName, StringComparison.InvariantCultureIgnoreCase))
            {
                user.UserDetails.LastName = request.UpdateProfileRequest.LastName!;
            }

            if (request.UpdateProfileRequest.Gender != null && user.UserDetails.Gender != request.UpdateProfileRequest.Gender)
            {
                user.UserDetails.Gender = request.UpdateProfileRequest.Gender.Value;
            }

            if (request.UpdateProfileRequest.DateOfBirth != null && !user.UserDetails.DateOfBirth.Equals(request.UpdateProfileRequest.DateOfBirth))
            {
                user.UserDetails.DateOfBirth = request.UpdateProfileRequest.DateOfBirth.Value;
            }

            if (request.File != null && !request.FileName.IsNullOrEmpty() && !string.Equals(user.UserDetails.ProfilePicture, request.FileName))
            {
                string filePathToSave = Path.Combine(user.Id.ToString(), request.FileName!);
                await _fileStorageService.SaveFileAsync(filePathToSave, request.File, cancellationToken);
                
                if (!user.UserDetails.ProfilePicture.IsNullOrEmpty())
                {
                    string filePathToDelete = Path.Combine(user.Id.ToString(), user.UserDetails.ProfilePicture!);
                    await _fileStorageService.DeleteFileAsync(filePathToDelete, cancellationToken);
                }

                user.UserDetails.ProfilePicture = request.FileName!;
            }

            _applicationDbContext.Users.Update(user);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return UserProfileResponse.FromUserDetails(user);
        }
    }
}
