using System.Text.RegularExpressions;

namespace MathemWebApi
{
    public static class CommonUtils
    {
        public static bool isInvalidSwedishPostalCode(this string postalCode)
        {
            string pattern = @"^(s-|S-){0,1}[0-9]{3}\s?[0-9]{2}$";
            // Create a Regex  
            Regex rg = new Regex(pattern);
            if (rg.IsMatch(postalCode))
                return false;
            else return true;
        }
    }
}
