using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace TDBrain_v3
{
    public static class Settings
    {
        public class DTO
        {
            public class DBSettingsDTO
            {
                public class PutanjeDoBazaDTO
                {
                    public class Item
                    {
                        public int MagacinID { get; set; }
                        public int Godina { get; set; }
                        public string PutanjaDoBaze { get; set; } = "";
                    }
                    
                    public List<Item> Items { get; set; } = new List<Item>();
                    
                    public DB.ConnectionStringCollection ToConnectionStringCollection()
                    {
                        DB.ConnectionStringCollection col = new DB.ConnectionStringCollection();

                        foreach(Item it in Items)
                            col.AddOrUpdate(it.MagacinID, it.Godina, it.PutanjaDoBaze);

                        return col;
                    }
                }
                public string ServerName { get; set; } = "localhost";
                public string Password { get; set; } = "masterkey";
                public string User { get; set; } = "SYSDBA";
                public PutanjeDoBazaDTO PutanjeDoBaza = new PutanjeDoBazaDTO();
                public string? ConnectionStringKomercijalnoConfig { get; set; }
                public string? ConnectionStringTDOffice_v2 { get; set; }
            }
            public class FTPSettingsDTO
            {
                public string? Url { get; set; }
                public string? Username { get; set; }
                public string? Password { get; set; }
            }

            public DBSettingsDTO DBSettings { get; set; } = new DBSettingsDTO();
            public FTPSettingsDTO FTPSettings { get; set; } = new FTPSettingsDTO();
        }

        static Settings()
        {
            if (!File.Exists("serverSettings.td"))
                Save();
            else
                Reload();
        }
        private static string EncDecKey { get; set; } = "b14ca5898a4e4111bbce2ea2315a1916";

        public static void Reload()
        {
            string encString = File.ReadAllText("serverSettings.td");

            DTO? dto = JsonConvert.DeserializeObject<DTO>(DecryptString(EncDecKey, encString));

            if(dto != null)
            {
                DB.Settings.ConnectionStringTDOffice_v2.SetPath(dto.DBSettings.ConnectionStringTDOffice_v2);
                DB.Settings.ConnectionStringKomercijalno = dto.DBSettings.PutanjeDoBaza.ToConnectionStringCollection();
                DB.Settings.Password = dto.DBSettings.Password;
                DB.Settings.ServerName = dto.DBSettings.ServerName;
                DB.Settings.User = dto.DBSettings.User;
                DB.Settings.ConnectionStringKomercijalnoConfig.SetPath(dto.DBSettings.ConnectionStringKomercijalnoConfig);
                FTPSettings.Username = dto.FTPSettings.Username;
                FTPSettings.Url = dto.FTPSettings.Url;
                FTPSettings.Password = dto.FTPSettings.Password;
            }
            else
            {
                throw new Exception("Error loading serverSettings!");
            }
        }
        public static DTO ToDTO()
        {
            DTO dto = new DTO();
            dto.DBSettings.ServerName = DB.Settings.ServerName;
            dto.DBSettings.PutanjeDoBaza = DB.Settings.ConnectionStringKomercijalno.ToPutanjeDoBazaDTO();
            dto.DBSettings.Password = DB.Settings.Password;
            dto.DBSettings.User = DB.Settings.User;
            dto.DBSettings.ConnectionStringKomercijalnoConfig = DB.Settings.ConnectionStringKomercijalnoConfig.Path();
            dto.DBSettings.ConnectionStringTDOffice_v2 = DB.Settings.ConnectionStringTDOffice_v2.Path();
            dto.FTPSettings.Username = FTPSettings.Username;
            dto.FTPSettings.Url = FTPSettings.Url;
            dto.FTPSettings.Password = FTPSettings.Password;
            return dto;
        }
        public static void Save()
        {
            File.WriteAllText("serverSettings.td", EncryptString(EncDecKey, JsonConvert.SerializeObject(ToDTO())));
        }

        private static string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }
        private static string DecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
