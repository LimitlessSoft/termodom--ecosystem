using TD.Web.Common.Contracts.Interfaces.IManagers;
using LSCore.Contracts.SettingsModels;
using LSCore.Contracts.Exceptions;
using LSCore.Contracts.Dtos;
using Minio.DataModel.Args;
using LSCore.Contracts;
using Minio;

namespace TD.Web.Common.Domain.Managers;

public class MinioManager(LSCoreMinioSettings settings) : IMinioManager
{
    public async Task UploadAsync(Stream fileStream, string fileName, string contentType, Dictionary<string, string>? tags = null)
    {
        fileName = fileName.Replace(Path.DirectorySeparatorChar, LSCoreContractsConstants.Minio.DictionarySeparatorChar);
        if (fileName[0] != LSCoreContractsConstants.Minio.DictionarySeparatorChar)
            fileName = LSCoreContractsConstants.Minio.DictionarySeparatorChar + fileName;

        var client = new MinioClient()
            .WithEndpoint($"{settings.Host}:{settings.Port}")
            .WithCredentials(settings.AccessKey, settings.SecretKey)
            .Build();

        var be = new BucketExistsArgs()
            .WithBucket(settings.BucketBase);

        bool found = await client.BucketExistsAsync(be).ConfigureAwait(false);
        if (!found)
        {
            var mb = new MakeBucketArgs()
                .WithBucket(settings.BucketBase);

            await client.MakeBucketAsync(mb).ConfigureAwait(false);
        }

        var uploadObj = new PutObjectArgs()
            .WithBucket(settings.BucketBase)
            .WithObjectSize(fileStream.Length)
            .WithStreamData(fileStream)
            .WithObject(fileName)
            .WithContentType(contentType);

        if (tags != null)
            uploadObj.WithTagging(new Minio.DataModel.Tags.Tagging(tags, false));

        await client.PutObjectAsync(uploadObj).ConfigureAwait(false);
    }

    public async Task<LSCoreFileDto> DownloadAsync(string file)
    {
        file = file.Replace(Path.DirectorySeparatorChar, LSCoreContractsConstants.Minio.DictionarySeparatorChar);

        var client = new MinioClient()
            .WithEndpoint($"{settings.Host}:{settings.Port}")
            .WithCredentials(settings.AccessKey, settings.SecretKey)
            .Build();

        try
        {
            var statObjectArgs = new StatObjectArgs()
                .WithBucket(settings.BucketBase)
                .WithObject(file);

            await client.StatObjectAsync(statObjectArgs).ConfigureAwait(false);
        }
        catch
        {
            throw new LSCoreNotFoundException();
        }

        var ms = new MemoryStream();
        var getArgs = new GetObjectArgs()
            .WithBucket(settings.BucketBase)
            .WithObject(file)
            .WithCallbackStream((stream) =>
            {
                stream.CopyTo(ms);
            });

        var tagsArgs = new GetObjectTagsArgs()
            .WithBucket(settings.BucketBase)
            .WithObject(file);

        var r = await client.GetObjectAsync(getArgs).ConfigureAwait(false);
        var tags = await client.GetObjectTagsAsync(tagsArgs).ConfigureAwait(false);

        return new LSCoreFileDto()
        {
            Data = ms.ToArray(),
            ContentType = r.ContentType,
            Tags = tags == null || tags.Tags == null ? new Dictionary<string, string>() : new Dictionary<string, string>(tags.Tags)
        };
    }
}