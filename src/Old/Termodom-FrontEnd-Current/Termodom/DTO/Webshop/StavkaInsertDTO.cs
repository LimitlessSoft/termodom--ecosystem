using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Termodom.DTO.Webshop
{
    /// <summary>
    /// /Webshop/Stavka/Insert
    /// </summary>
    public class StavkaInsertDTO
    {
        public int PorudzbinaID { get; set; }
        public int RobaID { get; set; }
        public double Kolicina { get; set; }
        public double VPCena { get; set; }
        public double Rabat { get; set; }

        public StavkaInsertDTO()
        {

        }
        public StavkaInsertDTO(int porudzbinaID, int robaID, double kolicina, double vpCena, double rabat)
        {
            this.PorudzbinaID = porudzbinaID;
            this.RobaID = robaID;
            this.Kolicina = kolicina;
            this.VPCena = vpCena;
            this.Rabat = rabat;
        }
    }
}
