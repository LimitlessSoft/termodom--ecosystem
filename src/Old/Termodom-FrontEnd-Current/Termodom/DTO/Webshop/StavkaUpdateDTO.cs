using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Termodom.DTO.Webshop
{
	/// <summary>
	/// /Webshop/Stavka/Update
	/// </summary>
	public class StavkaUpdateDTO
	{
		public int ID { get; set; }
		public double Kolicina { get; set; }
		public double VPCena { get; set; }
		public double Rabat { get; set; }

		public StavkaUpdateDTO() { }

		public StavkaUpdateDTO(int id, double kolicina, double vpCena, double rabat)
		{
			this.ID = id;
			this.Kolicina = kolicina;
			this.VPCena = vpCena;
			this.Rabat = rabat;
		}
	}
}
