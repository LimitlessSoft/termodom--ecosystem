using Minio;

namespace TD.Core.Domain.Managers
{
    public class MinioManager
    {
        private readonly string _bucketBase;
        private readonly string _host;
        private readonly string _accessKey;
        private readonly string _secretKey;
        private readonly string _port;

        public MinioManager(string BucketBase, string host, string accessKey, string secretKey, string port)
        {
            _bucketBase = BucketBase.ToLower();
            _host = host;
            _accessKey = accessKey;
            _secretKey = secretKey;
            _port = port;
        }

        public async Task Upload(Stream fileStream, string fileName, string contentType)
        {
            var client = new MinioClient()
                .WithEndpoint($"{_host}:{_port}")
                .WithCredentials(_accessKey, _secretKey)
                .Build();

            var be = new BucketExistsArgs()
                .WithBucket(_bucketBase);

            bool found = await client.BucketExistsAsync(be).ConfigureAwait(false);
            if(!found)
            {
                var mb = new MakeBucketArgs()
                    .WithBucket(_bucketBase);

                await client.MakeBucketAsync(mb).ConfigureAwait(false);
            }

            var uploadObj = new PutObjectArgs()
                .WithBucket(_bucketBase)
                .WithObjectSize(fileStream.Length)
                .WithStreamData(fileStream)
                .WithObject(fileName)
                .WithContentType(contentType);

            await client.PutObjectAsync(uploadObj).ConfigureAwait(false);
        }
    }
}
