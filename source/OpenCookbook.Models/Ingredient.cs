namespace OpenCookbook.Models
{
    public class Ingredient
    {
        public double Quantity { get; set; }

        public string? Unit { get; set; }

        public string Description { get; set; }

        public Ingredient(double quantity, string? unit, string description)
        {
            Quantity = quantity;
            Unit = unit;
            Description = description;
        }

        public Ingredient(string input)
        {
            var values = input.Split(' ', 3, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            Quantity = double.Parse(values[0]);

            if (MeasureUnit.KnownUnits.Contains(values[1]))
            {
                Unit = values[1];
                Description = values[2];
            }
            else
            {
                Unit = string.Empty;
                Description = string.Join(' ', values.Skip(1));
            }
        }
    }
}
