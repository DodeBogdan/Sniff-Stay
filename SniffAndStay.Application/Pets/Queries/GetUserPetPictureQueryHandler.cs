using MediatR;
using Microsoft.EntityFrameworkCore;
using SniffAndStay.Application.Interfaces.Common;
using SniffAndStay.Application.Interfaces.Persistence;
using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Application.Pets.Queries
{
    public record GetUserPetPictureQuery(Guid UserId, Guid PetId) : IRequest<Stream?>;
    public class GetUserPetPictureQueryHandler : IRequestHandler<GetUserPetPictureQuery, Stream?>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IFileStorageService _fileStorageService;
        private readonly IFilePathService _filePathService;

        public GetUserPetPictureQueryHandler(
            IApplicationDbContext applicationDbContext,
            IFileStorageService fileStorageService,
            IFilePathService filePathService)
        {
            _applicationDbContext = applicationDbContext;
            _fileStorageService = fileStorageService;
            _filePathService = filePathService;
        }

        public async Task<Stream?> Handle(GetUserPetPictureQuery request, CancellationToken cancellationToken)
        {
            if (!await _applicationDbContext.Users.AnyAsync(user => Equals(user.Id, request.UserId), cancellationToken))
            {
                throw new InvalidOperationException("User not found");
            }

            //TODO: Add check for user preferences for pet picture visibility

            Pet pet = await _applicationDbContext.Pets
                .FirstOrDefaultAsync(p => Equals(p.Id, request.PetId)
                && Equals(p.UserId, request.UserId), cancellationToken)
                ?? throw new InvalidOperationException("Pet not found");
    
            if(string.IsNullOrEmpty(pet.Picture))
            {
                return null;
            }

            string fileName = _filePathService.ResolveFileName(request.UserId, pet.Picture);
            string filePath = _filePathService.ResolveFilePath(fileName, FileType.PetPicture);
            return await _fileStorageService.GetFileAsync(filePath, cancellationToken);
        }
    }
}
