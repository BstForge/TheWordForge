using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using System;
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
        var dialog = new SaveFileDialog
        {
            Filters = { new FileDialogFilter { Name = "Forge Project", Extensions = { "forge" } } },
            DefaultExtension = "forge"
        };
        var result = await dialog.ShowAsync(this);
        if (!string.IsNullOrEmpty(result) && DataContext is NewProjectViewModel vm)
        {
            vm.SaveLocation = result;
        }
    }

    private async void LoadExistingProject(object? sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            AllowMultiple = false,
            Filters = { new FileDialogFilter { Name = "Forge Project", Extensions = { "forge" } } }
        };
        var files = await dialog.ShowAsync(this);
        if (files != null && files.Length > 0)
        {
            await ProjectService.LoadProjectAsync(files[0]);
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

    private void CreateProject(object? sender, RoutedEventArgs e)
    {
        if (DataContext is NewProjectViewModel vm)
        {
            ProjectService.NewProject(vm.Title, vm.AuthorName, vm.Genre, vm.SaveLocation);
            Close();
        }
    }
}
