using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.DTO
{
	public class KomecijalnoPodesiVrednostiParametaraPoSablonuDTO
	{
		public string NazivParametra { get; set; }
		public string VrednostParametra { get; set; }

		public KomecijalnoPodesiVrednostiParametaraPoSablonuDTO() { }

		public KomecijalnoPodesiVrednostiParametaraPoSablonuDTO(string naziv, string vrednost)
		{
			this.NazivParametra = naziv;
			this.VrednostParametra = vrednost;
		}
	}
}
