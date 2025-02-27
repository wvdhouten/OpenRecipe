namespace OpenRecipe;

/// <summary>
/// Represents the nutritional information for a recipe.
/// </summary>
public class Nutrition
{
    /// <summary>
    /// Gets or sets the number of servings.
    /// </summary>
    public int Servings { get; set; }

    /// <summary>
    /// Gets or sets the nutritional values.
    /// </summary>
    public Dictionary<string, NutritionalValue> Values { get; set; } = [];
}
