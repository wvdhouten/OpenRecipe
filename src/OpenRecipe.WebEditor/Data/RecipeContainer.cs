using OpenRecipe.WebEditor.Models;

namespace OpenRecipe.WebEditor.Data
{
    public class RecipeContainer : EntityContainer<RecipeEntity>
    {
        private List<string> Tags { get; } = [];

        public RecipeContainer(IndexedDbAccessor indexedDb) : base(indexedDb)
        {
        }

        public override Task RefreshCachesAsync()
        {
            Tags.Clear();
            Tags.AddRange(Entities.SelectMany(e => e.Value.Tags).Distinct().Order());

            return Task.CompletedTask;
        }

        public Task<IEnumerable<string>> GetTagsAsync()
        {
            return Task.FromResult(Tags.AsEnumerable());
        }
    }
}
