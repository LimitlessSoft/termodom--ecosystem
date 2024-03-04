using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Npgsql;

namespace TD.Web.Common.DbMigrations.Helper
{
    public static class MigrationHelper
    {
        public static void ImportTableStructure(IConfigurationRoot config, string sourceTableName, string destinationTableName)
        {
            string sourceConnectionString = $"Server={config["LEGACY_DATABASE_SERVER"]};Database={config["LEGACY_DATABASE"]};Uid={config["LEGACY_DATABASE_USER"]};Pwd={config["LEGACY_DATABASE_PASSWORD"]};Pooling=false;SslMode=none;convert zero datetime=True;CharSet=utf8;";
            string destinationConnectionString = $"Server={config["POSTGRES_HOST"]};Port={config["POSTGRES_PORT"]};Userid={config["POSTGRES_USER"]};Password={config["POSTGRES_PASSWORD"]};Pooling=false;MinPoolSize=1;MaxPoolSize=20;Timeout=15;Database={config["POSTGRES_DATABASE"]};Include Error Detail=true;";

            string query = $"SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '{config["LEGACY_DATABASE"]}' AND TABLE_NAME = '{sourceTableName}'";

            using (MySqlConnection connection = new MySqlConnection(sourceConnectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        string createTableQuery = $"CREATE TABLE IF NOT EXISTS {destinationTableName} (";

                        while (reader.Read())
                        {
                            string columnName = reader["COLUMN_NAME"].ToString();
                            string dataType = reader["DATA_TYPE"].ToString();
                            int characterMaxLength = reader["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value ? Convert.ToInt32(reader["CHARACTER_MAXIMUM_LENGTH"]) : 0;
                            bool isNullable = reader["IS_NULLABLE"].ToString() == "YES";

                            if (dataType == "smallint" || dataType == "tinyint")
                                dataType = "int";

                            createTableQuery += $"{columnName} {dataType}";

                            if (characterMaxLength > 0)
                                createTableQuery += $"({characterMaxLength})";

                            if (!isNullable)
                                createTableQuery += " NOT NULL";

                            createTableQuery += ",";
                        }

                        createTableQuery = createTableQuery.TrimEnd(',') + ")";

                        using (NpgsqlConnection destinationConnection = new NpgsqlConnection(destinationConnectionString))
                        {
                            destinationConnection.Open();
                            using (NpgsqlCommand createTableCommand = new NpgsqlCommand(createTableQuery, destinationConnection))
                            {
                                createTableCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }

        public static void ImportData(IConfigurationRoot config, string sourceTableName, string destinationTableName)
        {
            string sourceConnectionString = $"Server={config["LEGACY_DATABASE_SERVER"]};Database={config["LEGACY_DATABASE"]};Uid={config["LEGACY_DATABASE_USER"]};Pwd={config["LEGACY_DATABASE_PASSWORD"]};Pooling=false;SslMode=none;convert zero datetime=True;CharSet=utf8;";
            string destinationConnectionString = $"Server={config["POSTGRES_HOST"]};Port={config["POSTGRES_PORT"]};Userid={config["POSTGRES_USER"]};Password={config["POSTGRES_PASSWORD"]};Pooling=false;MinPoolSize=1;MaxPoolSize=20;Timeout=15;Database={config["POSTGRES_DATABASE"]};Include Error Detail=true;";

            using (MySqlConnection sourceConnection = new MySqlConnection(sourceConnectionString))
            {
                sourceConnection.Open();

                string selectQuery = $"SELECT * FROM {sourceTableName}";
                MySqlCommand selectCommand = new MySqlCommand(selectQuery, sourceConnection);

                using (MySqlDataReader reader = selectCommand.ExecuteReader())
                {
                    using (NpgsqlConnection destinationConnection = new NpgsqlConnection(destinationConnectionString))
                    {
                        destinationConnection.Open();

                        string insertQuery = $"INSERT INTO {destinationTableName} (";

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var columnName = reader.GetName(i);
                            insertQuery += $"{columnName},";
                        }
                        insertQuery = insertQuery.TrimEnd(',') + ") VALUES ";

                        while (reader.Read())
                        {
                            insertQuery += "(";
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var value = reader.GetValue(i);
                                if (value == DBNull.Value || value == null || value.ToString() == "{}")
                                    insertQuery += $"null,";
                                else if (value is string || value is DateTime)
                                    insertQuery += $"'{value}',";
                                else
                                    insertQuery += $"{value},";
                            }
                            insertQuery = insertQuery.TrimEnd(',') + "),";
                        }
                        insertQuery = insertQuery.TrimEnd(',');

                        NpgsqlCommand finalInsertCommand = new NpgsqlCommand(insertQuery, destinationConnection);
                        finalInsertCommand.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
