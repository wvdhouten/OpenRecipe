using OpenCookbook.Core.Extensions;
using OpenCookbook.Domain.Serialization.TypeConverters;
using OpenCookbook.Model;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace OpenCookbook.Domain.Serialization
{
    public class RecipeSerializer
    {
        public string Serialize(Recipe recipe)
        {
            var serializer = new SerializerBuilder()
                .WithNamingConvention(HyphenatedNamingConvention.Instance)
                .WithTypeConverter(IngredientConverter.Instance)
                .WithTypeConverter(UriConverter.Instance)
                .Build();

            var result = serializer.Serialize(recipe);

            recipe.Id = !string.IsNullOrEmpty(recipe.Id)
                ? recipe.Id
                : CreateSlug(recipe.Title).ToLowerInvariant();

            return result;
        }

        public Recipe Deserialize(string input, string slug)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(HyphenatedNamingConvention.Instance)
                .WithTypeConverter(IngredientConverter.Instance)
                .WithTypeConverter(UriConverter.Instance)
                .Build();

            var recipe = deserializer.Deserialize<Recipe>(input);
            recipe.Id = slug;

            return recipe;
        }

        private static string CreateSlug(string title)
        {
            title = title?.Trim().ToLowerInvariant().Replace(" ", "-", StringComparison.OrdinalIgnoreCase) ?? string.Empty;
            title = title.RemoveDiacritics();
            title = title.RemoveReservedUrlCharacters();

            return title.ToLowerInvariant();
        }
    }
}
