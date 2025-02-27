using OpenRecipe;
using OpenRecipe.Domain.Serialization;
using YamlDotNet.Core.Events;
using YamlDotNet.Core;
using OpenRecipe.Serialization;

namespace OpenCookbook.RecipeEditor
{
    public partial class RecipeForm : Form
    {
        public RecipeForm()
        {
            InitializeComponent();
        }

        private void SerializeButton_Click(object sender, EventArgs e)
        {
            var recipe = new Recipe
            {
                Name = titleTextbox.Text,
                Reference = sourceTextbox.Text,
                Servings = Convert.ToInt32(servingsInput.Value),
                PrepTime = Convert.ToInt32(prepTimeInput.Value),
                CookingTime = Convert.ToInt32(cookingTimeInput.Value),
                Ingredients = ingredientsTextbox.Lines.Select(ParseLine).ToList(),
                Instructions = [.. directionsTextbox.Lines],
                Tags = [.. tagsTextbox.Lines]
            };

            var result = RecipeSerializer.Serialize(recipe);
            resultTextbox.Text = result;
        }

        private Ingredient ParseLine(string line)
        {
            var parts = line.Split(':', StringSplitOptions.TrimEntries);
            var value = parts[1].Split(' ');

            var ingredient = new Ingredient
            {
                Name = parts[0],
                Quantity = double.Parse(value[0].TrimStart('!')),
                IsScaling = !value[0].StartsWith('!')
            };

            if (value.Length > 1)
                ingredient.Unit = value[1];

            return ingredient;
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            var result = loadRecipeDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                var path = loadRecipeDialog.FileName;
                var content = File.ReadAllText(path);

                var recipe = RecipeSerializer.Deserialize(content);
                PopulateForm(recipe);
            }
        }

        private void PopulateForm(Recipe recipe)
        {
            titleTextbox.Text = recipe.Name;
            sourceTextbox.Text = recipe.Reference?.ToString() ?? string.Empty;
            servingsInput.Value = recipe.Servings ?? 0;
            prepTimeInput.Value = recipe.PrepTime ?? 0;
            cookingTimeInput.Value = recipe.CookingTime ?? 0;
            ingredientsTextbox.Text = string.Join(Environment.NewLine, recipe.Ingredients.Select(ingredient => $"{ingredient.Quantity} {ingredient.Unit} {ingredient.Name}"));
            directionsTextbox.Text = string.Join(Environment.NewLine, recipe.Instructions);
            tagsTextbox.Text = string.Join(Environment.NewLine, recipe.Tags);
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            PopulateForm(new Recipe { Name = string.Empty });
            resultTextbox.Text = string.Empty;
        }
    }
}
