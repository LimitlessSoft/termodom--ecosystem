using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2
{
	/// <summary>
	/// Class used to manage mobile objects
	/// </summary>
	public class MobileNumber
	{
		private static char[] _allowedChars = new char[11]
		{
			'0',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9',
			'+'
		};
		private string _value { get; set; }
		public string Value
		{
			get { return _value; }
			set
			{
				if (!IsValid(value))
					throw new Exception("Given number is not valid!");
				_value = value;
			}
		}

		public MobileNumber() { }

		public MobileNumber(string number)
		{
			Value = Collate(number);

			if (string.IsNullOrWhiteSpace(Value))
				throw new Exceptions.InvalidMobileNumberException() { RawNumber = number };
		}

		/// <summary>
		/// Collates passed string number to valid MobileNumber
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public static string Collate(string number)
		{
			string newNumber = GenarateValidNumber(number);

			if (!IsValid(newNumber))
				return null;

			return newNumber;
		}

		/// <summary>
		/// Checks if given number is valid phone number
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public static bool IsValid(string number)
		{
			if (string.IsNullOrWhiteSpace(number) || number.Length <= 5 || number.Length > 15)
				return false;

			foreach (char c in number)
				if (!_allowedChars.Contains(c))
					return false;

			return true;
		}

		public static MobileNumber[] SplitAndGenerateMultiplyNumber(string mobiles)
		{
			return SplitAndGenerateMultiplyNumber(mobiles, out _);
		}

		public static MobileNumber[] SplitAndGenerateMultiplyNumber(
			string mobiles,
			out List<string> invalidValues
		)
		{
			List<string> invalidNumbers = new List<string>();
			string[] mobilesArray = mobiles.Split(',');
			MobileNumber[] numbers = new MobileNumber[mobilesArray.Length];

			for (int i = 0; i < mobilesArray.Length; i++)
			{
				try
				{
					numbers[i] = new MobileNumber(GenarateValidNumber(mobilesArray[i]));
				}
				catch (Exceptions.InvalidMobileNumberException ex)
				{
					invalidNumbers.Add(ex.RawNumber);
				}
			}
			invalidValues = invalidNumbers;
			return numbers;
		}

		public static string GenarateValidNumber(string mobile)
		{
			if (string.IsNullOrWhiteSpace(mobile))
				return null;

			string newMobile = new string(
				mobile.Where(t => char.IsDigit(t) || t.Equals('+')).ToArray()
			);
			if (newMobile.IndexOf('0') == 0)
			{
				newMobile = "+381" + newMobile.Substring(1);
			}
			if (newMobile.IndexOf('+') == 0 && newMobile[4] == '0')
				newMobile = newMobile.Remove(4, 1);
			return newMobile;
		}
	}
}
