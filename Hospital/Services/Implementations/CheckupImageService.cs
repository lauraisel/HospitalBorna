using AutoMapper;
using Hospital.DTOs;
using Hospital.Models;
using Hospital.Repositories;
using Hospital.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Minio.DataModel.Args;
using Microsoft.Extensions.Configuration;

namespace Hospital.Services.Implementations
{
    public class CheckupImageService : ICheckupImageService
    {
        private IRepository<CheckupImage>? _checkupImageRepository;
        private readonly RepositoryFactory _factory;
        private readonly IMapper _mapper;
        private readonly IMinioStorageService _minio;
        private readonly string _bucketName;
        private readonly string _minioEndpoint;

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
            _minioEndpoint = config["Minio:Endpoint"]!;
        }

        public async Task<CheckupImageDto> UploadImageAsync(CreateCheckupImageDto dto)
        {
            bool found = await _minio.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucketName));
            if (!found)
            {
                await _minio.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.File.FileName);
            await using var stream = dto.File.OpenReadStream();

            await _minio.UploadFileAsync(stream, fileName, dto.File.ContentType, _bucketName);

            var checkupImage = _mapper.Map<CheckupImage>(dto);
            checkupImage.FileName = fileName;

            await CheckupImageRepository.AddAsync(checkupImage);
            await CheckupImageRepository.SaveAsync();

            var resultDto = _mapper.Map<CheckupImageDto>(checkupImage);
            resultDto.ImageUrl = GetPublicUrl(fileName);

            return resultDto;
        }

        public async Task<string> GetImageUrlAsync(int imageId)
        {
            var image = await CheckupImageRepository.GetAsync(imageId);
            if (image == null) throw new KeyNotFoundException($"CheckupImage with Id {imageId} not found");

            return GetPublicUrl(image.FileName);
        }

        public async Task<List<CheckupImageDto>> GetImagesByCheckupIdAsync(int checkupId)
        {
            var images = CheckupImageRepository
                .GetAll()
                .Where(img => img.CheckupId == checkupId);

            var imageList = await images.ToListAsync();

            var dtoList = new List<CheckupImageDto>();

            foreach (var image in imageList)
            {
                var dto = _mapper.Map<CheckupImageDto>(image);

                var stream = await _minio.GetFileAsync(image.FileName, _bucketName);

                if (stream != null)
                {
                    using var ms = new MemoryStream();
                    await stream.CopyToAsync(ms);
                    dto.FileContent = ms.ToArray();
                }

                dtoList.Add(dto);
            }

            return dtoList;
        }


        private string GetPublicUrl(string fileName)
        {
             
            var endpoint = _minioEndpoint.StartsWith("http") ? _minioEndpoint : $"http://{_minioEndpoint}";


            return $"{endpoint}/{_bucketName}/{fileName}";
        }


    }
}
