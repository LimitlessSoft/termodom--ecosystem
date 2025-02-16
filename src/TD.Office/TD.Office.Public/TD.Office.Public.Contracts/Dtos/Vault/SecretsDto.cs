namespace TD.Office.Public.Contracts.Dtos.Vault;

public class SecretsDto
{
    public string DEPLOY_ENV { get; set; }
    public string JWT_AUDIENCE { get; set; }
    public string JWT_ISSUER { get; set; }
    public string JWT_KEY { get; set; }
    public string KOMERCIJALNO_TCMD_API_URL_CURRENT_YEAR { get; set; }
    public string KUBE_CONFIG_PURE { get; set; }
    public string POSTGRES_HOST { get; set; }
    public string POSTGRES_PASSWORD { get; set; }
    public int POSTGRES_PORT { get; set; }
    public string POSTGRES_USER { get; set; }
    public string TD_WEB_API_URL { get; set; }
    public string TD_WEB_ADMIN_API_KEY { get; set; }
}