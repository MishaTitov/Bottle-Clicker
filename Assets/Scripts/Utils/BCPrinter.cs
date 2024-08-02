using System;
using System.Numerics;

namespace BCUtils
{
    public class BCPrinter
    {
        private static string[] shortSuffixes = { "", "k", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc", "Ud", "Dd", "Td", "Qad", "Qid", "Sxd", "Spd", "Ocd", "Nod", "Vg", "Uvg" };
        private static string[] fullSuffixes = { "", " Thousand", " Million", " Billion", " Trillion", " Quadrillion", " Quintillion", " Sextillion", " Septillion",
        " Octillion", " Nonillion", " Decillion", " Undecillion", " Duodecillion", " Tredecillion", " Quattuordecillion", " Quindecillion", " Sexdecillion", " Septendecillion",
        " Octodecillion", " Novemdecillion", " Vigintillion", " Unvigintillion"};

        public static string FormatBigInteger(BigInteger number, bool isShort)
        {
            int magnitude = (int)Math.Floor((BigInteger.Log10(number) + 1) / 3);
            // If the magnitude is too large, use the highest suffix
            if (magnitude >= shortSuffixes.Length)
            {
                if (isShort)
                    return number.ToString("0.##") + shortSuffixes[shortSuffixes.Length - 1];
                else
                    return number.ToString("0.##") + fullSuffixes[fullSuffixes.Length - 1];
            }
            else if (magnitude < 0)
                magnitude = 0;

            double scaledNumber = (double)number / Math.Pow(1000, magnitude);
            if (scaledNumber < 1)
            {
                scaledNumber *= 1000;
                magnitude--;
                if (magnitude < 0)
                    magnitude = 0;
            }
            if (isShort)
                return scaledNumber.ToString("0.##") + shortSuffixes[magnitude];
            else
                return scaledNumber.ToString("0.##") + fullSuffixes[magnitude];
        }
    }
}