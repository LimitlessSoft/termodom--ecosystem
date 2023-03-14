namespace TDBrain_v3.DTO.Komercijalno
{
    /// <summary>
    /// Klasa koja sadrzi DTO klase vezane za /komercijalno/stavka endpointe
    /// </summary>
    public static class Stavka
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public class KopirajIzvornaBazaDTO
        {
            public int godina { get; set; }
            public int magacinID { get; set; }
        }
        public class KopirajDestinacionaBazaDTO
        {
            public int godina { get; set; }
            public int magacinID { get; set; }
        }


        public class KopirajIzvorniDokumentDTO
        {
            public int VrDok { get; set; }
            public int BrDok { get; set; }
        }
        public class KopirajDestinacioniDokumentDTO
        {
            public int VrDok { get; set; }
            public int BrDok { get; set; }
        }
        public class KopirajDestinacioniDokumentMoraBitiPrazanOptionsDTO
        {
            public KopirajDestinacioniDokumentMoraBitiPrazanOptionsOnDuplikatStavke OnDuplikatStavke { get; set; }
        }
        public enum KopirajDestinacioniDokumentMoraBitiPrazanOptionsOnDuplikatStavke
        {
            DuplirajStavku = 0,
            ZaobidjiStavku = 1
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
