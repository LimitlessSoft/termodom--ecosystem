using System.Collections.Generic;
using Termodom.Models;

namespace Termodom.DTO.Webshop
{
    public class GetProizvodiDTO
    {
        public int Stranica { get; set; }
        public List<Proizvod> Proizvodi { get; set; }
    }
}
