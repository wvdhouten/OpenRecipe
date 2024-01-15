
namespace OpenCookbook.Domain.Converters
{
    public class TemperatureConverter
    {
        private const string Celcius = "C";
        private const string Fahrenheit = "F";

        public double Convert(double input, string source, string target)
        {
            source = source.Trim().ToUpperInvariant();
            target = target.Trim().ToUpperInvariant();

            if (source == target) 
                return  input;

            var result = source switch
            {
                Fahrenheit => FromFahrenheit(input, target),
                Celcius => FromCelcius(input, target),
                _ => throw new ArgumentException("Source unit is invalid.", nameof(source)),
            };

            return Math.Round(result, 2, MidpointRounding.AwayFromZero);
        }

        private static double FromFahrenheit(double input, string target)
        {
            return target switch
            {
                Celcius => (input - 32) * 5 / 9,
                _ => throw new ArgumentException("Target unit is invalid.", nameof(target)),
            };
        }

        private static double FromCelcius(double input, string target)
        {
            return target switch
            {
                Fahrenheit => (input * 9) / 5 + 32,
                _ => throw new ArgumentException("Target unit is invalid.", nameof(target)),
            };
        }
    }
}
