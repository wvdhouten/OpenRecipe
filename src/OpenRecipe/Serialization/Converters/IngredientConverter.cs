using YamlDotNet.Core.Events;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using System.Text;

namespace OpenRecipe.Serialization.Converters
{
    internal sealed class IngredientConverter : IYamlTypeConverter
    {
        private static IngredientConverter? _instance;
        private readonly INamingConvention _namingConvention;

        private IngredientConverter(INamingConvention namingConvention)
        {
            _namingConvention = namingConvention;
        }

        public static IngredientConverter Initialize(INamingConvention namingConvention) => _instance ??= new IngredientConverter(namingConvention);

        public bool Accepts(Type type) => type == typeof(Ingredient);

        public object ReadYaml(IParser parser, Type type)
        {
            var ingredient = new Ingredient();
            parser.Consume<MappingStart>();

            while (parser.TryConsume<Scalar>(out var scalar))
            {
                var propertyName = scalar.Value;
                if (propertyName == _namingConvention.Apply(nameof(Ingredient.IsScaling)))
                    ParseIsScaling(parser, ingredient);
                else if (propertyName == _namingConvention.Apply(nameof(Ingredient.Notes)))
                    ParseNotes(parser, ingredient);
                else
                    ParseIngredient(parser, ingredient, propertyName);
            }

            parser.Consume<MappingEnd>();
            return ingredient;
        }

        private static void ParseIngredient(IParser parser, Ingredient ingredient, string propertyName)
        {
            ingredient.Name = propertyName;
            var value = parser.Consume<Scalar>().Value.Split(' ');
            ingredient.Quantity = double.Parse(value[0]);
            if (value.Length > 1)
                ingredient.Unit = value[1];
        }

        private static void ParseNotes(IParser parser, Ingredient ingredient)
        {
            var notes = new List<string>();
            parser.Consume<SequenceStart>();
            while (parser.TryConsume<Scalar>(out var note))
                notes.Add(note.Value);

            parser.Consume<SequenceEnd>();
            ingredient.Notes = notes;
        }

        private static void ParseIsScaling(IParser parser, Ingredient ingredient)
        {
            ingredient.IsScaling = bool.Parse(parser.Consume<Scalar>().Value);
        }

        public void WriteYaml(IEmitter emitter, object? value, Type type)
        {
            var ingredient = (Ingredient)value!;

            var output = new StringBuilder();
            output.Append(ingredient.Quantity);

            if (!string.IsNullOrWhiteSpace(ingredient.Unit))
                output.Append($" {ingredient.Unit}");

            emitter.Emit(new MappingStart());

            emitter.Emit(new Scalar(ingredient.Name));
            emitter.Emit(new Scalar($"{ingredient.Quantity}{ingredient.Unit}"));

            if (!ingredient.IsScaling)
            {
                emitter.Emit(new Scalar(_namingConvention.Apply(nameof(Ingredient.IsScaling))));
                emitter.Emit(new Scalar(ingredient.IsScaling.ToString().ToLower()));
            }

            if (ingredient.Notes.Any())
            {
                emitter.Emit(new Scalar(_namingConvention.Apply(nameof(Ingredient.Notes))));
                emitter.Emit(new SequenceStart(null, null, false, SequenceStyle.Block));
                foreach (var note in ingredient.Notes)
                {
                    emitter.Emit(new Scalar(note));
                }
                emitter.Emit(new SequenceEnd());
            }

            emitter.Emit(new MappingEnd());
        }
    }
}
