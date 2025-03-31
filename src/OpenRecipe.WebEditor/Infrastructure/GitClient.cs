using Alkaline64.Injectable;
using Octokit;
using OpenRecipe.WebEditor.Data;

namespace OpenRecipe.WebEditor.Infrastructure;

[Injectable(Lifetime.Singleton)]
public class GitClient
{
    private const string ProductName = "OpenRecipe";

    private readonly SettingsRepository _settingsRepository;

    private GitHubClient? currentClient = null;

    public GitClient(SettingsRepository settingsRepository)
    {
        _settingsRepository = settingsRepository;
    }

    public async Task<IEnumerable<string>> GetFilesAsync(string? owner = null, string? repository = null, string? branch = null)
    {
        var contents = await GetContents(string.Empty, owner, repository, branch);
        return contents.Select(content => content.Path);
    }

    // TODO: Convert to result pattern?
    public async Task<string?> GetContentAsync(string path, string? owner = null, string? repository = null, string? branch = null)
    {
        var contents = await GetContents(path, owner, repository, branch);
        if (contents.Any())
            return contents[0].Content;

        return null;
    }

    public async Task SaveAsync(string path, string content, string? owner = null, string? repository = null, string? branch = null)
    {
        var files = await GetContents(string.Empty, owner, repository, branch);
        var existing = files.FirstOrDefault(file => file.Path == path);

        var client = await GetClient();
        if (existing is not null)
            await client.Repository.Content.UpdateFile(
                owner ?? _settingsRepository.DefaultGitOwner,
                repository ?? _settingsRepository.DefaultGitRepository,
                path,
                new UpdateFileRequest($"Update {path}", content, existing.Sha, branch ?? _settingsRepository.DefaultGitBranch));
        else
            await client.Repository.Content.CreateFile(
                owner ?? _settingsRepository.DefaultGitOwner,
                repository ?? _settingsRepository.DefaultGitRepository,
                path,
                new CreateFileRequest($"Update {path}", content, branch ?? _settingsRepository.DefaultGitBranch));
    }

    private async Task<GitHubClient> GetClient()
    {
        if (currentClient is null || currentClient.Credentials.Password != _settingsRepository.GitAccessToken)
        {
            if (string.IsNullOrEmpty(_settingsRepository.GitAccessToken))
                await _settingsRepository.RefreshAll();

            currentClient = new GitHubClient(new ProductHeaderValue(ProductName))
            {
                Credentials = new Credentials(_settingsRepository.GitAccessToken)
            };
        }
        return currentClient;
    }

    private async Task<IReadOnlyList<RepositoryContent>> GetContents(string path, string? owner, string? repository, string? branch)
    {
        var client = await GetClient();

        if (string.IsNullOrEmpty(path))
            return await client.Repository.Content.GetAllContentsByRef(
                owner ?? _settingsRepository.DefaultGitOwner,
                repository ?? _settingsRepository.DefaultGitRepository,
                branch ?? _settingsRepository.DefaultGitBranch);

        return await client.Repository.Content.GetAllContentsByRef(
            owner ?? _settingsRepository.DefaultGitOwner,
            repository ?? _settingsRepository.DefaultGitRepository,
            path,
            branch ?? _settingsRepository.DefaultGitBranch);
    }
}
