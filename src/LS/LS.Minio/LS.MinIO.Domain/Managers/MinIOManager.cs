using LS.MinIO.Contracts;
using LS.MinIO.Contracts.Contracts;
using Minio;

namespace LS.MinIO.Domain.Managers
{
    public class MinIOManager : IMinIOManager
    {
        private readonly MinIOSettings _settings;
        public MinIOManager(MinIOSettings settings)
        {
            _settings = settings;
        }

        public async Task UploadAsync(string bucketName, string objectName, Stream fileStream, string contentType)
        {
            var minioClient = new MinioClient()
                .WithEndpoint(_settings.Endpoint)
                .WithCredentials(_settings.AccessKey, _settings.SecretKey)
                .WithSSL(false)
                .Build();

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithObjectSize(fileStream.Length)
                .WithStreamData(fileStream)
                .WithContentType(contentType);

            await minioClient.PutObjectAsync(putObjectArgs);
        }
    }
}
