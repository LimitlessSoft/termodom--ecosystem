using Newtonsoft.Json;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpCompress.Archives;
using SharpCompress.Archives.Rar;

namespace TDOffice_v2
{
    static class Program
    {
        #region StaticFlags
        public static bool PokrenutIzvestajNelogicnihMarzi { get; set; } = false;
        public static int IzvestajNelogicnihMarziTrenutniStage { get; set; } = 0;
        public static int IzvestajNelogicnihMarziMaxStage { get; set; } = 0;
        #endregion

        public static int ForbiddenRequestMaxTries { get; set; } = 10;
        public static TDOffice.User TrenutniKorisnik { get; set; }
        public static bool IsRunning { get; set; } = true;

        public static string BaseAPIUrl { get; set; } = "https://api.termodom.rs";
        //public static string BaseAPIUrl { get; set; } = "https://localhost:44311";

        private static string _APIToken { get; set; } = null;
        private static HttpClient _client = new HttpClient();
        public static string APIToken
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_APIToken))
                {
                    string username = null, pw = null;

                    using (InputBox ib = new InputBox("Web Login", "WEB Username"))
                    {
                        ib.ShowDialog();
                        if (string.IsNullOrWhiteSpace(ib.returnData))
                            return null;
                        username = ib.returnData.ToString();
                    }

                    using (InputBox ib = new InputBox("Web Login", "WEB Password"))
                    {
                        ib.ShowDialog();
                        if (string.IsNullOrWhiteSpace(ib.returnData))
                            return null;
                        pw = ib.returnData.ToString();
                    }

                    if (!TDWeb.TDWeb.SetApiKey(username, pw))
                        throw new Exception("Pogresan username ili password!");
                }
                return _APIToken;
            }
            set
            {
                _APIToken = value;
            }
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            TDOffice.TDOffice.connectionString = LocalSettings.Settings.tdofficeConnectionString;

            // Proveravam da li je glavi connection string dobar. Ako nije javljam gresku
            try
            {
                TDOffice.Config<string>.Get(-1);
            }
            catch
            {
                MessageBox.Show("Putanja do TDOffice_v2 baze nije ispravna! Ispravite je unutar %appdata%/TDoffice_v2/Settings.td fajla");
                return;
            }

            //TDOffice.Config<string> organizatorConnectionStringConfig = TDOffice.Config<string>.Get(CONFIG_ID_CONNECTION_STRING_ORGANIZATOR);
            TDOffice.Config<string> configConnectionStringConfig = TDOffice.Config<string>.Get(TDOffice.ConfigParameter.ConnectionStringConfig);
            TDOffice.Config<Dictionary<int, string>> komercijalnoConnectionStringsConfig = TDOffice.Config<Dictionary<int, string>>.Get(TDOffice.ConfigParameter.ConnectionStringsKomercijalno);
            //if (string.IsNullOrWhiteSpace(organizatorConnectionStringConfig.Tag))
            //{
            //    organizatorConnectionStringConfig.Tag = "data source=4monitor; initial catalog = C:\\ORGTASK\\Programi\\Proces5\\ORG\\ORGANIZATOR.FDB; user=SYSDBA; password=m";
            //    organizatorConnectionStringConfig.UpdateOrInsert();
            //}
            if (string.IsNullOrWhiteSpace(configConnectionStringConfig.Tag))
            {
                configConnectionStringConfig.Tag = "data source=localhost; initial catalog = C:\\Poslovanje\\Baze\\ConfigPDV\\CONFIG.FDB; user=SYSDBA; password=masterkey";
                configConnectionStringConfig.UpdateOrInsert();
            }
            if (komercijalnoConnectionStringsConfig.Tag == null)
            {
                komercijalnoConnectionStringsConfig.Tag = new Dictionary<int, string>()
                {
                    { 2022, "data source=localhost; initial catalog = C:\\Poslovanje\\Baze\\2022\\FIRMA2022.FDB; user=SYSDBA; password=masterkey" },
                    { 2021, "data source=localhost; initial catalog = C:\\Poslovanje\\Baze\\2021\\FIRMA2021.FDB; user=SYSDBA; password=masterkey" },
                    { 2020, "data source=localhost; initial catalog = C:\\Poslovanje\\Baze\\2020\\FIRMA2020.FDB; user=SYSDBA; password=masterkey" },
                    { 2019, "data source=localhost; initial catalog = C:\\Poslovanje\\Baze\\2019\\FIRMA2019.FDB; user=SYSDBA; password=masterkey" },
                    { 2018, "data source=localhost; initial catalog = C:\\Poslovanje\\Baze\\2018\\FIRMA2018.FDB; user=SYSDBA; password=masterkey" },
                    { 2017, "data source=localhost; initial catalog = C:\\Poslovanje\\Baze\\2017\\FIRMA2017.FDB; user=SYSDBA; password=masterkey" }
                };

                komercijalnoConnectionStringsConfig.UpdateOrInsert();
            }
            //orgnizatorConnectionString = organizatorConnectionStringConfig.Tag;
            Config.Config.connectionString = configConnectionStringConfig.Tag;
            Komercijalno.Komercijalno.CONNECTION_STRING = komercijalnoConnectionStringsConfig.Tag;

            if (IsProgramOldVersionedAsync().Result)
            {
                DownloadAndInstallNewVersionAsync().Wait();
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Inicijalizacija programa
            TDOffice.Config<List<int>> c = TDOffice.Config<List<int>>.Get(TDOffice.ConfigParameter.OsvezavanjePrava);
            if (c.Tag == null)
            {
                c.Tag = new List<int>();
                c.UpdateOrInsert();
            }
            TDOffice.Config<List<int>> cmp = TDOffice.Config<List<int>>.Get(TDOffice.ConfigParameter.MagacinUPopisu);
            if (cmp.Tag == null)
            {
                cmp.Tag = new List<int>();
                cmp.UpdateOrInsert();
            }
            // =======================

        #if DEBUG
            Termodom.Data.Managers.TDBrainManager.API_BASE_URL = "https://localhost:7207";
        #else
            Termodom.Data.Managers.TDBrainManager.API_BASE_URL = "https://4monitor:7207";
        #endif

            Application.Run(new Login());
            Program.IsRunning = false;
        }
        public static async Task<Version> GetMinVerzijaAsync()
        {
            var response = await _client.GetAsync("http://dev.api.termodom.rs/software/info?id=TDOffice_v2");
            DTO_FV dto = JsonConvert.DeserializeObject<DTO_FV>(await response.Content.ReadAsStringAsync());
            return new Version(dto.minimal_version);
        }
        /// <summary>
        /// Proverava da li je verzija starija od minimalne
        /// </summary>
        /// <returns></returns>

        public static async Task<Version> GetMinVerzijaInstallerAsync()
        {
            var response = await _client.GetAsync("http://dev.api.termodom.rs/software/info?id=TDOffice_Installer");
            DTO_IV dto = JsonConvert.DeserializeObject<DTO_IV>(await response.Content.ReadAsStringAsync());
            return new Version(dto.minimal_version);
        }
        public static async Task<bool> IsProgramOldVersionedAsync()
        {
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var productVersion = FileVersionInfo.GetVersionInfo(assemblyLocation).ProductVersion;
            Version v1 = new Version(productVersion);
            Version v2 = await GetMinVerzijaAsync();

            return v1 < v2;
        }
        public static async Task<bool> IsInstallerOldVersionedAsync()
        {
            string tempPath = Path.GetTempPath() + @"\TDOffInstaler\TDOffice_Installer.exe";
            var fileVersion = FileVersionInfo.GetVersionInfo(tempPath).FileVersion;
            Version v1 = new Version(fileVersion);
            Version v2 = await GetMinVerzijaInstallerAsync();

            return v1 < v2;
        }
        public class DTO_FV
        {
            public string title { get; set; }
            public string last_version { get; set; }
            public string minimal_version { get; set; }

        }
        public class DTO_IV
        {
            public string title { get; set; }
            public string last_version { get; set; }
            public string minimal_version { get; set; }

        }
        public static async Task DownloadAndInstallNewVersionAsync()
        {
            _ = Task.Run(() =>
            {
                MessageBox.Show("Verzija programa koju koristite je zastarela!\nPokrenuto je skidanje nove verzije!\nSacekajte sledeci prozor!");
            });

            string tempPath = Path.GetTempPath() + @"\TDOffInstaler";
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }
            if(!File.Exists(Path.Combine(tempPath, "TDOffice_Installer.exe")))
            {
                var response1 = await _client.GetAsync("http://dev.api.termodom.rs/software/download?id=TDOffice_Installer");
                if ((int)response1.StatusCode == 200)
                {
                    Stream str = await response1.Content.ReadAsStreamAsync();
                    FileStream fs = new FileStream(Path.Combine(tempPath, "TDOffice_Installer.exe"), FileMode.OpenOrCreate);
                    str.CopyTo(fs);
                    fs.Close();
                    str.Close();
                }
                else
                {
                    MessageBox.Show("Greska prilikom skidanja nove verzije!");
                    return;
                }
            }
            else
            {
                //Provera verzije Installera
                if (IsInstallerOldVersionedAsync().Result)
                {
                    var response1 = await _client.GetAsync("http://dev.api.termodom.rs/software/download?id=TDOffice_Installer");
                    if ((int)response1.StatusCode == 200)
                    {
                        Stream str = await response1.Content.ReadAsStreamAsync();
                        FileStream fs = new FileStream(Path.Combine(tempPath, "TDOffice_Installer.exe"), FileMode.OpenOrCreate);
                        str.CopyTo(fs);
                        fs.Close();
                        str.Close();
                    }
                    else
                    {
                        MessageBox.Show("Greska prilikom skidanja nove verzije!");
                        return;
                    }
                }
            }
            
            Process p = Process.Start(Path.Combine(tempPath, "TDOffice_Installer.exe"));
            Application.Exit();
        }
        public static void GCCollectWithDelayAsync()
        {
            Thread t1 = new Thread(() =>
            {
                Thread.Sleep(3000);
                GC.Collect();
            });
            t1.IsBackground = true;
            t1.Start();
        }
    }
}