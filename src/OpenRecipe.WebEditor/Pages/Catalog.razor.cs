using Microsoft.AspNetCore.Components;
using OpenRecipe.WebEditor.Components;
using OpenRecipe.WebEditor.Data;
using OpenRecipe.WebEditor.Models;

namespace OpenRecipe.WebEditor.Pages;

public partial class Catalog
{
    [Inject] private RecipeRepository RecipeRepository { get; set; } = null!;
    [Inject] private Toaster Toaster { get; set; } = null!;

    private int RecipeCount = 0;
    private IEnumerable<string> KnownTags = [];
    private readonly IList<string> SelectedTags = [];

    private string NameFilter = string.Empty;
    private string IngredientFilter = string.Empty;

    private Confirm<RecipeEntity> ConfirmDeleteModal = new();

    private IEnumerable<RecipeEntity> SearchResults { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        SearchResults = await RecipeRepository.GetAsync();
        KnownTags = await RecipeRepository.GetTagsAsync();
        RecipeCount = SearchResults.Count();
    }

    private void UpdateTagSelection(string tag, bool isSelected)
    {
        if (isSelected)
            SelectedTags.Add(tag);
        else
            SelectedTags.Remove(tag);
    }

    private async Task ApplyFilterAsync()
    {
        SearchResults = await RecipeRepository.GetAsync(
            e => MatchesNameFilter(e)
            && MatchesIngredientFilter(e)
            && MatchesTagFilter(e)
        );
    }

    private bool MatchesNameFilter(RecipeEntity entity)
    {
        return entity.Name.Contains(NameFilter, StringComparison.OrdinalIgnoreCase)
        || (entity.Description != null
            && entity.Description.Contains(NameFilter, StringComparison.OrdinalIgnoreCase));
    }

    private bool MatchesIngredientFilter(RecipeEntity entity)
    {
        return entity.Ingredients.Any(i => i.Name.Contains(IngredientFilter, StringComparison.OrdinalIgnoreCase));
    }

    private bool MatchesTagFilter(RecipeEntity entity)
    {
        return SelectedTags.All(entity.Tags.Contains);
    }

    private void DeleteRecipe(RecipeEntity entity)
    {
        ConfirmDeleteModal.Show(entity, message: $"Are you sure you want to delete '{entity.Name}'?");
    }

    private async Task ConfirmDeleteRecipeAsync(RecipeEntity entity)
    {
        if (string.IsNullOrWhiteSpace(entity.Id))
            return;

        await RecipeRepository.RemoveAsync(entity.Id);

        var allRecipes = await RecipeRepository.GetAsync();
        KnownTags = await RecipeRepository.GetTagsAsync();
        RecipeCount = allRecipes.Count();

        await Toaster.CreateToastAsync($"Recipe '{entity.Name}' deleted.");

        await ApplyFilterAsync();
    }
}
