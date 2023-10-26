using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2
{
    public static class Debug
    {
        private static string _logFilePath { get; set; } = "C:\\LimitlessSoft\\TDOffice_v2\\log.txt";

        static Debug()
        {
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(_logFilePath));
        }
        public static void Log(string line)
        {
            line = line.Replace("\r", Environment.NewLine);
            System.IO.File.AppendAllText(_logFilePath, line + Environment.NewLine);
        }
        public static void Log(string[] lines)
        {
            List<string> list = new List<string>();
            foreach(string line in lines)
                list.Add(line.Replace("\r", Environment.NewLine));
            list.Add(Environment.NewLine);
            System.IO.File.AppendAllLines(_logFilePath, list);
        }
    }
}
