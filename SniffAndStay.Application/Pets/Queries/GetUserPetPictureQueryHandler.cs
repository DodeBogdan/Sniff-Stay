using MediatR;
using Microsoft.EntityFrameworkCore;
using SniffAndStay.Application.Exceptions;
using SniffAndStay.Application.Interfaces.Common;
using SniffAndStay.Application.Interfaces.Persistence;
using SniffAndStay.Application.Interfaces.Security;
using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Application.Pets.Queries
{
    public record GetUserPetPictureQuery(Guid PetId) : IRequest<Stream?>;

    public class GetUserPetPictureQueryHandler : IRequestHandler<GetUserPetPictureQuery, Stream?>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFileStorageService _fileStorageService;
        private readonly IFilePathService _filePathService;

        public GetUserPetPictureQueryHandler(
            IApplicationDbContext applicationDbContext,
            IFileStorageService fileStorageService,
            IFilePathService filePathService,
            ICurrentUserService currentUserService)
        {
            _applicationDbContext = applicationDbContext;
            _fileStorageService = fileStorageService;
            _filePathService = filePathService;
            _currentUserService = currentUserService;
        }

        public async Task<Stream?> Handle(GetUserPetPictureQuery request, CancellationToken cancellationToken)
        {
            Guid userId = _currentUserService.UserId
                ?? throw new InvalidUserException("Invalid user.");

            Pet pet = await _applicationDbContext.Pets
                .FirstOrDefaultAsync(p => Equals(p.Id, request.PetId)
                && Equals(p.UserId, userId), cancellationToken)
                ?? throw new NotFoundException("Pet not found");
    
            if(string.IsNullOrEmpty(pet.Picture))
            {
                return null;
            }

            string fileName = _filePathService.ResolveFileName(userId, pet.Picture);
            string filePath = _filePathService.ResolveFilePath(fileName, FileType.PetPicture);
            return await _fileStorageService.GetFileAsync(filePath, cancellationToken);
        }
    }
}
