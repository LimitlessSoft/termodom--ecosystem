using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDOffice_v2.Komercijalno
{
    public static class File
    {
        public static List<int> ProcitajRLT(string path)
        {
            if (!System.IO.File.Exists(path))
                throw new System.IO.FileNotFoundException();

            List<int> list = new List<int>();

            string[] lines = System.IO.File.ReadAllLines(path);

            for(int i = 1; i < lines.Length; i++)
                list.Add(Convert.ToInt32(lines[i]));

            return list;
        }
        public static string[] GenerisiRLT(List<int> listaRobaID)
        {
            string[] list = new string[listaRobaID.Count + 1];

            list[0] = "ID: 0";

            for (int i = 0; i < listaRobaID.Count; i++)
                list[i + 1] = listaRobaID[i].ToString();

            return list;
        }
    }
}
