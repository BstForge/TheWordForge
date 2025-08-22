using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using System;
using System.IO;
using TheWordForge.services;

namespace TheWordForge;

public partial class NewProjectWindow : Window
{
    public NewProjectWindow()
    {
        InitializeComponent();
        DataContext = new NewProjectViewModel();
    }

    private async void BrowseSaveLocation(object? sender, RoutedEventArgs e)
    {
        var folders = await StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions());
        if (folders.Count > 0 && DataContext is NewProjectViewModel vm)
        {
            vm.SaveLocation = folders[0].Path.LocalPath;
        }
    }

    private async void LoadExistingProject(object? sender, RoutedEventArgs e)
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
        {
            await ProjectService.LoadProjectAsync(files[0].Path.LocalPath);
            Close();
        }
    }

    private void Cancel(object? sender, RoutedEventArgs e)
    {
        if (ProjectService.CurrentProject == null)
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Shutdown();
            }
            else
            {
                Environment.Exit(0);
            }
        }
        else
        {
            Close();
        }
    }

    private async void CreateProject(object? sender, RoutedEventArgs e)
    {
        if (DataContext is NewProjectViewModel vm)
        {
            var filePath = Path.Combine(vm.SaveLocation, $"{vm.Title}.forge");
            ProjectService.NewProject(vm.Title, vm.AuthorName, vm.Genre, filePath);
            await ProjectService.SaveProjectAsync();
            Close();
        }
    }
}
