using OpenRecipe.WebEditor.Models;

namespace OpenRecipe.WebEditor.Infrastructure.IndexedDb;

public abstract class EntityContainer
{
    public abstract Task RefreshAsync();
}

public class EntityContainer<TEntity> : EntityContainer where TEntity : class, IEntity
{
    private readonly string _collectionName = typeof(TEntity).Name;
    private readonly IndexedDbAccessor _indexedDb;

    protected Dictionary<string, TEntity> Entities { get; } = [];

    public EntityContainer(IndexedDbAccessor indexedDb)
    {
        _indexedDb = indexedDb;
    }

    public override async Task RefreshAsync()
    {
        Entities.Clear();

        var entities = await _indexedDb.GetAllAsync<TEntity>(_collectionName);
        foreach (var entity in entities)
            Entities[entity.Id!] = entity;
    }

    public virtual Task RefreshCachesAsync() {
        return Task.CompletedTask;
    }

    public Task<IEnumerable<TEntity>> GetAsync()
    {
        return Task.FromResult(Entities.Values.AsEnumerable());
    }

    public Task<IEnumerable<TEntity>> GetAsync(Func<TEntity, bool> predicate)
    {
        return Task.FromResult(Entities.Values.Where(predicate));
    }

    public Task<TEntity?> GetAsync(string id)
    {
        Entities.TryGetValue(id, out var entity);

        return Task.FromResult(entity);
    }

    public async Task<TEntity> SetAsync(TEntity entity)
    {
        entity.Id ??= Ulid.NewUlid().ToString();
        Entities[entity.Id] = entity;
        await _indexedDb.SetValueAsync(_collectionName, entity);

        return entity;
    }

    public async Task<TEntity?> RemoveAsync(string id)
    {
        if (Entities.Remove(id, out var entity))
            await _indexedDb.RemoveValueAsync<TEntity>(_collectionName, id);

        return entity;
    }

    public async Task ClearAsync()
    {
        Entities.Clear();
        await _indexedDb.ClearAsync(_collectionName);
    }
}
