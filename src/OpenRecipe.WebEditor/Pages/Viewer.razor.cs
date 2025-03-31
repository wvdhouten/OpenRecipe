using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OpenRecipe.Serialization;
using OpenRecipe.WebEditor.Components;
using OpenRecipe.WebEditor.Data;

namespace OpenRecipe.WebEditor.Pages;

public partial class Viewer
{
    [Inject] RecipeRepository RecipeRepository { get; set; } = null!;
    [Inject] Toaster Toaster { get; set; } = null!;

    [Parameter] public string? Id { get; set; } = null;
    private Recipe? Recipe { get; set; } = null;
    private int DesiredServings { get; set; } = 0;

    protected override async Task OnInitializedAsync()
    {
        if (Id == null)
            return;

        Recipe = await RecipeRepository.GetAsync(Id);
        if (Recipe == null)
            return;

        DesiredServings = Recipe.Servings ?? 0;
    }

    private double ScaleIngredient(Ingredient ingredient)
    {
        if (!ingredient.IsScaling)
            return ingredient.Quantity;

        return Math.Round(ingredient.Quantity / (Recipe?.Servings ?? 1) * DesiredServings, 2);
    }

    private async Task LoadFile(InputFileChangeEventArgs e)
    {
        Recipe = RecipeSerializer.Deserialize(await new StreamContent(e.File.OpenReadStream()).ReadAsStringAsync());
        await Toaster.CreateToastAsync($"Recipe '{Recipe.Name}' loaded.");
    }
}
