namespace TD.Web.Common.Contracts.Configurations;

public class MinioConfiguration
{
    public required string BucketBase { get; set; }
    public required string Host { get; set; }
    public required string AccessKey { get; set; }
    public required string SecretKey { get; set; }
    public required string Port { get; set; }
}