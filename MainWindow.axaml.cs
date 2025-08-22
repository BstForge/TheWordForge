using Avalonia.Controls;
using Avalonia.Interactivity;
using TheWordForge.services;

namespace TheWordForge;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
        PreferencesService.TransitionChanged += (_, __) => ApplyTransitions();
        ApplyTransitions();
    }

    private void ApplyTransitions()
    {
        TopMenuContent.PageTransition = PreferencesService.BuildTransitionFor(TransitionRegion.Top);
        LeftPaneContent.PageTransition = PreferencesService.BuildTransitionFor(TransitionRegion.Left);
        CenterPanelContent.PageTransition = PreferencesService.BuildTransitionFor(TransitionRegion.Center);
    }

    private void OpenPreferences(object? sender, RoutedEventArgs e)
    {
        var win = new PreferencesWindow();
        win.ShowDialog(this);
    }
}
