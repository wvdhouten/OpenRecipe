namespace OpenRecipe;

/// <summary>
/// Represents a nutritional value in a recipe.
/// </summary>
public class NutritionalValue
{
  /// <summary>
  /// Gets or sets the value of the nutritional component.
  /// </summary>
  public double Value { get; set; }

  /// <summary>
  /// Gets or sets the unit of the nutritional value.
  /// </summary>
  public string Unit { get; set; } = string.Empty;
}
