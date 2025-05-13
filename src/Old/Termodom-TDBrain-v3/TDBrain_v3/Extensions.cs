namespace TDBrain_v3
{
	/// <summary>
	/// Static class containing extensions
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Konvertuje objekat u string ili null
		/// </summary>
		/// <param name="sender"></param>
		/// <returns></returns>
		public static string? ToStringOrDefault(this object sender)
		{
			if (sender == null)
				return null;

			return sender.ToString();
		}

		/// <summary>
		/// Loguje exception u debug-u
		/// </summary>
		/// <param name="sender"></param>
		public static void Log(this Exception sender)
		{
			Debug.Log(sender.ToString());
		}
	}
}
