using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Models.Webshop
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
	public class Grupa
	{
		public int ID { get; set; }
		public string Naziv { get; set; }
		public int DisplayIndex { get; set; }
	}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
