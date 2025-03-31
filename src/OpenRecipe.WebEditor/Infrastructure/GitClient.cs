using Alkaline64.Injectable;
using Octokit;
using OpenRecipe.WebEditor.Models;

namespace OpenRecipe.WebEditor.Infrastructure;

[Injectable(Lifetime.Singleton)]
public class GitClient
{
    private const string ProductName = "OpenRecipe";

    public string GitAccessToken { get; set; } = string.Empty;
    public RepositoryInfo DefaultRepositoryInfo { get; set; } = new();

    private GitHubClient? currentClient = null;

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

        var client = GetClient();
        if (existing is not null)
            await client.Repository.Content.UpdateFile(
                owner ?? DefaultRepositoryInfo.Owner,
                repository ?? DefaultRepositoryInfo.Repository,
                path,
                new UpdateFileRequest($"Update {path}", content, existing.Sha, branch ?? DefaultRepositoryInfo.Branch));
        else
            await client.Repository.Content.CreateFile(
                owner ?? DefaultRepositoryInfo.Owner,
                repository ?? DefaultRepositoryInfo.Repository,
                path,
                new CreateFileRequest($"Update {path}", content, branch ?? DefaultRepositoryInfo.Branch));
    }

    private GitHubClient GetClient()
    {
        if (currentClient is null || currentClient.Credentials.Password != GitAccessToken)
        {
            currentClient = new GitHubClient(new ProductHeaderValue(ProductName))
            {
                Credentials = new Credentials(GitAccessToken)
            };
        }
        return currentClient;
    }

    private async Task<IReadOnlyList<RepositoryContent>> GetContents(string path, string? owner, string? repository, string? branch)
    {
        var client = GetClient();

        if (string.IsNullOrEmpty(path))
            return await client.Repository.Content.GetAllContentsByRef(
                owner ?? DefaultRepositoryInfo.Owner,
                repository ?? DefaultRepositoryInfo.Repository,
                branch ?? DefaultRepositoryInfo.Branch);

        return await client.Repository.Content.GetAllContentsByRef(
            owner ?? DefaultRepositoryInfo.Owner,
            repository ?? DefaultRepositoryInfo.Repository,
            path,
            branch ?? DefaultRepositoryInfo.Branch);
    }
}
