using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API
{
    public static class Extensions
    {
        public static string ToStringOrDefault(this object sender)
        {
            if (sender == null)
                return "";

            return sender.ToString();
        }

        public static string Hash(string value)
        {
            HashAlgorithm algorithm = SHA256.Create();
            byte[] res = algorithm.ComputeHash(Encoding.UTF8.GetBytes(value));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in res)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        /// <summary>
        /// Proverava da li je mejl adresa validan
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsMailValid(this string value)
        {
            if (string.IsNullOrWhiteSpace(value) ||
                value.Count(x => x.Equals('@')) != 1 ||
                string.IsNullOrWhiteSpace(value.Split('@')[0]) ||
                string.IsNullOrWhiteSpace(value.Split('@')[1]) || 
                value.Split('@')[1].Count(x => x.Equals('.')) == 0)
                return false;

            return true;
        }
    }
}
