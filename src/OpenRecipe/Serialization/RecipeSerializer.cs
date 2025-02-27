using OpenRecipe.Serialization.Converters;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace OpenRecipe.Serialization;

public class RecipeSerializer
{
    public static string Serialize(Recipe recipe)
    {
        var serializer = new SerializerBuilder()
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull | DefaultValuesHandling.OmitDefaults | DefaultValuesHandling.OmitEmptyCollections)
            .WithNamingConvention(HyphenatedNamingConvention.Instance)
            .WithTypeConverter(IngredientConverter.Initialize(HyphenatedNamingConvention.Instance))
            .Build();

        var result = serializer.Serialize(recipe);

        return result;
    }

    public static Recipe Deserialize(string input)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(HyphenatedNamingConvention.Instance)
            .WithTypeConverter(IngredientConverter.Initialize(HyphenatedNamingConvention.Instance))
            .Build();

        var recipe = deserializer.Deserialize<Recipe>(input);

        return recipe;
    }
}
