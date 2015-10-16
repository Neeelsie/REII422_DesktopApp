using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace RealEstate.Classes
{
    class Validation
    {
        public bool IsTextNumeric(string text)
        {
            Regex reg = new Regex("[^0-9]");
            return !reg.IsMatch(text);
        }

        public bool IsNumberInRange(int min, int max, string text)
        {
            try
            {
                int temp_value = int.Parse(text);

                return ((temp_value >= min && temp_value <= max) ? true : false);
            }
            catch
            {
                return false;
            }
        }

        public bool TextHasNumber(string text)
        {
            return text.Where(c => Char.IsDigit(c)).Any();
        }


        public bool TextHasSpecialChars(string text)
        {
            return text.Any(c => !Char.IsLetterOrDigit(c));
        }

        public bool TextIsLongerThan(string text, int length)
        {
            return (text.Length > length ? true : false);
        }

        public bool TextContainsBlankSpaces(string text)
        {
            return text.Contains(' ');
        }
    }
}
