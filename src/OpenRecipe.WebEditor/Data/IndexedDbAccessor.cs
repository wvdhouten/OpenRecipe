using Microsoft.JSInterop;

namespace OpenRecipe.WebEditor.Data;

public class IndexedDbAccessor : IAsyncDisposable
{
    private Lazy<IJSObjectReference> _accessorJsRef = new();
    private readonly IJSRuntime _jsRuntime;

    public IndexedDbAccessor(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeAsync(string collectionName)
    {
        await WaitForReference();
        await _accessorJsRef.Value.InvokeVoidAsync("initialize", collectionName);
    }

    public async Task<IEnumerable<T>> GetAllAsync<T>(string collectionName)
    {
        await WaitForReference();
        var result = await _accessorJsRef.Value.InvokeAsync<IEnumerable<T>>("getAll", collectionName);

        return result;
    }

    public async Task<T> GetValueAsync<T>(string collectionName, string id)
    {
        await WaitForReference();
        var result = await _accessorJsRef.Value.InvokeAsync<T>("get", collectionName, id);

        return result;
    }

    public async Task SetValueAsync<T>(string collectionName, T value)
    {
        await WaitForReference();
        await _accessorJsRef.Value.InvokeVoidAsync("set", collectionName, value);
    }

    public async Task RemoveValueAsync<T>(string collectionName, string id)
    {
        await WaitForReference();
        await _accessorJsRef.Value.InvokeVoidAsync("remove", collectionName, id);
    }

    public async Task ClearAsync(string collectionName)
    {
        await WaitForReference();
        await _accessorJsRef.Value.InvokeVoidAsync("clear", collectionName);
    }

    private async Task WaitForReference()
    {
        if (_accessorJsRef.IsValueCreated is false)
            _accessorJsRef = new(await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/IndexedDbAccessor.js"));
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        if (_accessorJsRef.IsValueCreated)
            await _accessorJsRef.Value.DisposeAsync();
    }
}
