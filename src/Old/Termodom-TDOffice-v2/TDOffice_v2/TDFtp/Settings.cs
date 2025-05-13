using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.TDFtp
{
	public static class Settings
	{
		public static string GetUsername()
		{
			var response = TDBrain_v3.GetAsync("/ftpsettings/username/get");
			if ((int)response.Result.StatusCode == 200)
				return response.Result.Content.ReadAsStringAsync().Result;

			throw new Exception("Greska prilikom komunikacije sa APIjem");
		}

		public static string GetPassword()
		{
			var response = TDBrain_v3.GetAsync("/ftpsettings/password/get");
			if ((int)response.Result.StatusCode == 200)
				return response.Result.Content.ReadAsStringAsync().Result;

			throw new Exception("Greska prilikom komunikacije sa APIjem");
		}

		public static string GetUrl()
		{
			var response = TDBrain_v3.GetAsync("/ftpsettings/url/get");
			if ((int)response.Result.StatusCode == 200)
				return response.Result.Content.ReadAsStringAsync().Result;

			throw new Exception("Greska prilikom komunikacije sa APIjem");
		}
	}
}
