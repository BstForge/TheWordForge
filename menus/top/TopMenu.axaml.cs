using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using TheWordForge.panes;
using TheWordForge;

namespace TheWordForge.menus.top;

public partial class TopMenu : UserControl
{
    private MainViewModel? ViewModel => DataContext as MainViewModel;

    public TopMenu()
    {
        InitializeComponent();
        DataContextChanged += (_, __) => Attach();
    }

    private void Attach()
    {
        if (ViewModel != null)
        {
            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
            UpdateState();
        }
    }

    private void ViewModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainViewModel.ActivePanel))
            UpdateState();
    }

    private void UpdateState()
    {
        if (ViewModel == null) return;
        var active = ViewModel.ActivePanel;
        TimelineButton.IsEnabled = active != ActivePanel.Timeline;
        OutlineButton.IsEnabled = active != ActivePanel.Outline;
        CharacterBibleButton.IsEnabled = active != ActivePanel.CharacterBible;
        LocationBibleButton.IsEnabled = active != ActivePanel.LocationBible;
        ItemBibleButton.IsEnabled = active != ActivePanel.ItemBible;
        LoreBibleButton.IsEnabled = active != ActivePanel.LoreBible;
        CustomizeButton.Content = $"Customize {GetPanelName(active)}";
    }

    private static string GetPanelName(ActivePanel panel) => panel switch
    {
        ActivePanel.Transcript => "Transcript",
        ActivePanel.Timeline => "Timeline",
        ActivePanel.Outline => "Outline",
        ActivePanel.CharacterBible => "Character Bible",
        ActivePanel.LocationBible => "Location Bible",
        ActivePanel.ItemBible => "Item Bible",
        ActivePanel.LoreBible => "Lore Bible",
        _ => panel.ToString()
    };

    private void OpenTimeline(object? sender, RoutedEventArgs e)
    {
        if (ViewModel != null)
            ViewModel.CurrentRightPane = new TimelineRightPane();
    }

    private void OpenOutline(object? sender, RoutedEventArgs e)
    {
        if (ViewModel != null)
            ViewModel.CurrentRightPane = new OutlineRightPane();
    }

    private void OpenCharacterBible(object? sender, RoutedEventArgs e)
    {
        if (ViewModel != null)
            ViewModel.CurrentRightPane = new CharacterBibleRightPane();
    }

    private void OpenLocationBible(object? sender, RoutedEventArgs e)
    {
        if (ViewModel != null)
            ViewModel.CurrentRightPane = new LocationBibleRightPane();
    }

    private void OpenItemBible(object? sender, RoutedEventArgs e)
    {
        if (ViewModel != null)
            ViewModel.CurrentRightPane = new ItemBibleRightPane();
    }

    private void OpenLoreBible(object? sender, RoutedEventArgs e)
    {
        if (ViewModel != null)
            ViewModel.CurrentRightPane = new LoreBibleRightPane();
    }

    private void OpenCustomize(object? sender, RoutedEventArgs e)
    {
        if (ViewModel == null) return;
        var title = $"{GetPanelName(ViewModel.ActivePanel)} Customization";
        var win = new CustomizationWindow(title);
        win.Show();
    }
}

