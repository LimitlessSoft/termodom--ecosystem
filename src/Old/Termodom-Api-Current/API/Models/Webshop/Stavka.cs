using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Models.Webshop
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
	public class Stavka
	{
		public int ID { get; set; }
		public Int16 PorudzbinaID { get; set; }
		public Int16 RobaID { get; set; }
		public double Kolicina { get; set; }
		public double VPCena { get; set; }
		public double Rabat { get; set; }
	}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
