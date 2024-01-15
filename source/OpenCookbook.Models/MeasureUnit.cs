using System.Collections.ObjectModel;

namespace OpenCookbook.Models
{
    public class MeasureUnit
    {
        public static ICollection<string> KnownUnits { get; } = new Collection<string>
        {
            Milliliter, Centiliter, Deciliter, Liter,
            Gram, Kilogram,
            Teaspoon, Tablespoon
        };

        public const string Milliliter = "ml";
        public const string Centiliter = "cl";
        public const string Deciliter = "dl";
        public const string Liter = "l";

        public const string Gram = "g";
        public const string Kilogram = "kg";

        public const string Teaspoon = "tsp";
        public const string Tablespoon = "tbsp";
    }
}
