namespace TD.Office.InterneOtpremnice.Contracts.Dtos.Vault;

public class SecretsDto
{
	public string DEPLOY_ENV { get; set; }
	public string POSTGRES_HOST { get; set; }
	public string POSTGRES_PASSWORD { get; set; }
	public int POSTGRES_PORT { get; set; }
	public string POSTGRES_USER { get; set; }
	public string[] API_KEYS { get; set; }
	public string OFFICE_API_BASE_URL { get; set; }
	public string OFFICE_API_KEY { get; set; }
}
