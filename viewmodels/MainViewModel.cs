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
    private UserControl _currentLeftPane = new menus.side.SideMenu();

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

    public UserControl CurrentLeftPane
    {
        get => _currentLeftPane;
        private set
        {
            if (_currentLeftPane != value)
            {
                _currentLeftPane = value;
                OnPropertyChanged(nameof(CurrentLeftPane));
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
                CurrentLeftPane = new menus.side.SideMenu();
                break;
            case ActivePanel.Timeline:
                CurrentCenterPanel = new TimelinePanel();
                CurrentTopMenu = new TimelineTopMenu();
                CurrentLeftPane = new menus.side.SideMenu();
                break;
            case ActivePanel.Outline:
                CurrentCenterPanel = new OutlinePanel();
                CurrentTopMenu = new OutlineTopMenu();
                CurrentLeftPane = new menus.side.SideMenu();
                break;
            case ActivePanel.CharacterBible:
                CurrentCenterPanel = new CharacterBiblePanel();
                CurrentTopMenu = new CharacterBibleTopMenu();
                CurrentLeftPane = new menus.side.SideMenu();
                break;
            case ActivePanel.LocationBible:
                CurrentCenterPanel = new LocationBiblePanel();
                CurrentTopMenu = new LocationBibleTopMenu();
                CurrentLeftPane = new menus.side.SideMenu();
                break;
            case ActivePanel.ItemBible:
                CurrentCenterPanel = new ItemBiblePanel();
                CurrentTopMenu = new ItemBibleTopMenu();
                CurrentLeftPane = new menus.side.SideMenu();
                break;
            case ActivePanel.LoreBible:
                CurrentCenterPanel = new LoreBiblePanel();
                CurrentTopMenu = new LoreBibleTopMenu();
                CurrentLeftPane = new menus.side.SideMenu();
                break;
        }
    }

    private void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
