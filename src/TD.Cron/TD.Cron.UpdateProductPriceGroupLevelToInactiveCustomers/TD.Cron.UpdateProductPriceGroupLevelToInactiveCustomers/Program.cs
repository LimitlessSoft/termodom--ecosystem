using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using TD.Cron.UpdateProductPriceGroupLevelToInactiveCustomers.Constants;
using TD.Cron.UpdateProductPriceGroupLevelToInactiveCustomers.Database;
using TD.Cron.UpdateProductPriceGroupLevelToInactiveCustomers.Helpers;

var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

var configurationRoot = builder.Build();

var level = 0;
var inactiveDays = 90;
var execute = false;

using (var connection = DatabaseConnector.CreateConnection(configurationRoot))
{
    try
    {

        using (var command = new NpgsqlCommand(dbHelper.GetSettingKeysQuery(), connection))
        {
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var key = reader["Key"].ToString();
                        var value = reader["Value"].ToString();
                        if (key == Constants._updateLevelKey)
                            level = Convert.ToInt32(value);
                        else if (key == Constants._inactiveDaysKey)
                            inactiveDays = Convert.ToInt32(value);
                        else if (key == Constants._inactiveStatusKey)
                            execute = value.Equals("true", StringComparison.OrdinalIgnoreCase);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
            }
        }

        if(execute)
            using (var command = new NpgsqlCommand(dbHelper.GetUpdateQuery(level, inactiveDays), connection))
                command.ExecuteNonQuery();
        connection.Close();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

return 0;