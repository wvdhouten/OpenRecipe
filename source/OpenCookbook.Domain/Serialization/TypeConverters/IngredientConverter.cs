using YamlDotNet.Core.Events;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using OpenCookbook.Models;

namespace OpenCookbook.Domain.Serialization.TypeConverters
{
    internal sealed class IngredientConverter : IYamlTypeConverter
    {
        private static IngredientConverter? _instance;

        public static IngredientConverter Instance => _instance ??= new IngredientConverter();

        public bool Accepts(Type type) => type == typeof(Ingredient);

        public object ReadYaml(IParser parser, Type type)
        {
            var value = parser.Consume<Scalar>().Value;

            var ingredient = new Ingredient(value);

            return ingredient;
        }

        public void WriteYaml(IEmitter emitter, object? value, Type type)
        {
            var ingredient = (Ingredient)value!;

            var output = string.IsNullOrEmpty(ingredient.Unit)
                ? $"{ingredient.Quantity} {ingredient.Description}"
                : $"{ingredient.Quantity} {ingredient.Unit} {ingredient.Description}";

            emitter.Emit(new Scalar(output));
        }
    }
}
