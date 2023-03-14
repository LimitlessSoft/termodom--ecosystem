using DBMigration.TDOffice;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using Termodom.Data.Managers;

namespace DBMigration
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
#if DEBUG
            Termodom.Data.Managers.TDBrainManager.API_BASE_URL = "https://localhost:7207";
#else
            Termodom.Data.Managers.TDBrainManager.API_BASE_URL = "https://4monitor:7207";
#endif
            Console.WriteLine("Press enter to start migration process");
            Console.ReadLine();
            Console.WriteLine("Migration process started");;

            // _ = ImportBeleskiAsync();

            // var userToKorisnikMigrationManager = new UsersToKorisnikMigrationManager();
            // _ = userToKorisnikMigrationManager.MigrateAsync();

            Console.ReadLine();
        }

        private static async Task ImportBeleskiAsync()
        {
            try
            {
                Console.WriteLine("Zapocinjem import beleski!");
                OpenFileDialog dialog = new OpenFileDialog();
                if (DialogResult.OK == dialog.ShowDialog())
                {
                    string path = dialog.FileName;
                    List<JObject> users = JsonConvert.DeserializeObject<List<JObject>>(System.IO.File.ReadAllText(path));

                    foreach (JObject j in users)
                    {
                        JToken a = j["Tag"]["Beleske"];

                        foreach (var b in a)
                        {
                            int id = Convert.ToInt32(b["ID"]);
                            string korisnikId = j["ID"].ToString();
                            string body = b["Body"].ToString();
                            string naziv = b["Naziv"].ToString();

                            var response = await TDBrainManager.PutAsync("/TDOffice/Korisnik/Beleska/Save", new Dictionary<string, string>()
                            {
                                { "KorisnikId", korisnikId },
                                { "Naslov", naziv },
                                { "Body", body }
                            });
                        }
                    }
                    Console.WriteLine("Gotov import beleski!");
                }
                else
                {
                    Console.WriteLine("Obustavljam import beelski!");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}