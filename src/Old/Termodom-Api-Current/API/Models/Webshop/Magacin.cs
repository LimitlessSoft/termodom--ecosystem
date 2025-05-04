using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.Webshop
{
	/// <summary>
	/// Model predstavlja magacine
	/// </summary>
	public class Magacin
	{
		public int ID { get; set; }
		public string Naziv { get; set; }
		public string Adresa { get; set; }
		public string Grad { get; set; }
		public string Email { get; set; }
		public string Koordinate { get; set; }
		public string Telefon { get; set; }
	}
}
