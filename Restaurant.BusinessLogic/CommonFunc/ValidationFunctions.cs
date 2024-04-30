using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace Restaurant.BusinessLogic.CommonFunc
{
    static public class ValidationFunctions
    {
        public static bool BeInFuture(DateTime time)
        {
            return time > DateTime.Now;
        }

        public static bool BeInPast(DateTime time)
        {
            return time < DateTime.Now;
        }

        public static bool CorrectFileExtension(IFormFile? picture)
        {
            if (picture == null)
                return true;
            if (picture.FileName.ToUpper().EndsWith(".JPEG")
                || picture.FileName.ToUpper().EndsWith(".PNG")
                || picture.FileName.ToUpper().EndsWith(".JPG")
                )
                return true;
            return false;
        }

        public static bool ContainSpecialCharacters(string password)
        {
            if (password != null)
            {
                string specialCharacters = "!@#$%^&*()_-+=<>?/";
                return password.Any(ch => specialCharacters.Contains(ch));
            }
            return true;
        }

        public static bool ContainUppercaseLetter(string password)
        {
            if (password != null)
            {
                return password.Any(ch => char.IsUpper(ch));
            }
            return true;
        }

        public static bool ContainNumber(string password)
        {
            if (password != null)
            {
                return password.Any(ch => char.IsDigit(ch));
            }
            return true;
        }

        public static bool ContainsOnlyNumbersWithOptionalPlus(string phone)
        {
            if (phone != null)
            {
                string pattern = @"^\+?\d+$";

                return Regex.IsMatch(phone, pattern);
            }
            return true;

        }

        public static bool ContainsOnlyLetters(string name)
        {
            if (name != null)
                return Regex.IsMatch(name, @"^[A-Za-z0-9 _]*[A-Za-z0-9][A-Za-z0-9 _]*$");
            return true;
        }

        internal static bool ContainsOnlyLettersAndNumbers(string name)
        {
            if (name != null)
                return Regex.IsMatch(name, @"^[A-Za-z0-9 _]*[A-Za-z0-9][A-Za-z0-9 _]*$");
            return true;
        }
    }
}
