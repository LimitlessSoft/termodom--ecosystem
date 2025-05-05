using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2
{
	public static class Random
	{
		private static System.Random _rnd = new System.Random();

		public static int Next()
		{
			return _rnd.Next();
		}

		public static int Next(int maxValue)
		{
			return _rnd.Next(maxValue);
		}

		public static int Next(int minValue, int maxValue)
		{
			return _rnd.Next(minValue, maxValue);
		}

		public static double NextDouble()
		{
			return _rnd.NextDouble();
		}

		public static double NextDouble(double minValue, double maxValue)
		{
			return _rnd.NextDouble() * (maxValue - minValue) + minValue;
		}

		public static Color LightColor()
		{
			return Color.FromArgb(Next(125, 255), Next(125, 255), Next(125, 255));
		}
	}
}
