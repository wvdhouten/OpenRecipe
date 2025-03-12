using OpenRecipe.WebEditor.Models;

namespace OpenRecipe.WebEditor.Data
{
    public class RecipeRepository : EntityRepository<RecipeEntity>
    {
        private List<string> Tags { get; } = [];

        public RecipeRepository(IndexedDbAccessor indexedDb) : base(indexedDb)
        {
        }

        public override Task RefreshCachesAsync()
        {
            Tags.Clear();
            Tags.AddRange(Entities.SelectMany(e => e.Value.Tags).Distinct());

            return Task.CompletedTask;
        }

        public Task<IEnumerable<string>> GetTagsAsync()
        {
            return Task.FromResult(Tags.AsEnumerable());
        }
    }
}
