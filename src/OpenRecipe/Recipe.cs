namespace OpenRecipe;

/// <summary>
/// Represents a recipe incl. ingredients and instructions.
/// </summary>
public class Recipe
{
    /// <summary>
    /// Gets or sets the name of the recipe.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the recipe.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the source or reference of the recipe.
    /// </summary>
    public string? Source { get; set; }

    /// <summary>
    /// Gets or sets the number of servings the recipe yields.
    /// </summary>
    public int? Servings { get; set; }

    /// <summary>
    /// Gets or sets the preparation time in minutes.
    /// </summary>
    public int? PrepTime { get; set; }

    /// <summary>
    /// Gets or sets the cooking time in minutes.
    /// </summary>
    public int? CookingTime { get; set; }

    /// <summary>
    /// Gets or sets the list of ingredients required for the recipe.
    /// </summary>
    public IList<Ingredient> Ingredients { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of utilities required for preparing the recipe.
    /// </summary>
    public IList<string> Utilities { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of instructions for preparing the recipe.
    /// </summary>
    public IList<string> Instructions { get; set; } = [];

    /// <summary>
    /// Gets or sets the nutritional information of the recipe.
    /// </summary>
    public Nutrition? Nutrition { get; set; }

    /// <summary>
    /// Gets or sets the list of tags associated with the recipe.
    /// </summary>
    public IList<string> Tags { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of notes for the recipe.
    /// </summary>
    public IList<string> Notes { get; set; } = [];
}
