namespace Termodom.Data.Entities.TDOffice_v2
{
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string PW { get; set; }
        public int MagacinID { get; set; }
        public object TAG { get; set; }
        public int? KomercijalnoUserID { get; set; }
        public int Grad { get; set; }
        public int OpomeniZaNeizvrseniZadatak { get; set; }
        public int BonusZakljucavanjaCount { get; set; }
        public double BonusZakljucavanjaLimit { get; set; }
    }
}
