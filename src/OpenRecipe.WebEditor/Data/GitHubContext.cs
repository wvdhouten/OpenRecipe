using Octokit;

namespace OpenRecipe.WebEditor.Data
{
    public class GitHubContext
    {
        private readonly string _gitOwner = string.Empty;
        private readonly string _gitRepository = string.Empty;
        private readonly string _gitPat = string.Empty;

        public GitHubContext(string gitOwner, string gitRepository, string gitPat)
        {
            _gitOwner = gitOwner;
            _gitRepository = gitRepository;
            _gitPat = gitPat;
        }

        public async Task GetFiles()
        {
            var client = new GitHubClient(new ProductHeaderValue("OpenRecipe"))
            {
                Credentials = new Credentials(_gitPat)
            };

            await client.Repository.Content.GetAllContents(_gitOwner, _gitRepository);
        }

        public async Task<string> GetContent(string path)
        {
            var client = new GitHubClient(new ProductHeaderValue("OpenRecipe"))
            {
                Credentials = new Credentials(_gitPat)
            };

            var contents = await client.Repository.Content.GetAllContents(_gitOwner, _gitRepository, path);
            if (contents.Any())
                return contents[0].Content;

            return string.Empty;
        }

        public async Task SaveAsync(string path, string content, string branch = "main")
        {
            var client = new GitHubClient(new ProductHeaderValue("OpenRecipe"))
            {
                Credentials = new Credentials(_gitPat)
            };

            var existingContent = await client.Repository.Content.GetAllContentsByRef(_gitOwner, _gitRepository, branch);
            var existing = existingContent.FirstOrDefault(file => file.Path == path);

            if (existing != null)
                await client.Repository.Content.UpdateFile(_gitOwner, _gitRepository, path, new UpdateFileRequest($"Update {path}", content, existing.Sha, branch));
            else
                await client.Repository.Content.CreateFile(_gitOwner, _gitRepository, path, new CreateFileRequest($"Update {path}", content, branch));
        }
    }
}
