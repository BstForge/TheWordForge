using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using TheWordForge;

namespace TheWordForge.menus.side;

public partial class SideMenu : UserControl
{
    public SideMenu()
    {
        InitializeComponent();
    }

    private void Button_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainViewModel vm && sender is Button btn && btn.Tag is string tag &&
            Enum.TryParse<ActivePanel>(tag, out var panel))
        {
            vm.ActivePanel = panel;
        }
    }
}
