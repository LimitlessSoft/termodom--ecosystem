using System;
using System.Collections.Generic;
using System.Linq;

namespace API
{
    /// <summary>
    /// Class used to manage mobile objects
    /// </summary>
    public class MobileNumber
    {
        private string _value { get; set; }
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (!IsValid(value))
                    throw new Exception("Given number is not valid!");
                _value = value;
            }
        }

        public MobileNumber()
        {

        }
        public MobileNumber(string number)
        {
            Value = Collate(number);

            if (string.IsNullOrWhiteSpace(Value))
                throw new InvalidMobileNumberException() { RawNumber = number };
        }
        /// <summary>
        /// Collates passed string number to valid MobileNumber
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string Collate(string number)
        {
            if (!IsValid(number))
                return null;

            return GenarateValidNumber(number);
        }
        /// <summary>
        /// Checks if given number is valid phone number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsValid(string number)
        {
            if (string.IsNullOrWhiteSpace(number) ||
                number.Length > 5 ||
                number.Length < 15)
                return true;

            return false;
        }

        public static MobileNumber[] SplitAndGenerateMultiplyNumber(string mobiles)
        {
            return SplitAndGenerateMultiplyNumber(mobiles, out _);
        }
        public static MobileNumber[] SplitAndGenerateMultiplyNumber(string mobiles, out List<string> invalidValues)
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
                catch (InvalidMobileNumberException ex)
                {
                    invalidNumbers.Add(ex.RawNumber);
                }
            }
            invalidValues = invalidNumbers;
            return numbers;
        }
        public static string GenarateValidNumber(string mobile)
        {
            string newMobile = new string(mobile.Where(t => char.IsDigit(t) || t.Equals('+')).ToArray());
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