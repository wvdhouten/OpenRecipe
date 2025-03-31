using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OpenRecipe.Serialization;
using OpenRecipe.WebEditor.Components;
using OpenRecipe.WebEditor.Data;
using OpenRecipe.WebEditor.Infrastructure;
using OpenRecipe.WebEditor.Models;

namespace OpenRecipe.WebEditor.Pages
{
    public partial class Editor
    {
        [Inject] RecipeRepository RecipeRepository { get; set; } = null!;
        [Inject] SettingsRepository SettingsRepository { get; set; } = null!;
        [Inject] GitClient GitClient { get; set; } = null!;
        [Inject] Toaster Toaster { get; set; } = null!;

        [Parameter] public string? Id { get; set; } = null;

        private RecipeEntity Recipe { get; set; } = new RecipeEntity { Id = Ulid.NewUlid().ToString() };

        private List<string> KnownTags = [];

        private bool CanSaveToCloud
            => !string.IsNullOrEmpty(SettingsRepository.DefaultGitOwner)
            && !string.IsNullOrEmpty(SettingsRepository.DefaultGitRepository)
            && !string.IsNullOrEmpty(SettingsRepository.DefaultGitBranch);

        protected override async Task OnInitializedAsync()
        {
            await RefreshData();

            if (Id != null)
                Recipe = (await RecipeRepository.GetAsync(Id)) ?? new RecipeEntity { Id = Ulid.NewUlid().ToString() };
        }

        protected async Task RefreshData()
        {
            KnownTags = [.. await RecipeRepository.GetTagsAsync()];
        }

        private async Task NewRecipe()
        {
            Recipe = new RecipeEntity { Id = Ulid.NewUlid().ToString() };

            await RefreshData();
        }

        private async Task LoadFile(InputFileChangeEventArgs e)
        {
            var serializedRecipe = await new StreamContent(e.File.OpenReadStream()).ReadAsStringAsync();
            Recipe = RecipeSerializer.Deserialize<RecipeEntity>(serializedRecipe);

            foreach (var tag in Recipe.Tags)
            {
                if (!KnownTags.Contains(tag))
                    KnownTags.Add(tag);
            }

            await Toaster.CreateToastAsync($"Recipe '{Recipe.Name}' loaded.");
        }

        private async Task SaveRecipe()
        {
            Recipe.Id ??= Ulid.NewUlid().ToString();

            await RecipeRepository.SetAsync(Recipe);
            await Toaster.CreateToastAsync($"Recipe '{Recipe.Name}' saved.");

            await RefreshData();
        }

        private async Task CloudSaveRecipe()
        {
            await SaveRecipe();

            await GitClient.SaveAsync($"{Recipe.Id}.yuml",
                RecipeSerializer.Serialize(Recipe),
                SettingsRepository.DefaultGitOwner,
                SettingsRepository.DefaultGitRepository,
                SettingsRepository.DefaultGitBranch);
            await Toaster.CreateToastAsync($"Recipe '{Recipe.Name}' saved in git repository.");

            await RefreshData();
        }

        #region Ingredients

        private string newIngredient = string.Empty;

        private void AddIngredient()
        {
            if (string.IsNullOrWhiteSpace(newIngredient))
                return;

            Recipe.Ingredients.Add(new Ingredient { Name = newIngredient.Trim() });
            newIngredient = string.Empty;
        }

        private void RemoveIngredient(Ingredient ingredient)
        {
            Recipe.Ingredients.Remove(ingredient);
        }

        #endregion

        #region IngredientNotes

        private static void AddNote(Ingredient ingredient)
        {
            ingredient.Notes.Add(string.Empty);
        }

        private static void RemoveNote(Ingredient ingredient, int index)
        {
            ingredient.Notes.RemoveAt(index);
        }

        #endregion

        #region Utilities

        private string newUtility = string.Empty;

        private void AddUtility()
        {
            if (string.IsNullOrWhiteSpace(newUtility))
                return;

            Recipe.Utilities.Add(newUtility.Trim());
            newUtility = string.Empty;
        }

        private void RemoveUtility(int index)
        {
            Recipe.Utilities.RemoveAt(index);
        }

        #endregion

        #region Instructions

        private string newInstruction = string.Empty;

        private void AddInstruction()
        {
            if (string.IsNullOrWhiteSpace(newInstruction.Trim()))
                return;

            Recipe.Instructions.Add(newInstruction.Trim());
            newInstruction = string.Empty;
        }

        private void RemoveInstruction(int index)
        {
            Recipe.Instructions.RemoveAt(index);
        }

        #endregion

        #region Tags

        private string newTag = string.Empty;

        private void AddTag()
        {
            if (string.IsNullOrWhiteSpace(newTag))
                return;

            KnownTags.Add(newTag);
            Recipe.Tags.Add(newTag.Trim());
            newTag = string.Empty;
        }

        private void UpdateTagSelection(string tag, bool isSelected)
        {
            if (isSelected)
                Recipe.Tags.Add(tag);
            else
                Recipe.Tags.Remove(tag);
        }

        #endregion

        #region Notes

        private string newNote = string.Empty;

        private void AddNote()
        {
            if (string.IsNullOrWhiteSpace(newNote))
                return;

            Recipe.Notes.Add(newNote.Trim());
            newNote = string.Empty;
        }

        private void RemoveNote(int index)
        {
            Recipe.Notes.RemoveAt(index);
        }

        #endregion

        #region Metadata

        private string newMetadataKey = string.Empty;
        private string newMetadataValue = string.Empty;

        private void AddMetadata()
        {
            if (string.IsNullOrWhiteSpace(newMetadataKey) || Recipe.Metadata.ContainsKey(newMetadataKey))
                return;

            Recipe.Metadata.Add(newMetadataKey.Trim(), newMetadataValue.Trim());

            newMetadataKey = string.Empty;
            newMetadataValue = string.Empty;
        }

        private void RemoveMetadata(string key)
        {
            Recipe.Metadata.Remove(key);
        }

        #endregion
    }
}
