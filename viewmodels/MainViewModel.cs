using System.ComponentModel;
using Avalonia.Controls;
using TheWordForge.menus.top;
using TheWordForge.panels;

namespace TheWordForge;

public enum ActivePanel
{
    Transcript,
    Timeline,
    Outline,
    CharacterBible,
    LocationBible,
    ItemBible,
    LoreBible
}

public class MainViewModel : INotifyPropertyChanged
{
    private ActivePanel _activePanel;
    private UserControl _currentCenterPanel = new TranscriptPanel();
    private UserControl _currentTopMenu = new TranscriptTopMenu();

    public event PropertyChangedEventHandler? PropertyChanged;

    public ActivePanel ActivePanel
    {
        get => _activePanel;
        set
        {
            if (_activePanel != value)
            {
                _activePanel = value;
                UpdatePanels();
                OnPropertyChanged(nameof(ActivePanel));
            }
        }
    }

    public UserControl CurrentCenterPanel
    {
        get => _currentCenterPanel;
        private set
        {
            if (_currentCenterPanel != value)
            {
                _currentCenterPanel = value;
                OnPropertyChanged(nameof(CurrentCenterPanel));
            }
        }
    }

    public UserControl CurrentTopMenu
    {
        get => _currentTopMenu;
        private set
        {
            if (_currentTopMenu != value)
            {
                _currentTopMenu = value;
                OnPropertyChanged(nameof(CurrentTopMenu));
            }
        }
    }

    public MainViewModel()
    {
        _activePanel = ActivePanel.Transcript;
    }

    private void UpdatePanels()
    {
        switch (_activePanel)
        {
            case ActivePanel.Transcript:
                CurrentCenterPanel = new TranscriptPanel();
                CurrentTopMenu = new TranscriptTopMenu();
                break;
            case ActivePanel.Timeline:
                CurrentCenterPanel = new TimelinePanel();
                CurrentTopMenu = new TimelineTopMenu();
                break;
            case ActivePanel.Outline:
                CurrentCenterPanel = new OutlinePanel();
                CurrentTopMenu = new OutlineTopMenu();
                break;
            case ActivePanel.CharacterBible:
                CurrentCenterPanel = new CharacterBiblePanel();
                CurrentTopMenu = new CharacterBibleTopMenu();
                break;
            case ActivePanel.LocationBible:
                CurrentCenterPanel = new LocationBiblePanel();
                CurrentTopMenu = new LocationBibleTopMenu();
                break;
            case ActivePanel.ItemBible:
                CurrentCenterPanel = new ItemBiblePanel();
                CurrentTopMenu = new ItemBibleTopMenu();
                break;
            case ActivePanel.LoreBible:
                CurrentCenterPanel = new LoreBiblePanel();
                CurrentTopMenu = new LoreBibleTopMenu();
                break;
        }
    }

    private void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
