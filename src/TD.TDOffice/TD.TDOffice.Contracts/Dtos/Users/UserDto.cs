using System.ComponentModel.DataAnnotations.Schema;

namespace TD.TDOffice.Contracts.Dtos.Users
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int MagacinId { get; set; }
        public int? KomercijalnoUserId { get; set; }
        public int GradId { get; set; }
        public int OpomeniZaNeizvrseniZadatak { get; set; }
        public int BonusZakljucavanjaCount { get; set; }
        public double BonusZakljucavanjaLimit { get; set; }
    }
}
