using MediatR;
using SniffAndStay.Application.Common.Extensions;
using SniffAndStay.Application.Exceptions;
using SniffAndStay.Application.Interfaces.Common;
using SniffAndStay.Application.Interfaces.Persistence;
using SniffAndStay.Application.Interfaces.Security;
using SniffAndStay.Application.Users.DTOs;
using SniffAndStay.Application.Users.Response;
using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Application.Users.Commands
{
    public record AddOrUpdateUserProfileCommand(UserProfileRequest UpdateProfileRequest, Stream? File, string? FileName) : IRequest<UserProfileResponse>;
    public class AddOrUpdateUserProfileCommandHandler : IRequestHandler<AddOrUpdateUserProfileCommand, UserProfileResponse>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFileStorageService _fileStorageService;
        private readonly IFilePathService _filePathService;

        public AddOrUpdateUserProfileCommandHandler(
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

        public async Task<UserProfileResponse> Handle(AddOrUpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            User user = await _currentUserService.GetUserAsync(IncludeType.Details, cancellationToken)
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
                string fileNameToSave = _filePathService.ResolveFileName(user.Id, request.FileName!);
                string filePathToSave = _filePathService.ResolveFilePath(fileNameToSave, FileType.ProfilePicture);
                await _fileStorageService.SaveFileAsync(filePathToSave, request.File, cancellationToken);
                
                if (!user.UserDetails.ProfilePicture.IsNullOrEmpty())
                {
                    string fileNameToDelete = _filePathService.ResolveFileName(user.Id, user.UserDetails.ProfilePicture!);
                    string filePathToDelete = _filePathService.ResolveFilePath(fileNameToDelete, FileType.ProfilePicture);
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
