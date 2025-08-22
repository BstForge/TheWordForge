using System.ComponentModel;
using Avalonia.Controls;
using TheWordForge.menus.top;
using TheWordForge.panels;
using TheWordForge.panes;

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
    private UserControl _currentTopMenu = new TopMenu();
    private UserControl _currentLeftPane = new menus.side.SideMenu();
    private UserControl _currentRightPane = new RightPane();

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

    public UserControl CurrentRightPane
    {
        get => _currentRightPane;
        set
        {
            if (_currentRightPane != value)
            {
                _currentRightPane = value;
                OnPropertyChanged(nameof(CurrentRightPane));
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
                CurrentLeftPane = new menus.side.SideMenu();
                break;
            case ActivePanel.Timeline:
                CurrentCenterPanel = new TimelinePanel();
                CurrentLeftPane = new menus.side.SideMenu();
                break;
            case ActivePanel.Outline:
                CurrentCenterPanel = new OutlinePanel();
                CurrentLeftPane = new menus.side.SideMenu();
                break;
            case ActivePanel.CharacterBible:
                CurrentCenterPanel = new CharacterBiblePanel();
                CurrentLeftPane = new menus.side.SideMenu();
                break;
            case ActivePanel.LocationBible:
                CurrentCenterPanel = new LocationBiblePanel();
                CurrentLeftPane = new menus.side.SideMenu();
                break;
            case ActivePanel.ItemBible:
                CurrentCenterPanel = new ItemBiblePanel();
                CurrentLeftPane = new menus.side.SideMenu();
                break;
            case ActivePanel.LoreBible:
                CurrentCenterPanel = new LoreBiblePanel();
                CurrentLeftPane = new menus.side.SideMenu();
                break;
        }
    }

    private void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
