using Avalonia.Controls;

namespace TheWordForge;

public partial class PreferencesWindow : Window
{
    public PreferencesWindow()
    {
        InitializeComponent();
        DataContext = new PreferencesViewModel();
    }
}
