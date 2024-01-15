using YamlDotNet.Core.Events;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace OpenCookbook.Domain.Serialization.TypeConverters
{
    internal sealed class UriConverter : IYamlTypeConverter
    {
        private static UriConverter? _instance;

        public static UriConverter Instance => _instance ??= new UriConverter();

        public bool Accepts(Type type) => type == typeof(Uri);

        public object ReadYaml(IParser parser, Type type)
        {
            var value = parser.Consume<Scalar>().Value;

            var ingredient = new Uri(value);

            return ingredient;
        }

        public void WriteYaml(IEmitter emitter, object? value, Type type)
        {
            var ingredient = (Uri)value!;

            emitter.Emit(new Scalar(ingredient.ToString()));
        }
    }
}
