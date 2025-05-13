namespace TD.Web.Public.Contracts.Dtos.Vault;

public class SecretsDto
{
	public string DEPLOY_ENV { get; set; }
	public string JWT_AUDIENCE { get; set; }
	public string JWT_ISSUER { get; set; }
	public string JWT_KEY { get; set; }
	public string POSTGRES_HOST { get; set; }
	public string POSTGRES_PASSWORD { get; set; }
	public int POSTGRES_PORT { get; set; }
	public string POSTGRES_USER { get; set; }
	public int MINIO_PORT { get; set; }
	public string MINIO_ACCESS_KEY { get; set; }
	public string MINIO_SECRET_KEY { get; set; }
	public string MINIO_HOST { get; set; }
}
