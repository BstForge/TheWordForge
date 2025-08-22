using Avalonia.Controls;
using Avalonia.Interactivity;

namespace TheWordForge;

public partial class PreferencesWindow : Window
{
    public PreferencesWindow()
    {
        InitializeComponent();
        DataContext = new PreferencesViewModel();
    }

    private void ClosePreferences(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
