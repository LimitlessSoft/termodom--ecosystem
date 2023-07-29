namespace LS.MinIO.Contracts.Contracts
{
    public interface IMinIOManager
    {
        Task UploadAsync(string bucketName, string objectName, Stream fileStream, string contentType);
    }
}
