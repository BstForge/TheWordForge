using Avalonia.Controls;
using Avalonia.Interactivity;
using TheWordForge.services;
using System.Threading.Tasks;

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
        var dialog = new OpenFileDialog
        {
            AllowMultiple = false,
            Filters = { new FileDialogFilter { Name = "Forge Project", Extensions = { "forge" } } }
        };
        var files = await dialog.ShowAsync(this);
        if (files != null && files.Length > 0)
            await ProjectService.LoadProjectAsync(files[0]);
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
