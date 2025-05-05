using TD.Web.Common.Contracts.Dtos;

namespace TD.Web.Common.Contracts.Interfaces.IManagers;

public interface IMinioManager
{
	Task UploadAsync(
		Stream fileStream,
		string fileName,
		string contentType,
		Dictionary<string, string>? tags = null
	);

	Task<FileDto> DownloadAsync(string file);
}
