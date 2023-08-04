using System.ComponentModel.DataAnnotations.Schema;
using TD.Core.Contracts;

namespace TD.Komercijalno.Contracts.Entities
{
    [Table("NACIN_PLACANJA")]
    public class NacinPlacanja : IEntity
    {
        [Column("NPID")]
        public int Id { get; set; }
        [Column("NAZIV")]
        public string Naziv { get; set; }
    }
}
