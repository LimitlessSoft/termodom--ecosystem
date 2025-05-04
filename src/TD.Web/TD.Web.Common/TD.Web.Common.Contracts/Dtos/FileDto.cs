namespace TD.Web.Common.Contracts.Dtos;

public class FileDto
{
	public required byte[] Data { get; set; }
	public required string ContentType { get; set; }
	public Dictionary<string, string>? Tags { get; set; }
}
