using System.ComponentModel.DataAnnotations.Schema;
using TD.Core.Contracts;

namespace TD.TDOffice.Contracts.Entities
{
    [Table("USERS")]
    public class User : IEntity
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("USERNAME")]
        public string Username { get; set; }
        [Column("PW")]
        public string Password { get; set; }
        [Column("MAGACINID")]
        public int MagacinId { get; set; }
        [Column("KOMERCIJALNO_USER_ID")]
        public int? KomercijalnoUserId { get; set; }
        [Column("GRAD")]
        public int GradId { get; set; }
        [Column("OPOMENI_ZA_NEIZVRSENI_ZADATAK")]
        public int OpomeniZaNeizvrseniZadatak { get; set; }
        [Column("BONUS_ZAKLJUCAVANJA_COUNT")]
        public int BonusZakljucavanjaCount { get; set; }
        [Column("BONUS_ZAKLJUCAVANJA_LIMIT")]
        public double BonusZakljucavanjaLimit { get; set; }

        [NotMapped]
        public bool IsActive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [NotMapped]
        public DateTime CreatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [NotMapped]
        public long? UpdatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [NotMapped]
        public DateTime? UpdatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
