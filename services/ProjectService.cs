using System;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using TheWordForge.models;

namespace TheWordForge.services;

public static class ProjectService
{
    public static Project? CurrentProject { get; private set; }

    public static void NewProject(string title, string author, string genre, string filePath)
    {
        CurrentProject = new Project
        {
            Title = title,
            Author = author,
            Genre = genre,
            FilePath = filePath
        };
    }

    public static async Task LoadProjectAsync(string path)
    {
        using var fs = File.OpenRead(path);
        using var archive = new ZipArchive(fs, ZipArchiveMode.Read);
        var entry = archive.GetEntry("project.json");
        if (entry == null) return;
        using var reader = new StreamReader(entry.Open());
        var json = await reader.ReadToEndAsync();
        var node = JsonNode.Parse(json);
        if (node == null) return;
        CurrentProject = new Project
        {
            Title = node["title"]?.GetValue<string>() ?? string.Empty,
            Author = node["author"]?.GetValue<string>() ?? string.Empty,
            Genre = node["genre"]?.GetValue<string>() ?? string.Empty,
            FilePath = path
        };
    }

    public static Task SaveProjectAsync()
    {
        if (CurrentProject?.FilePath == null)
            return Task.CompletedTask;

        var options = new JsonSerializerOptions { WriteIndented = true };
        using var fs = File.Create(CurrentProject.FilePath);
        using var archive = new ZipArchive(fs, ZipArchiveMode.Create);

        void WriteJson(string entryName, object data)
        {
            var entry = archive.CreateEntry(entryName);
            using var writer = new StreamWriter(entry.Open());
            writer.Write(JsonSerializer.Serialize(data, options));
        }

        var projectJson = new
        {
            title = CurrentProject.Title,
            author = CurrentProject.Author,
            genre = CurrentProject.Genre,
            transitions = PreferencesService.TransitionsEnabled,
            autosave = new { enabled = false, mode = "", minutes = 0 },
            createdUtc = DateTime.UtcNow,
            modifiedUtc = DateTime.UtcNow,
            schemaVersion = 1
        };
        WriteJson("project.json", projectJson);

        WriteJson("transcript/index.json", new { chapters = new[] { new { id = 1, title = "" } } });
        archive.CreateEntry("transcript/chapters/");
        WriteJson("transcript/chapters/ch_1.json", new { scenes = Array.Empty<object>() });

        foreach (var bible in new[] { "characters", "locations", "items", "lore" })
        {
            archive.CreateEntry($"bibles/{bible}/");
            WriteJson($"bibles/{bible}/index.json", new { entries = Array.Empty<object>() });
            archive.CreateEntry($"bibles/{bible}/entries/");
        }

        archive.CreateEntry("outline/");
        WriteJson("outline/outline.json", new { });

        archive.CreateEntry("timeline/");
        WriteJson("timeline/timeline.json", new { events = Array.Empty<object>() });
        archive.CreateEntry("assets/");

        return Task.CompletedTask;
    }
}
