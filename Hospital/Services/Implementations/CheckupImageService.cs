using AutoMapper;
using Hospital.DTOs;
using Hospital.Models;
using Hospital.Repositories;
using Hospital.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Services.Implementations
{
    public class CheckupImageService : ICheckupImageService
    {
        private IRepository<CheckupImage>? _checkupImageRepository;
        private readonly RepositoryFactory _factory;
        private readonly IMapper _mapper;
        private readonly string _bucketName;
        private readonly IMinioStorageService _minio;

        private IRepository<CheckupImage> CheckupImageRepository => 
            _checkupImageRepository ??= _factory.CreateRepository<CheckupImage>();

        public CheckupImageService(
        RepositoryFactory factory,
        IMinioStorageService minio,
        IMapper mapper,
        IConfiguration config)
        {
            _factory = factory;
            _minio = minio;
            _mapper = mapper;
            _bucketName = config["Minio:BucketName"]!;
        }

        public async Task<CheckupImageDto> UploadImageAsync(CreateCheckupImageDto dto)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.File.FileName);
            await using var stream = dto.File.OpenReadStream();

            await _minio.UploadFileAsync(stream, fileName, dto.File.ContentType, _bucketName);

            var checkupImage = _mapper.Map<CheckupImage>(dto);
            checkupImage.FileName = fileName;

            await CheckupImageRepository.AddAsync(checkupImage);
            await CheckupImageRepository.SaveAsync();

            var resultDto = _mapper.Map<CheckupImageDto>(checkupImage);
            resultDto.ImageUrl = await _minio.GeneratePresignedUrlAsync(fileName, _bucketName);

            return resultDto;
        }

        public async Task<string> GetImageUrlAsync(int imageId)
        {
            var image = await CheckupImageRepository.GetAsync(imageId);
            if (image == null) throw new KeyNotFoundException($"CheckupImage with Id {imageId} not found");

            var url = await _minio.GeneratePresignedUrlAsync(image.FileName, _bucketName);
            return url;
        }

        public async Task<List<CheckupImageDto>> GetImagesByCheckupIdAsync(int checkupId)
        {
            var images = CheckupImageRepository
                .GetAll()
                .Where(img => img.CheckupId == checkupId);

            var imageList = await images.ToListAsync();

            var dtoList = _mapper.Map<List<CheckupImageDto>>(imageList);

            foreach (var dto in dtoList)
            {
                dto.ImageUrl = await _minio.GeneratePresignedUrlAsync(dto.FileName, _bucketName);
            }

            return dtoList;
        }
    }
}
