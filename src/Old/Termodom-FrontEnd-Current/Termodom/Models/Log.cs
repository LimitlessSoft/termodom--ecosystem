using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Termodom.Models
{
	public class LogovanjeGreske
	{
		public string Messages { get; set; }
		public string Date { get; set; }
	}

	public static class Log
	{
		private static string _folderName = "Logs";
		private static string _fileName = "error.txt";
		public static object _lock = new object();

		public static void WriteAsync(LogovanjeGreske log)
		{
			Task.Run(() =>
			{
				lock (_lock)
				{
					List<LogovanjeGreske> logs = new List<LogovanjeGreske>();
					string path = Path.Combine(_folderName, _fileName);
					if (!Directory.Exists(_folderName))
						Directory.CreateDirectory(_folderName);

					if (!File.Exists(path))
						using (File.CreateText(path)) { }

					logs = JsonConvert.DeserializeObject<List<LogovanjeGreske>>(
						File.ReadAllText(path)
					);
					if (logs == null)
						logs = new List<LogovanjeGreske>();

					logs.Add(log);

					File.WriteAllText(path, JsonConvert.SerializeObject(logs));
				}
			});
		}
	}
}
