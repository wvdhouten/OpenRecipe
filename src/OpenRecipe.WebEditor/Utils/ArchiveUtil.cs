using System.IO.Compression;

namespace OpenRecipe.WebEditor.Utils;

public class ArchiveUtil
{
    public static async Task<Stream> PackAsync(IEnumerable<(string key, string content)> entities)
    {
        var stream = new MemoryStream();
        var archive = new ZipArchive(stream, ZipArchiveMode.Create, true);
        foreach (var (key, content) in entities)
        {
            var entry = archive.CreateEntry($"{key}.yuml", CompressionLevel.Optimal);
            using var entryStream = entry.Open();
            using var writer = new StreamWriter(entryStream) { AutoFlush = true };
            await writer.WriteAsync(content);
        }

        archive.Dispose();
        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }

    public static async Task<IEnumerable<(string key, string content)>> UnpackAsync(Stream stream)
    {
        var archive = new ZipArchive(stream, ZipArchiveMode.Read);
        var entityTasks = archive.Entries.Select(async e => await UnpackEntryAsync(e));

        await Task.WhenAll(entityTasks);
        return entityTasks.Select(t => t.Result);
    }

    private static async Task<(string key, string content)> UnpackEntryAsync(ZipArchiveEntry entry)
    {
        var key = entry.Name;
        using var entryStream = entry.Open();
        using var reader = new StreamReader(entryStream);
        var content = await reader.ReadToEndAsync();

        return (key, content);
    }
}
