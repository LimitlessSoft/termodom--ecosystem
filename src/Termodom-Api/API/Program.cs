using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace API
{
    #pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Program
    {
        public static string ConnectionString { get; set; } = "Server=174.138.184.42;Database=termodom_db_main;Uid=masdos_mdoas;Pwd=j1cnH38$;Pooling=false;SslMode=none;convert zero datetime=True;";
        public static string ConnectionStringMagacin { get; set; } = "Server=174.138.184.42;Database=termodom_magacin;Uid=tdmagacin;Pwd=m2n^M92w;Pooling=false;SslMode=none";
        public static string ConnectionStringWebshop { get; set; } = "Server=174.138.184.42;Database=termodom_webshop;Uid=homotomo333;Pwd=01W#l6jy;Pooling=false;SslMode=none;convert zero datetime=True;";

        private static string sessionsPath = Path.Combine(AppContext.BaseDirectory, "sessions.txt");

        public static Dictionary<int, string> GetSessions()
        {
            if (!File.Exists(sessionsPath))
                File.WriteAllText(sessionsPath, JsonConvert.SerializeObject(new Dictionary<int, string>()));

            return JsonConvert.DeserializeObject<Dictionary<int, string>>(File.ReadAllText(sessionsPath));
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
