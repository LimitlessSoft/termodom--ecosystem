namespace Termodom
{
	public static class Random
	{
		/// <summary>
		/// Fixed random number that is assigned once application starts
		/// </summary>
		public static int Fixed { get; private set; }

		private static System.Random _rnd = new System.Random();

		static Random()
		{
			Fixed = Next(0, 1000);
		}

		public static int Next(int maxValue)
		{
			return _rnd.Next(0, maxValue);
		}

		public static int Next(int minValue, int maxValue)
		{
			return _rnd.Next(minValue, maxValue);
		}
	}
}
