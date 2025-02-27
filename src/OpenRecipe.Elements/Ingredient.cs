using System.ComponentModel;

namespace OpenRecipe.Elements;

/// <summary>
/// Represents an ingredient in a recipe.
/// </summary>
public class Ingredient
{
    /// <summary>
    /// Gets or sets the name of the ingredient.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity of the ingredient.
    /// </summary>
    public double Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit of measurement for the ingredient.
    /// </summary>
    public string? Unit { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the ingredient scales.
    /// </summary>
    [DefaultValue(true)]
    public bool IsScaling { get; set; } = true;
}
