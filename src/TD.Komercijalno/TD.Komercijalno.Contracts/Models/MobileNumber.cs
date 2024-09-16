using LSCore.Contracts.Exceptions;

namespace TD.Komercijalno.Contracts.Models
{
    public class MobileNumber
    {
        private string? _value { get; init; }

        private string? Value
        {
            get { return _value; }
            init
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
                throw new LSCoreBadRequestException($"Mobilni nije ispravan: {number}");
        }

        /// <summary>
        /// Collates passed string number to valid MobileNumber
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private static string? Collate(string number) =>
            !IsValid(number) ? null : GenerateValidNumber(number);

        /// <summary>
        /// Checks if given number is valid phone number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool IsValid(string? number) =>
            string.IsNullOrWhiteSpace(number) || number.Length > 5 || number.Length < 15;

        private static MobileNumber[] SplitAndGenerateMultiplyNumber(string mobiles)
        {
            var mobilesArray = mobiles.Split(',');
            var numbers = new MobileNumber[mobilesArray.Length];

            for (var i = 0; i < mobilesArray.Length; i++)
                numbers[i] = new MobileNumber(GenerateValidNumber(mobilesArray[i]));

            return numbers;
        }

        private static string GenerateValidNumber(string mobile)
        {
            var newMobile = new string(
                mobile.Where(t => char.IsDigit(t) || t.Equals('+')).ToArray()
            );
            if (newMobile.StartsWith('0'))
            {
                newMobile = string.Concat("+381", newMobile.AsSpan(1));
            }
            if (newMobile.StartsWith('+') && newMobile[4] == '0')
                newMobile = newMobile.Remove(4, 1);
            return newMobile;
        }
    }
}
