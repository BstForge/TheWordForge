using System.Collections.ObjectModel;
using System.ComponentModel;
using TheWordForge.models;

namespace TheWordForge;

public class TranscriptPanelViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Chapter> Chapters { get; } = new();

    public event PropertyChangedEventHandler? PropertyChanged;

    public TranscriptPanelViewModel()
    {
        var chapter = new Chapter { Title = "Chapter 1" };
        chapter.Scenes.Add(new Scene { Title = "Scene 1" });
        Chapters.Add(chapter);
    }
}
