using MediatR;
using Microsoft.Extensions.Options;
using SniffAndStay.Application.Common.Exceptions;
using SniffAndStay.Application.Common.Extensions;
using SniffAndStay.Application.Configuration;
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
        private readonly FileStorageConfig _fileStorageConfig;

        public UpdateUserProfileCommandHandler(
            IApplicationDbContext applicationDbContext,
            ICurrentUserService currentUserService,
            IFileStorageService fileStorageService,
            IOptions<FileStorageConfig> fileStorageConfig)
        {
            _applicationDbContext = applicationDbContext;
            _currentUserService = currentUserService;
            _fileStorageService = fileStorageService;
            _fileStorageConfig = fileStorageConfig.Value;
        }

        public async Task<UserProfileResponse> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            User user = await _currentUserService.GetUserWithDetails(cancellationToken)
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
                string profilePicturePath = string.Format(_fileStorageConfig.ProfilePicturePath, user.Id);
                await _fileStorageService.SaveFileAsync(profilePicturePath, request.FileName!, request.File, cancellationToken);
                
                if (!user.UserDetails.ProfilePicture.IsNullOrEmpty())
                {
                    await _fileStorageService.DeleteFileAsync(Path.Combine(profilePicturePath, user.UserDetails.ProfilePicture!), cancellationToken);
                }

                user.UserDetails.ProfilePicture = request.FileName!;
            }

            _applicationDbContext.Users.Update(user);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return UserProfileResponse.FromUserDetails(user);
        }
    }
}
