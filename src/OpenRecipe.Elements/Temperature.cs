using System.ComponentModel;

namespace OpenRecipe.Elements;

/// <summary>
/// Represents a temperature with a value and a unit.
/// </summary>
public class Temperature
{
    /// <summary>
    /// Gets or sets the temperature value.
    /// </summary>
    public double Value { get; set; }

    /// <summary>
    /// Gets or sets the unit of the temperature. Default is Celsius ("C").
    /// </summary>
    [DefaultValue("C")]
    public string Unit { get; set; } = "C";
}
