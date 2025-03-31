using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OpenRecipe.Serialization;
using OpenRecipe.WebEditor.Components;
using OpenRecipe.WebEditor.Data;
using OpenRecipe.WebEditor.Infrastructure;
using OpenRecipe.WebEditor.Models;
using OpenRecipe.WebEditor.Utils;
using System.Text.Json;

namespace OpenRecipe.WebEditor.Pages;

public partial class Settings
{
    [Inject] SettingsRepository SettingsRepository { get; set; } = null!;
    [Inject] RecipeRepository RecipeRepository { get; set; } = null!;
    [Inject] GitClient GitClient { get; set; } = null!;
    [Inject] Downloader Downloader { get; set; } = null!;
    [Inject] Toaster Toaster { get; set; } = null!;

    int RecipeCount = 0;

    private List<RepositoryInfo> WatchedRepositories = [];
    private RepositoryInfo NewWatchedRepository { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        await SettingsRepository.RefreshAll();

        GitClient.DefaultRepositoryInfo.Owner = SettingsRepository.DefaultGitOwner ?? string.Empty;
        GitClient.DefaultRepositoryInfo.Repository = SettingsRepository.DefaultGitRepository ?? string.Empty;
        GitClient.DefaultRepositoryInfo.Branch = SettingsRepository.DefaultGitBranch ?? string.Empty;
        GitClient.GitAccessToken = SettingsRepository.GitAccessToken ?? string.Empty;

        WatchedRepositories = await SettingsRepository.GetSetting(nameof(WatchedRepositories), new List<RepositoryInfo>());

        await UpdateCountsAsync();
    }

    private async Task ClearAsync()
    {
        await RecipeRepository.ClearAsync();
        await UpdateCountsAsync();

        await Toaster.ShowAsync("Local DB cleared.");
    }

    private async Task UpdateCountsAsync()
    {
        RecipeCount = (await RecipeRepository.GetAsync()).Count();
    }

    private async Task ImportRepositoryAsync(RepositoryInfo info)
    {
        var files = await GitClient.GetFilesAsync(info.Owner, info.Repository, info.Branch);
        foreach (var file in files)
            await TryImportFileAsync(file);

        await UpdateCountsAsync();

        await Toaster.ShowAsync($"Imported recipes from {info.Owner}/{info.Repository}:{info.Branch}.");
    }

    private async Task TryImportFileAsync(string path)
    {
        if (!path.EndsWith(".yuml"))
            return;

        var content = await GitClient.GetContentAsync(path);
        if (content is null)
            return;

        var recipe = RecipeSerializer.Deserialize<RecipeEntity>(content);
        recipe.Id ??= Ulid.NewUlid().ToString();

        await RecipeRepository.SetAsync(recipe);
    }

    private async Task ImportArchiveAsync(InputFileChangeEventArgs e)
    {
        using var memoryStream = new MemoryStream();
        using (var fileStream = new StreamContent(e.File.OpenReadStream(5 * 1024 * 1024)).ReadAsStream())
        {
            await fileStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
        }

        var contents = await ArchiveUtil.UnpackAsync(memoryStream);

        foreach (var (id, content) in contents)
        {
            var recipe = RecipeSerializer.Deserialize<RecipeEntity>(content);
            recipe.Id ??= Ulid.NewUlid().ToString();
            await RecipeRepository.SetAsync(recipe);
        }

        await UpdateCountsAsync();

        await Toaster.ShowAsync("Imported recipes from archive.");
    }

    private async Task ExportArchiveAsync()
    {
        var recipes = await RecipeRepository.GetAsync();

        var contents = recipes.Select(recipe => (recipe.Id!, RecipeSerializer.Serialize(recipe)));
        using var stream = await ArchiveUtil.PackAsync(contents);

        await Downloader.DownloadFile("open-recipe-archive.zip", stream);
    }

    private async void SaveSettings()
    {
        SettingsRepository.DefaultGitOwner = GitClient.DefaultRepositoryInfo.Owner;
        SettingsRepository.DefaultGitRepository = GitClient.DefaultRepositoryInfo.Repository;
        SettingsRepository.DefaultGitBranch = GitClient.DefaultRepositoryInfo.Branch;
        SettingsRepository.GitAccessToken = GitClient.GitAccessToken;

        await SettingsRepository.SetSetting(nameof(WatchedRepositories), JsonSerializer.Serialize(WatchedRepositories));

        await SettingsRepository.SaveAll();

        await Toaster.ShowAsync("Settings saved.");
    }

    private void AddWatchedRepository()
    {
        WatchedRepositories.Add(NewWatchedRepository);
        NewWatchedRepository = new();
    }

    private void RemoveWatchedRepository(RepositoryInfo watchedRepository)
    {
        WatchedRepositories.Remove(watchedRepository);
    }
}
