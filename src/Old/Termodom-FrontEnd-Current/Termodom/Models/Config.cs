using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace Termodom.Models
{
	// TODO: Transfer U API
	public class Config<T>
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public T Value { get; set; }

		private static object ___insertLock = new object();

		public void Update()
		{
			using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
			{
				con.Open();
				Update(con);
			}
		}

		public void Update(MySqlConnection con)
		{
			using (
				MySqlCommand cmd = new MySqlCommand(
					"UPDATE CONFIG SET NAME = @N, VALUE = @V WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@N", Name);
				cmd.Parameters.AddWithValue("@V", Value);
				cmd.Parameters.AddWithValue("@ID", ID);

				cmd.ExecuteNonQuery();
			}
		}

		public static Config<T> Get(int id)
		{
			using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
			{
				con.Open();
				return Get(con, id);
			}
		}

		public static Config<T> Get(MySqlConnection con, int id)
		{
			using (
				MySqlCommand cmd = new MySqlCommand(
					"SELECT ID, NAME, VALUE FROM CONFIG WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ID", id);

				using (MySqlDataReader dr = cmd.ExecuteReader())
				{
					if (dr.Read())
					{
						return new Config<T>()
						{
							ID = Convert.ToInt32(dr["ID"]),
							Name = dr["NAME"].ToString(),
							Value = JsonConvert.DeserializeObject<T>(dr["VALUE"].ToString())
						};
					}
				}
			}

			return null;
		}

		public static Config<T> Get(string name)
		{
			using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
			{
				con.Open();
				return Get(con, name);
			}
		}

		public static Config<T> Get(MySqlConnection con, string name)
		{
			name = name.ToUpper();
			using (
				MySqlCommand cmd = new MySqlCommand(
					"SELECT ID, NAME, VALUE FROM CONFIG WHERE NAME = @NAME",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@NAME", name);

				using (MySqlDataReader dr = cmd.ExecuteReader())
				{
					if (dr.Read())
					{
						return new Config<T>()
						{
							ID = Convert.ToInt32(dr["ID"]),
							Name = dr["NAME"].ToString(),
							Value = JsonConvert.DeserializeObject<T>(dr["VALUE"].ToString())
						};
					}
				}
			}

			return null;
		}

		public static int Insert(string name, T value)
		{
			using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
			{
				con.Open();
				return Insert(con, name, value);
			}
		}

		public static int Insert(MySqlConnection con, string name, T value)
		{
			if (Get(name) != null)
				throw new Exception("Config with given name already exists!");

			lock (___insertLock)
			{
				int id = MaxID() + 1;

				using (
					MySqlCommand cmd = new MySqlCommand(
						"INSERT INTO CONFIG (ID, NAME, VALUE) VALUES (@ID, @NAME, @VALUE)",
						con
					)
				)
				{
					cmd.Parameters.AddWithValue("@ID", id);
					cmd.Parameters.AddWithValue("@NAME", name.ToUpper());
					cmd.Parameters.AddWithValue("@VALUE", value);

					cmd.ExecuteNonQuery();
				}
				return id;
			}
		}

		public static int MaxID()
		{
			using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
			{
				con.Open();
				return MaxID(con);
			}
		}

		public static int MaxID(MySqlConnection con)
		{
			using (MySqlCommand cmd = new MySqlCommand("SELECT MAX(ID) FROM CONFIG", con))
			using (MySqlDataReader dr = cmd.ExecuteReader())
				if (dr.Read())
					return Convert.ToInt32(dr[0]);

			return 0;
		}
	}
}
