using System.Globalization;

namespace Code.Utility
{
    public static class KiloFormatExtension
    {
        public static string KiloFormat(this int num)
        {
            if (num >= 100000000)
                return (num / 1000000).ToString("#,0M");

            if (num >= 10000000)
                return (num / 1000000).ToString("0.#") + "M";

            if (num >= 100000)
                return (num / 1000).ToString("#,0K");

            if (num >= 10000)
                return (num / 1000).ToString("0.#") + "K";

            return num.ToString("#,0");
        }

        public static string KiloFormat(this float num)
        {
            if (num >= 100000000)
                return (num / 1000000).ToString("#,0M");

            if (num >= 10000000)
                return (num / 1000000).ToString("0.#") + "M";

            if (num >= 100000)
                return (num / 1000).ToString("#,0K");

            if (num >= 10000)
                return (num / 1000).ToString("0.#") + "K";

            return num.ToString("#,0");
        }

        public static string KiloFormatShort(this int num)
        {
            if (num < 1000)
            {
                return num.ToString("#0", CultureInfo.InvariantCulture);
            }

            if (num < 10000)
            {
                num /= 10;
                return (num / 100f).ToString("#.0'k'", CultureInfo.InvariantCulture);
            }

            if (num < 1000000)
            {
                num /= 100;
                return (num / 10f).ToString("#.0'k'", CultureInfo.InvariantCulture);
            }

            if (num < 10000000)
            {
                num /= 10000;
                return (num / 100f).ToString("#.00'm'", CultureInfo.InvariantCulture);
            }

            num /= 100000;
            return (num / 10f).ToString("#,0.0'm'", CultureInfo.InvariantCulture);
        }


        private static readonly string[] ValueNames = new string[] { "A", "B", "C", "D", "E", "F" };

        public static string ToAbc(this float value)
        {
            var culture = CultureInfo.InvariantCulture;
            float r = 1000f;
            if (value < r) return value.ToString("0.##", culture);
            for (int i = 0; i < 6; i++)
            {
                if (value >= r && value < r * 1000f)
                {
                    float formatValue = value / r;
                    return $"{formatValue.ToString("0.##", culture)} {ValueNames[i]}";
                }

                r *= 1000f;
            }

            return value.ToString("0.##", culture);
        }

        public static string ToAbc(this int value)
        {
            var culture = CultureInfo.InvariantCulture;
            float r = 1000;
            if (value < r) return value.ToString("0.##", culture);
            for (int i = 0; i < 6; i++)
            {
                if (value >= r && value < r * 1000)
                {
                    var v = (int)(value / r);
                    return $"{v.ToString("0.##", culture)} {ValueNames[i]}";
                }

                r *= 1000;
            }

            return value.ToString("0.##", culture);
        }
    }
}