using System.Collections.ObjectModel;
using TheWordForge.models;

namespace TheWordForge;

public class TranscriptPanelViewModel
{
    public ObservableCollection<Chapter> Chapters { get; } = new();

    public TranscriptPanelViewModel()
    {
        var chapter = new Chapter { Title = "Chapter 1" };
        chapter.Scenes.Add(new Scene { Title = "Scene 1" });
        Chapters.Add(chapter);
    }
}
