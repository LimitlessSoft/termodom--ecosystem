using LSCore.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Common.Contracts.Entities
{
    public class KorisnikEntity : LSCoreEntity
    {
        public string Ime { get; set; }
        public string Nadimak { get; set; }
        public string Sifra { get; set; }
        public KorisnikTip Tip { get; set; }
    }
}
