using System;
using Termodom.Models;

namespace Termodom.DTO.Webshop
{
    public class StavkaDTO
    {
        public int ID { get; set; }
        public Int16 PorudzbinaID { get; set; }
        public Int16 RobaID { get; set; }
        public double Kolicina { get; set; }
        public double VPCena { get; set; }
        public double Rabat { get; set; }
    }
}
