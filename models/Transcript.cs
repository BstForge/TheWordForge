namespace TheWordForge.models;

using System.Collections.ObjectModel;

public class Scene
{
    public string Title { get; set; } = "New Scene";
}

public class Chapter
{
    public string Title { get; set; } = "New Chapter";
    public ObservableCollection<Scene> Scenes { get; } = new();
}
