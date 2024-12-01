using Microsoft.Extensions.Configuration;

namespace TD.Cron.UpdateProductPriceGroupLevelToInactiveCustomers.Helpers;
public class dbHelper
{
    public static string CreateConnectionString(IConfigurationRoot configurationRoot)
    {
        var dbHost = configurationRoot["POSTGRES_HOST"];
        var dbPort = configurationRoot["POSTGRES_PORT"];
        var dbPassword = configurationRoot["POSTGRES_PASSWORD"];
        var dbUser = configurationRoot["POSTGRES_USER"];
        var dbName = configurationRoot["POSTGRES_DATABASE_NAME"];

        return $"Host={dbHost};Port={dbPort};Username={dbUser};Password={dbPassword};Database={dbName};";
    }

    public static string GetUpdateQuery(int level, int inactiveDays)
    {
        var query = $@"
            UPDATE ""ProductPriceGroupLevel""
            SET ""Level"" = {level}
            WHERE ""UserId"" IN (
                SELECT u.""Id""
                FROM ""Users"" u
                LEFT JOIN (
                    SELECT ""CreatedBy"", MAX(""CheckedOutAt"") AS ""LastOrderDate""
                    FROM ""Orders""
                    WHERE ""Status"" = 4
                    GROUP BY ""CreatedBy""
                ) o ON u.""Id"" = o.""CreatedBy""
                LEFT JOIN ""Orders"" o2 ON u.""Id"" = o2.""CreatedBy""
                WHERE (o.""LastOrderDate"" IS NOT NULL AND NOW() - o.""LastOrderDate"" > INTERVAL '{inactiveDays} days')
                   OR (o.""LastOrderDate"" IS NULL)
                GROUP BY u.""Id""
            );";

        return query;
    }

    public static string GetSettingKeysQuery()
    {
        var settingsKeysQuery = $@"
            SELECT * 
            FROM ""Settings"" 
            WHERE ""Key"" IN ('{Constants.Constants._inactiveStatusKey}', 
                      '{Constants.Constants._inactiveDaysKey}', 
                      '{Constants.Constants._updateLevelKey}')";

        return settingsKeysQuery;
    }
}
