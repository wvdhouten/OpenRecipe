using Alkaline64.Injectable;
using Microsoft.JSInterop;
using OpenRecipe.WebEditor.Infrastructure.IndexedDb;
using OpenRecipe.WebEditor.Models;

namespace OpenRecipe.WebEditor.Infrastructure;

[Injectable(Lifetime.Singleton)]
public class StorageContext : IndexedDbAccessor // TODO: Avoid exposing IndexedDbAccessor.
{
    public EntityContainer<SettingEntity> Settings { get; private set; } = null!;

    public EntityContainer<RecipeEntity> Recipes { get; private set; } = null!;

    public StorageContext(IJSRuntime runtime) : base(runtime) { }
}
