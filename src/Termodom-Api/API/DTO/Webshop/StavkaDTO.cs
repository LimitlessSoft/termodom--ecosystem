using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTO.Webshop
{
    public class StavkaDTO
    {
        public int ItemID { get; set; }
        public int PorudzbinaID { get; set; }
        public int RobaID { get; set; }
        public double Kolicina { get; set; }
        public double VPCena { get; set; }
        public double Rabat { get; set; }
    }
}
