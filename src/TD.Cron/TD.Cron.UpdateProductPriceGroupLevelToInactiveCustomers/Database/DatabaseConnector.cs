using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using TD.Cron.UpdateProductPriceGroupLevelToInactiveCustomers.Helpers;

namespace TD.Cron.UpdateProductPriceGroupLevelToInactiveCustomers.Database;
public class DatabaseConnector
{
    public static NpgsqlConnection CreateConnection(IConfigurationRoot configurationRoot)
    {
        // Generate the connection string
        string connectionString = dbHelper.CreateConnectionString(configurationRoot);

        // Create and return the NpgsqlConnection object
        var connection = new NpgsqlConnection(connectionString);

        try
        {
            connection.Open();
            Console.WriteLine("Successfully connected to the database.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to the database: {ex.Message}");
        }

        return connection;
    }
}
