using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public partial class Cenovnik
    {
        public class Artikal
        {
            public int ID { get; set; }
            public Cena Cena { get; set; }
        }
    }
}
