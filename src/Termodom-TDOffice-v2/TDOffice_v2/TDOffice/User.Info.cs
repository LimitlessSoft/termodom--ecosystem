using System.Collections.Generic;

namespace TDOffice_v2.TDOffice
{
    public partial class User
    {
        public class Info
        {
            public int narudbenicaPPID { get; set; } = 4698;
            public string Beleska { get; set; }
            public List<User.Beleska> Beleske { get; set; } = new List<User.Beleska>();
            public Dictionary<TipAutomatskogObavestenja, bool> PrimaObavestenja = new Dictionary<TipAutomatskogObavestenja, bool>()
            {
                { TipAutomatskogObavestenja.NakonZameneRobe, false },
                { TipAutomatskogObavestenja.NakonRazduzenjaRobe, false },
                { TipAutomatskogObavestenja.PravoPristupaModulu, false }
            };
        }

        void asd()
        {

            // Key je unikatan
            // Value nije unikatan
            Dictionary<string, int> dnevnik = new Dictionary<string, int>();
            dnevnik["asd"] = 1; // Postavlja 1 kao VALUE unutar dnevnika na poziciji gde je KEY "asd"
            int a = dnevnik["asd"]; // Hvata vrednost VALUE iz dnevnika na poziciji gde je KEY "asd"

            Dictionary<TipAutomatskogObavestenja, bool> objasnjenjePrimaObavestenja = new Dictionary<TipAutomatskogObavestenja, bool>();
            objasnjenjePrimaObavestenja[TipAutomatskogObavestenja.NakonRazduzenjaRobe] = true;
            bool daLiPrimaSledeceObavestenje = objasnjenjePrimaObavestenja[TipAutomatskogObavestenja.NakonRazduzenjaRobe];

            List<User> users = User.List();

            foreach (User u in users)
                if (u.Tag.PrimaObavestenja[TipAutomatskogObavestenja.NakonRazduzenjaRobe]) // Proverava da li korisnik prima ovo obavestenje
                    return;


            User userIndex = new User();
            userIndex.Tag.PrimaObavestenja[TipAutomatskogObavestenja.NakonRazduzenjaRobe] = false;
            userIndex.Update();

            

        }
    }
}
