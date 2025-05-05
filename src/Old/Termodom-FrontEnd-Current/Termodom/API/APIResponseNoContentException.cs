using System;

namespace Termodom.API
{
	/// <summary>
	/// Response je uspesno vracen ali kao 204 (nema content-a)
	/// </summary>
	public class APIResponseNoContentException : Exception { }
}
