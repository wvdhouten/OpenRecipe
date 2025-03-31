using Alkaline64.Injectable;
using OpenRecipe.WebEditor.Infrastructure;
using OpenRecipe.WebEditor.Models;

namespace OpenRecipe.WebEditor.Data;

[Injectable(Lifetime.Singleton)]
public class RecipeRepository
{
    private readonly StorageContext _context;

    private Lazy<List<string>> Tags { get; } = new();

    public RecipeRepository(StorageContext context)
    {
        _context = context;
    }

    public async Task ClearAsync()
    {
        await _context.Recipes.ClearAsync();
        await RefreshTagsAsync();
    }

    public async Task<IEnumerable<RecipeEntity>> GetAsync() 
        => await _context.Recipes.GetAsync();

    public async Task<IEnumerable<RecipeEntity>> GetAsync(Func<RecipeEntity, bool> predicate) 
        => await _context.Recipes.GetAsync(predicate);

    public async Task<RecipeEntity?> GetAsync(string id) 
        => await _context.Recipes.GetAsync(id);

    public async Task<RecipeEntity> SetAsync(RecipeEntity entity)
    {
        var result = await _context.Recipes.SetAsync(entity);
        await RefreshTagsAsync();
        return result;
    }

    public async Task<RecipeEntity?> RemoveAsync(string id)
    {
        var result = await _context.Recipes.RemoveAsync(id);
        await RefreshTagsAsync();
        return result;
    }

    public async Task<IEnumerable<string>> GetTagsAsync()
    {
        if (!Tags.IsValueCreated)
            await RefreshTagsAsync();

        return Tags.Value.AsEnumerable();
    }

    private async Task RefreshTagsAsync()
    {
        Tags.Value.Clear();
        var recipes = await _context.Recipes.GetAsync();
        Tags.Value.AddRange(recipes.SelectMany(e => e.Tags).Distinct().Order());
    }
}
