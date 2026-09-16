using System;
using System.Globalization;

namespace HVO.Scripts.Common
{
    public static class FormatNumber
    {
        private static readonly (double Divider, string Suffix)[] Prefixes = new (double, string)[]
        {
            (1e24, "AD"),
            (1e21, "AC"),
            (1e18, "AB"),
            (1e15, "AA"),
            (1e12, "T"),
            (1e9, "B"),
            (1e6, "M"),
            (1e3, "K")
        };

        public static string Format(double amount)
        {
            amount = Math.Round(amount, 4);
            bool isNegative = amount < 0;
            if (isNegative) amount = Math.Abs(amount);
            double maxValue = 9.999 * Math.Pow(10, 24);
            if (amount > maxValue) amount = maxValue;
            if (amount < 100_000)
            {
                string formatted = amount.ToString("N0", CultureInfo.InvariantCulture);
                return isNegative ? "-" + formatted : formatted;
            }

            foreach (var (divider, suffix) in Prefixes)
            {
                decimal decimalDivider = (decimal)divider;
                if (amount >= divider && amount < divider * 1000)
                {
                    decimal value = (decimal)amount / decimalDivider;
                    string compact = TrimToFourDigitsFloor(value);
                    return (isNegative ? "-" : "") + compact + $"{suffix}";
                }
            }

            return (isNegative ? "-" : "") + "9.99AD";
        }

        public static string Format(decimal amount)
        {
            bool isNegative = amount < 0;
            if (isNegative) amount = Math.Abs(amount);

            decimal maxValue = 9.999m * (decimal)Math.Pow(10, 24);
            if (amount > maxValue) amount = maxValue;

            if (amount < 100_000m)
            {
                string formatted = amount.ToString("N0", CultureInfo.InvariantCulture);
                return isNegative ? "-" + formatted : formatted;
            }

            foreach (var (divider, suffix) in Prefixes)
            {
                decimal decimalDivider = (decimal)divider;

                if (amount >= decimalDivider && amount < decimalDivider * 1000m)
                {
                    decimal value = amount / decimalDivider;

                    string compact = TrimToFourDigitsFloor(value);

                    return (isNegative ? "-" : "") + compact + $"{suffix}";
                }
            }

            return (isNegative ? "-" : "") + "9.99AD";
        }

        private static string TrimToFourDigitsFloor(decimal value)
        {
            string intPart = Math.Floor(value).ToString(CultureInfo.InvariantCulture);

            if (intPart.Length >= 4) return intPart;

            int availableDecimals = 4 - intPart.Length;

            decimal factor = 1m;
            for (int i = 0; i < availableDecimals; i++) factor *= 10m;

            decimal floored = Math.Floor(value * factor) / factor;

            string formatted = floored.ToString($"F{availableDecimals}", CultureInfo.InvariantCulture);

            return formatted.TrimEnd('0').TrimEnd('.');
        }

        private static string FormatShort(float amount, long divider, string suffix)
        {
            float value = amount / divider;
            return value.ToString(value % 1 == 0 ? "0" : "0.#") + suffix;
        }

        private static string FormatWithDots(float number)
        {
            return number.ToString("#,0").Replace(",", ".");
        }

        public static float FormatStringPercentToFloat(string input)
        {
            float effectBonus = 0;
            string numeric = input.Replace("%", "");
            if (float.TryParse(numeric, out float value))
            {
                effectBonus = value;
            }

            return effectBonus;
        }
    }
}