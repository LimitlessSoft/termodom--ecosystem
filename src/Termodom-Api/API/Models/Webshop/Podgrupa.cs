using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.Webshop
{
    public class Podgrupa
    {
        public int ID { get; set; }
        public int GrupaID { get; set; }
        public string Naziv { get; set; }
        public string Slika { get; set; }
       
    }
}
