using Alkaline64.Injectable;
using OpenRecipe.WebEditor.Infrastructure;
using OpenRecipe.WebEditor.Models;
using System.Text.Json;

namespace OpenRecipe.WebEditor.Data;

[Injectable(Lifetime.Singleton)]
public class SettingsRepository
{
    private readonly StorageContext _context;

    public string GitAccessToken { get; set; } = string.Empty;

    public string? DefaultGitOwner { get; set; }

    public string? DefaultGitRepository { get; set; }

    public string? DefaultGitBranch { get; set; }

    public SettingsRepository(StorageContext context)
    {
        _context = context;
    }

    public async Task RefreshAll()
    {
        GitAccessToken = await GetSetting(nameof(GitAccessToken));
        DefaultGitOwner = await GetSetting(nameof(DefaultGitOwner));
        DefaultGitRepository = await GetSetting(nameof(DefaultGitRepository));
        DefaultGitBranch = await GetSetting(nameof(DefaultGitBranch));
    }

    public async Task SaveAll()
    {
        await SetSetting(nameof(GitAccessToken), GitAccessToken);
        await SetSetting(nameof(DefaultGitOwner), DefaultGitOwner);
        await SetSetting(nameof(DefaultGitRepository), DefaultGitRepository);
        await SetSetting(nameof(DefaultGitBranch), DefaultGitBranch);
    }

    public async Task<string> GetSetting(string key)
    {
        var setting = await _context.Settings.GetAsync(key);
        return setting?.Value ?? string.Empty;
    }

    public async Task<TValue> GetSetting<TValue>(string key, TValue defaultValue = default!)
    {
        var setting = await _context.Settings.GetAsync(key);
        if (string.IsNullOrWhiteSpace(setting?.Value))
            return defaultValue;

        var value = JsonSerializer.Deserialize<TValue>(setting?.Value ?? string.Empty);
        return value ?? defaultValue;
    }

    public async Task SetSetting(string key, string? value)
    {
        if (value is null)
            await _context.Settings.RemoveAsync(key);
        else
            await _context.Settings.SetAsync(new SettingEntity { Id = key, Value = value });
    }

    public async Task SetSetting<TValue>(string key, TValue? value)
    {
        if (value is null)
            await _context.Settings.RemoveAsync(key);
        else
        {
            var serializedValue = JsonSerializer.Serialize(value);
            await _context.Settings.SetAsync(new SettingEntity { Id = key, Value = serializedValue });
        }
    }
}
