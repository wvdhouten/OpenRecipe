using Microsoft.Extensions.Options;

namespace OpenCookbook.Core
{
    public class FileManager
    {
        private readonly UserPreferences settings;

        public FileManager(IOptions<UserPreferences> options)
        {
            settings = options.Value;
        }

        public string ReadFile(string path)
        {
            return File.ReadAllText(path);
        }

        public void WriteFile(string fileName, string content)
        {
            var path = settings.RepoPath ?? "c:\\tmp";
            Directory.CreateDirectory(path);

            File.WriteAllText(Path.Combine(path, fileName), content);
        }
    }
}
