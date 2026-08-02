using MediatR;
using SniffAndStay.Application.Common.Extensions;
using SniffAndStay.Application.Interfaces.Common;
using SniffAndStay.Application.Interfaces.Persistence;
using SniffAndStay.Application.Interfaces.Security;
using SniffAndStay.Application.Pets.DTOs;
using SniffAndStay.Application.Pets.Response;
using SniffAndStay.Domain.Entities;

namespace SniffAndStay.Application.Pets.Commands
{
    public record AddPetCommand(PetRequest PetRequest, Stream? File, string? FileName) : IRequest<PetResponse>;

    public class AddPetCommandHandler : IRequestHandler<AddPetCommand, PetResponse>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFileStorageService _fileStorageService;
        private readonly IFilePathService _filePathService;

        public AddPetCommandHandler(
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

        public async Task<PetResponse> Handle(AddPetCommand request, CancellationToken cancellationToken)
        {
            User user = await _currentUserService.GetUserAsync(IncludeType.None, cancellationToken);

            Pet pet = new()
            {
                Name = request.PetRequest.Name,
                UserId = user.Id,
                DateOfBirth = request.PetRequest.DateOfBirth,
                PetType = request.PetRequest.PetType
            };

            if (request.File != null && !request.FileName.IsNullOrEmpty())
            {
                string fileName = _filePathService.ResolveFileName(user.Id, request.FileName!);
                string filePathToSave = _filePathService.ResolveFilePath(fileName, FileType.PetPicture);
                await _fileStorageService.SaveFileAsync(filePathToSave, request.File, cancellationToken);

                pet.Picture = request.FileName!;
            }

            _applicationDbContext.Pets.Add(pet);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return PetResponse.FromPet(pet);
        }
    }
}
