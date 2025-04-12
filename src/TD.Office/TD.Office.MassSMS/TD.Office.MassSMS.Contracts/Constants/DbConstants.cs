using Microsoft.Extensions.Configuration;

namespace TD.Office.MassSMS.Contracts.Constants;

public static class DbConstants
{
	public static string ConnectionString(IConfigurationRoot configurationRoot) =>
		$"Server={configurationRoot["POSTGRES_HOST"]};Port={configurationRoot["POSTGRES_PORT"]};Userid={configurationRoot["POSTGRES_USER"]};Password={configurationRoot["POSTGRES_PASSWORD"]};Pooling=false;MinPoolSize=1;MaxPoolSize=20;Timeout=15;Database={configurationRoot["DEPLOY_ENV"]}_tdoffice_mass_sms;Include Error Detail=true;";

	public static string MigrationAssemblyName => "TD.Office.MassSMS.DbMigrations";
}
