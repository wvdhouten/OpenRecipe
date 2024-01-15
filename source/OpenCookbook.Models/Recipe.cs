using OpenCookbook.Models;
using YamlDotNet.Serialization;

namespace OpenCookbook.Model
{
    public class Recipe
    {
        [YamlIgnore]
        public required string Id { get; set; }

        public required string Title { get; set; }

        public string? Source { get; set; }

        public int? Servings { get; set; }

        public int? PrepTime { get; set; }

        public int? CookingTime { get; set; }

        public IList<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

        public IList<string> Directions { get; set; } = new List<string>();

        public IList<string> Tags { get; set; } = new List<string>();

        public string? Notes { get; set; }

        public IDictionary<string, object> Custom { get; set; } = new Dictionary<string, object>();
    }
}
