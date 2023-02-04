using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.DTO.TDBrain_v3.Komercijalno
{
    public class RobaDTO
    {
        public int ID { get; set; }
        public string KatBr { get; set; }
        public string KatBrPro { get; set; }
        public string Naziv { get; set; }
        public string JM { get; set; }
        public string TarifaID { get; set; }
        public int Vrsta { get; set; }
        public string NazivZaStampu { get; set; }
        public string GrupaID { get; set; }
        public int Podgrupa { get; set; }
        public string proID { get; set; }
        public int? DOB_PPID { get; set; }
        public double? TrKol { get; set; }
        public string TrPak { get; set; }
    }
}
