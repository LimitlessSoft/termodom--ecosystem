using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace API
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
	public class Program
	{
		// Plivanje333
		public static string ConnectionString { get; set; } =
			"Server=mysql6008.site4now.net;Database=db_a997a5_tdmain;Uid=a997a5_tdmain;Pwd=Plivanje333;Pooling=false;SslMode=none;convert zero datetime=True;CharSet=utf8;";
		public static string ConnectionStringMagacin { get; set; } = "null";
		public static string ConnectionStringWebshop { get; set; } =
			"Server=mysql6008.site4now.net;Database=db_a997a5_tdshop;Uid=a997a5_tdshop;Pwd=Plivanje333;Pooling=false;SslMode=none;convert zero datetime=True;CharSet=utf8;";

		private static string sessionsPath = Path.Combine(AppContext.BaseDirectory, "sessions.txt");

		public static Dictionary<int, string> GetSessions()
		{
			if (!File.Exists(sessionsPath))
				File.WriteAllText(
					sessionsPath,
					JsonConvert.SerializeObject(new Dictionary<int, string>())
				);

			return JsonConvert.DeserializeObject<Dictionary<int, string>>(
				File.ReadAllText(sessionsPath)
			);
		}

		public static void AddSession(int userId, string bearer)
		{
			var sessions = GetSessions();
			sessions[userId] = bearer;
			File.WriteAllText(sessionsPath, JsonConvert.SerializeObject(sessions));
		}

		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
