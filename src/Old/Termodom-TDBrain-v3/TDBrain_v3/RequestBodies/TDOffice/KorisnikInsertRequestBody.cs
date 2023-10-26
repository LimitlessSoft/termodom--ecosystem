namespace TDBrain_v3.RequestBodies.TDOffice
{
    /// <summary>
    /// 
    /// </summary>
    public class KorisnikInsertRequestBody
    {
        public string KorisnickoIme { get; set; }
        public string Sifra { get; set; }
        public int MagacinId { get; set; }
        public int? KomercijalnoUserId { get; set; }
        public int Grad { get; set; }
    }
}
