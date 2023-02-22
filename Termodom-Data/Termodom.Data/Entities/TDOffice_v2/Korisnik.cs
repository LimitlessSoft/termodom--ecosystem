namespace Termodom.Data.Entities.TDOffice_v2
{
    public class Korisnik
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int? MagacinId { get; set; }
        public int? KomecijalnoUserId { get; set; }
        public int Grad { get; set; }
        public int OpomeniZaNeizvrseniZadatak { get; set; }
        public int BonusZakljucavanjaCount { get; set; }
        public double BonusZakljucavanjaLimit { get; set; }
    }
}
