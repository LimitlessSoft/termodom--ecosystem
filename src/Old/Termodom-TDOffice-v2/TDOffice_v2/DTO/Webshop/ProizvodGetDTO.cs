using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TDOffice_v2.DTO.Webshop
{
	public class ProizvodGetDTO
	{
		public int RobaID { get; set; }
		public string Slika { get; set; }
		public int PodgrupaID { get; set; }
		public Int16 Aktivan { get; set; }
		public double PDV { get; set; }
		public int DisplayIndex { get; set; }
		public string KratakOpis { get; set; }
		public int Poseta { get; set; }
		public Int16 Rasprodaja { get; set; }
		public string Keywords { get; set; }
		public double NabavnaCena { get; set; }
		public double ProdajnaCena { get; set; }
		public string Rel { get; set; }
		public string DetaljanOpis { get; set; }
		public Int16 Klasifikacija { get; set; }
		public double TransportnoPakovanje { get; set; }
		public string TransportnoPakovanjeJM { get; set; }
		public Int16 KupovinaSamoUTransportnomPakovanju { get; set; }
		public string IstaknutiProizvodi { get; set; }
		public Int16 UpozorenjeZalihaMalihStovarista { get; set; }
		public Int16 CenovnaGrupaID { get; set; }
		public string PovezaniProizvodi { get; set; }
		public double AktuelniRabat { get; set; }
		public List<int> Podgrupe { get; set; }
		public int Parent { get; set; }
		public int BrendID { get; set; }
	}
}
