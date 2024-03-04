using MySql.Data.MySqlClient;
using Npgsql;

namespace TD.Web.Common.ImportTool.Helper
{
    public static class TableHelper
    {
        public static void ImportUsersStructure()
        {
            string sourceConnectionString = "Server=mysql6008.site4now.net;Database=db_a997a5_tdmain;Uid=a997a5_tdmain;Pwd=Plivanje333;Pooling=false;SslMode=none;convert zero datetime=True;CharSet=utf8;";
            string destinationConnectionString = $"Server=139.177.181.216;Port=5432;Userid=postgres;Password=FFnF2JegHu0pt6RmBr5ib2mxRIuaCwNe;Pooling=false;MinPoolSize=1;MaxPoolSize=20;Timeout=15;Database=develop_web;Include Error Detail=true;";
            string sourceTableName = "korisnik";
            string sourceDatabaseName = "db_a997a5_tdmain";
            string destinationTableName = "old_users";

            string query = $"SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '{sourceDatabaseName}' AND TABLE_NAME = '{sourceTableName}'";

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

        public static void ImportUsersData()
        {
            string sourceConnectionString = "Server=mysql6008.site4now.net;Database=db_a997a5_tdmain;Uid=a997a5_tdmain;Pwd=Plivanje333;Pooling=false;SslMode=none;convert zero datetime=True;CharSet=utf8;";
            string destinationConnectionString = $"Server=139.177.181.216;Port=5432;Userid=postgres;Password=FFnF2JegHu0pt6RmBr5ib2mxRIuaCwNe;Pooling=false;MinPoolSize=1;MaxPoolSize=20;Timeout=15;Database=develop_web;Include Error Detail=true;";

            string sourceTableName = "korisnik";
            string destinationTableName = "old_users";

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

                        string insertQuery = $"INSERT INTO {destinationTableName} VALUES ";
                        while (reader.Read())
                        {
                            insertQuery += "(";
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var a = reader.GetValue(i);
                                if (a == DBNull.Value || a == null || a.ToString() == "{}")
                                    insertQuery += $"null,";
                                else if (a is string || a is DateTime)
                                    insertQuery += $"'{a}',";
                                else
                                    insertQuery += $"{a},";
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
