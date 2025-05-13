using Microsoft.AspNetCore.Mvc;

namespace Termodom.Models
{
	/// <summary>
	/// Sadrzi metode koje vracaju ObjectResult sa predefinisanim status kodom i porukom
	/// </summary>
	public static class AspNetCoreMVCResponseExtensions
	{
		/// <summary>
		/// Vraca ObjectResult sa status kodom 500 i porukom "Porudzbina nije pronadjena"
		/// </summary>
		/// <param name="sender"></param>
		/// <returns></returns>
		public static ObjectResult NotFound(this Porudzbina sender)
		{
			ObjectResult res = new ObjectResult("Porudzbina nije pronadjena");
			res.StatusCode = 500;
			return res;
		}

		/// <summary>
		/// Vraca ObjectResult sa satus kodom 500 i porukom "Stavka nije pronadjena"
		/// </summary>
		/// <param name="sender"></param>
		/// <returns></returns>
		public static ObjectResult NotFound(this Porudzbina.Stavka sender)
		{
			ObjectResult res = new ObjectResult("Stavka nije pronadjena");
			res.StatusCode = 500;
			return res;
		}
	}
}
