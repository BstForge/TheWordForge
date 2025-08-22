using Avalonia.Controls;

namespace TheWordForge;

public partial class CustomizationWindow : Window
{
    public CustomizationWindow() : this("Customization")
    {
    }

    public CustomizationWindow(string title)
    {
        InitializeComponent();
        Title = title;
        TitleBlock.Text = title;
    }
}

