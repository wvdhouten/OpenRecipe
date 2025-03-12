using OpenRecipe.WebEditor.Models;

namespace OpenRecipe.WebEditor.Data
{
    public class EntityRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly string _collectionName = typeof(TEntity).Name.ToLowerInvariant();
        private readonly Dictionary<string, TEntity> _entities = [];
        private readonly IndexedDbAccessor _indexedDb;

        public EntityRepository(IndexedDbAccessor indexedDb)
        {
            _indexedDb = indexedDb;
        }

        public async Task InitAsync()
        {

            await _indexedDb.InitializeAsync(_collectionName);
            await RefreshAsync();
        }

        public async Task RefreshAsync()
        {
            _entities.Clear();

            var entities = await _indexedDb.GetAllAsync<TEntity>(_collectionName);
            foreach (var entity in entities)
                _entities[entity.Id!] = entity;
        }

        public Task<IEnumerable<TEntity>> GetAsync()
        {
            return Task.FromResult(_entities.Values.AsEnumerable());
        }

        public Task<IEnumerable<TEntity>> GetAsync(Func<TEntity, bool> predicate)
        {
            return Task.FromResult(_entities.Values.Where(predicate));
        }

        public Task<TEntity?> GetAsync(string id)
        {
            _entities.TryGetValue(id, out var entity);

            return Task.FromResult(entity);
        }

        public async Task<TEntity> SetAsync(TEntity entity)
        {
            entity.Id ??= Ulid.NewUlid().ToString();
            _entities[entity.Id] = entity;
            await _indexedDb.SetValueAsync(_collectionName, entity);

            return entity;
        }

        public async Task<TEntity?> RemoveAsync(string id)
        {
            if (_entities.Remove(id, out var entity))
                await _indexedDb.RemoveValueAsync<TEntity>(_collectionName, id);

            return entity;
        }

        public async Task ClearAsync()
        {
            _entities.Clear();
            await _indexedDb.ClearAsync(_collectionName);
        }
    }
}
