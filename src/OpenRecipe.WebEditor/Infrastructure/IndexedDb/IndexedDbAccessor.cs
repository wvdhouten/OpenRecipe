using Microsoft.JSInterop;

namespace OpenRecipe.WebEditor.Infrastructure.IndexedDb;

public class IndexedDbAccessor : IAsyncDisposable
{
    private Lazy<IJSObjectReference> _accessorJsRef = new();
    private readonly IJSRuntime _jsRuntime;

    public IndexedDbAccessor(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    internal async Task InitializeAsync()
    {
        var containers = InitContainersAsync();

        await WaitForReference();
        await _accessorJsRef.Value.InvokeVoidAsync("initialize", DateTime.Now.ToString("yyyyMMddhhmmss"), containers.Keys);

        foreach (var container in containers.Values)
            await container.RefreshAsync();
    }

    private Dictionary<string, EntityContainer> InitContainersAsync()
    {
        var containers = new Dictionary<string, EntityContainer>();
        var properties = GetType().GetProperties();
        foreach (var property in properties)
        {
            var propertyType = property.PropertyType;
            if (TryInitContainer(propertyType, out var container))
            {
                var instance = Activator.CreateInstance(propertyType, this)
                    ?? throw new InvalidOperationException($"Failed to create instance of {propertyType}");

                property.SetValue(this, instance);

                if (instance is EntityContainer entityContainer)

                containers.Add(container, entityContainer);
            }
        }
        return containers;
    }

    private static bool TryInitContainer(Type? property, out string container)
    {
        container = string.Empty;

        if (property == null)
            return false;

        if (!property.IsGenericType || property.GetGenericTypeDefinition() != typeof(EntityContainer<>))
            return TryInitContainer(property.BaseType, out container);

        container = property.GenericTypeArguments.First().Name;
        return true;
    }

    internal async Task<IEnumerable<T>> GetAllAsync<T>(string collectionName)
    {
        await WaitForReference();
        var result = await _accessorJsRef.Value.InvokeAsync<IEnumerable<T>>("getAll", collectionName);

        return result;
    }

    internal async Task<T> GetValueAsync<T>(string collectionName, string id)
    {
        await WaitForReference();
        var result = await _accessorJsRef.Value.InvokeAsync<T>("get", collectionName, id);

        return result;
    }

    internal async Task SetValueAsync<T>(string collectionName, T value)
    {
        await WaitForReference();
        await _accessorJsRef.Value.InvokeVoidAsync("set", collectionName, value);
    }

    internal async Task RemoveValueAsync<T>(string collectionName, string id)
    {
        await WaitForReference();
        await _accessorJsRef.Value.InvokeVoidAsync("remove", collectionName, id);
    }

    internal async Task ClearAsync(string collectionName)
    {
        await WaitForReference();
        await _accessorJsRef.Value.InvokeVoidAsync("clear", collectionName);
    }

    private async Task WaitForReference()
    {
        if (!_accessorJsRef.IsValueCreated)
            _accessorJsRef = new(await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/IndexedDbAccessor.js"));
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        if (_accessorJsRef.IsValueCreated)
            await _accessorJsRef.Value.DisposeAsync();
    }
}
