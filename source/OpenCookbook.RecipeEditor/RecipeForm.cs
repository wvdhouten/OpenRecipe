using Microsoft.Extensions.Options;
using OpenCookbook.Core;
using OpenCookbook.Domain.Serialization;
using OpenCookbook.Model;
using OpenCookbook.Models;
using System.IO;

namespace OpenCookbook.RecipeEditor
{
    public partial class RecipeForm : Form
    {
        private readonly RecipeSerializer serializer;
        private readonly FileManager fileManager;
        private readonly UserPreferences settings;

        public RecipeForm(IOptions<UserPreferences> options, RecipeSerializer serializer, FileManager fileManager)
        {
            InitializeComponent();

            settings = options.Value;
            loadRecipeDialog.InitialDirectory = settings.RepoPath;
            this.serializer = serializer;
            this.fileManager = fileManager;
        }

        private void SerializeButton_Click(object sender, EventArgs e)
        {
            var recipe = new Recipe
            {
                Title = titleTextbox.Text,
                Id = slugTextbox.Text,
                Source = sourceTextbox.Text,
                Servings = Convert.ToInt32(servingsInput.Value),
                PrepTime = Convert.ToInt32(prepTimeInput.Value),
                CookingTime = Convert.ToInt32(cookingTimeInput.Value),
                Ingredients = ingredientsTextbox.Lines.Select(line => new Ingredient(line)).ToList(),
                Directions = directionsTextbox.Lines.ToList(),
                Tags = tagsTextbox.Lines.ToList(),
                Notes = notesTextbox.Text
            };

            var result = serializer.Serialize(recipe);
            resultTextbox.Text = result;

            fileManager.WriteFile($"{recipe.Id}.yaml", result);
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            var result = loadRecipeDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                var path = loadRecipeDialog.FileName;
                var slug = Path.GetFileNameWithoutExtension(path);
                var content = fileManager.ReadFile(path);

                var recipe = serializer.Deserialize(content, slug);
                PopulateForm(recipe);
            }
        }

        private void PopulateForm(Recipe recipe)
        {
            titleTextbox.Text = recipe.Title;
            slugTextbox.Text = recipe.Id;
            sourceTextbox.Text = recipe.Source?.ToString() ?? string.Empty;
            servingsInput.Value = recipe.Servings ?? 0;
            prepTimeInput.Value = recipe.PrepTime ?? 0;
            cookingTimeInput.Value = recipe.CookingTime ?? 0;
            ingredientsTextbox.Text = string.Join(Environment.NewLine, recipe.Ingredients.Select(ingredient => $"{ingredient.Quantity} {ingredient.Unit} {ingredient.Description}"));
            directionsTextbox.Text = string.Join(Environment.NewLine, recipe.Directions);
            tagsTextbox.Text = string.Join(Environment.NewLine, recipe.Tags);
            notesTextbox.Text = recipe.Notes;
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            PopulateForm(new Recipe { Title = string.Empty, Id = string.Empty });
            resultTextbox.Text = string.Empty;
        }
    }
}
