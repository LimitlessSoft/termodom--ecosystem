using Minio;
using Minio.Exceptions;
using System.IO;
using System.Net.Mime;
using TD.Core.Contracts.Dtos;
using TD.Core.Contracts.Http;

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

        public async Task UploadAsync(Stream fileStream, string fileName, string contentType, Dictionary<string, string> tags = null)
        {
            fileName = fileName.Replace(Path.DirectorySeparatorChar, Contracts.Constants.Minio.DictionarySeparatorChar);
            if (fileName[0] != Contracts.Constants.Minio.DictionarySeparatorChar)
                fileName = Contracts.Constants.Minio.DictionarySeparatorChar + fileName;

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

            if (tags != null)
                uploadObj.WithTagging(new Minio.DataModel.Tags.Tagging(tags, false));

            await client.PutObjectAsync(uploadObj).ConfigureAwait(false);
        }

        public async Task<Response<FileDto>> DownloadAsync(string file)
        {
            file = file.Replace(Path.DirectorySeparatorChar, Contracts.Constants.Minio.DictionarySeparatorChar);

            var response = new Response<FileDto>();
            var client = new MinioClient()
                .WithEndpoint($"{_host}:{_port}")
                .WithCredentials(_accessKey, _secretKey)
                .Build();

            try
            {
                var statObjectArgs = new StatObjectArgs()
                    .WithBucket(_bucketBase)
                    .WithObject(file);

                await client.StatObjectAsync(statObjectArgs).ConfigureAwait(false);
            }
            catch
            {
                response.Status = System.Net.HttpStatusCode.NotFound;
                return response;
            }

            var ms = new MemoryStream();
            var getArgs = new GetObjectArgs()
                .WithBucket(_bucketBase)
                .WithObject(file)
                .WithCallbackStream((stream) =>
                {
                    stream.CopyTo(ms);
                });

            var tagsArgs = new GetObjectTagsArgs()
                .WithBucket(_bucketBase)
                .WithObject(file);

            var r = await client.GetObjectAsync(getArgs).ConfigureAwait(false);
            var tags = await client.GetObjectTagsAsync(tagsArgs).ConfigureAwait(false);

            response.Payload = new FileDto()
            {
                Data = ms.ToArray(),
                ContentType = r.ContentType,
                Tags = tags == null || tags.Tags == null ? new Dictionary<string, string>() : new Dictionary<string, string>(tags.Tags)
            };
            return response;
        }
    }
}
