using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace TheWordForge;

public partial class App : Application
{
    // Parameterless constructor required for Avalonia XAML loader
    public App()
    {
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
            desktop.Startup += (_, _) =>
            {
                var newProjectWindow = new NewProjectWindow();
                newProjectWindow.Show(desktop.MainWindow);
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
