using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Models.Magacin
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
	public class Stavka
	{
		public Int16 ID { get; set; }
		public Int16 RobaID { get; set; }
		public Int16 VrDok { get; set; }
		public Int16 BrDok { get; set; }
		public double Kolicina { get; set; }
	}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
