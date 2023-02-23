using Newtonsoft.Json;
using Termodom.Data.Entities.TDOffice_v2;
using Termodom.Data.Managers;

namespace DBMigration.TDOffice
{
    public class UsersToKorisnikMigrationManager : IMigrationManager
    {
        public async Task MigrateAsync()
        {
            Console.WriteLine("[Migratin UsersToKorisnik] > Start");
            var response = await TDBrainManager.GetAsync("/TDOffice_v2/Users/Dictionary");

            UserDictionary users;
            switch((int)response.StatusCode)
            {
                case 200:
                    users = JsonConvert.DeserializeObject<UserDictionary>(await response.Content.ReadAsStringAsync());
                    break;
                default:
                    Console.WriteLine("Error fetching from /TDOffice_v2/Users/Dictionary. Aborting.");
                    return;
            }

            foreach (User u in users.Values)
            {
                _ = await TDBrainManager.PutAsync("/TDOffice/Korisnik/Insert", new Dictionary<string, string>()
                {
                    { "KorisnickoIme", u.Username },
                    { "Sifra", u.PW },
                    { "MagacinId", u.MagacinID.ToString() },
                    { "Grad", u.Grad.ToString() },
                    { "KomercijalnoUserID", u.KomercijalnoUserID == null ? "" : u.KomercijalnoUserID.ToString() }
                });
            }

            Console.WriteLine("[Migratin UsersToKorisnik] > Finish");
        }
    }
}
