using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
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
        TopMenuContent.PageTransition = PreferencesService.BuildTransition();
        LeftPaneContent.PageTransition = PreferencesService.BuildTransition();
        CenterPanelContent.PageTransition = PreferencesService.BuildTransition();
        RightPaneContent.PageTransition = PreferencesService.BuildTransition();
    }

    private void OpenPreferences(object? sender, RoutedEventArgs e)
    {
        var win = new PreferencesWindow();
        win.ShowDialog(this);
    }

    private void OpenNewProject(object? sender, RoutedEventArgs e)
    {
        var win = new NewProjectWindow();
        win.ShowDialog(this);
    }

    private async void LoadProject(object? sender, RoutedEventArgs e)
    {
        var options = new FilePickerOpenOptions
        {
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("Forge Project") { Patterns = new[] { "*.forge" } }
            }
        };
        var files = await StorageProvider.OpenFilePickerAsync(options);
        if (files.Count > 0)
            await ProjectService.LoadProjectAsync(files[0].Path.LocalPath);
    }

    private async void SaveProject(object? sender, RoutedEventArgs e)
    {
        await ProjectService.SaveProjectAsync();
    }

    private void OpenHamburgerMenu(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.ContextMenu is { } menu)
        {
            menu.PlacementTarget = button;
            menu.Open();
        }
    }

    private void ExitApplication(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
