using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.VisualTree;
using TheWordForge;
using TheWordForge.models;

namespace TheWordForge.panels;

public partial class TranscriptPanel : UserControl
{
    private object? _dragItem;
    private object? _selectedItem;
    private bool _suppressText;

    private readonly TextBox _textBox;
    private readonly TextBlock _wordText;
    private readonly TextBlock _charText;
    private readonly TreeView _tree;

    public TranscriptPanel()
    {
        InitializeComponent();
        DataContext = new TranscriptPanelViewModel();

        _textBox = this.FindControl<TextBox>("TranscriptTextBox")!;
        var scopeBox = this.FindControl<ComboBox>("ScopeComboBox")!;
        _wordText = this.FindControl<TextBlock>("WordCountText")!;
        _charText = this.FindControl<TextBlock>("CharacterCountText")!;
        _tree = this.FindControl<TreeView>("ChapterTree")!;

        _textBox.TextChanged += TextBoxOnTextChanged;
        scopeBox.SelectionChanged += (_, __) => UpdateCounts();

        _tree.AddHandler(DragDrop.DropEvent, TreeViewDrop);
        _tree.AddHandler(DragDrop.DragOverEvent, TreeViewDragOver);
        _tree.PointerPressed += TreeViewPointerPressed;
        _tree.SelectionChanged += TreeSelectionChanged;

        UpdateCounts();
    }

    private void UpdateCounts()
    {
        var text = _textBox.Text ?? string.Empty;
        var wordCount = string.IsNullOrWhiteSpace(text) ? 0 :
            text.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
        var charCount = text.Length;
        _wordText.Text = $"Words: {wordCount}";
        _charText.Text = $"Chars: {charCount}";
    }

    private void AddChapter(object? sender, RoutedEventArgs e)
    {
        if (DataContext is TranscriptPanelViewModel vm)
        {
            var chapter = new Chapter { Title = $"Chapter {vm.Chapters.Count + 1}" };
            chapter.Scenes.Add(new Scene { Title = "Scene 1" });
            vm.Chapters.Add(chapter);
        }
    }

    private void TreeSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        _selectedItem = _tree.SelectedItem;
        _suppressText = true;
        if (_selectedItem is Scene scene)
        {
            _textBox.Text = scene.Text;
        }
        else if (_selectedItem is Chapter chapter)
        {
            _textBox.Text = string.Join("\n***\n", chapter.Scenes.Select(s => s.Text));
        }
        else
        {
            _textBox.Text = string.Empty;
        }
        _suppressText = false;
        UpdateCounts();
    }

    private void TextBoxOnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (_suppressText)
        {
            UpdateCounts();
            return;
        }

        if (_selectedItem is Scene scene)
        {
            scene.Text = _textBox.Text ?? string.Empty;
        }
        else if (_selectedItem is Chapter chapter)
        {
            var parts = (_textBox.Text ?? string.Empty).Split(new[] { "\n***\n" }, StringSplitOptions.None);
            for (int i = 0; i < chapter.Scenes.Count; i++)
            {
                chapter.Scenes[i].Text = i < parts.Length ? parts[i].Trim() : string.Empty;
            }
        }

        UpdateCounts();
    }

    private void AddScene(object? sender, RoutedEventArgs e)
    {
        if ((sender as MenuItem)?.DataContext is Chapter chapter)
        {
            chapter.Scenes.Add(new Scene { Title = $"Scene {chapter.Scenes.Count + 1}" });
        }
    }

    private async void RenameChapter(object? sender, RoutedEventArgs e)
    {
        if ((sender as MenuItem)?.DataContext is Chapter chapter)
        {
            var result = await Prompt("Rename Chapter", chapter.Title);
            if (!string.IsNullOrWhiteSpace(result))
                chapter.Title = result;
        }
    }

    private async void DeleteChapter(object? sender, RoutedEventArgs e)
    {
        if ((sender as MenuItem)?.DataContext is Chapter chapter && DataContext is TranscriptPanelViewModel vm)
        {
            if (await Confirm($"Delete '{chapter.Title}' and all its scenes?"))
            {
                vm.Chapters.Remove(chapter);
                if (Equals(_selectedItem, chapter))
                {
                    _selectedItem = null;
                    _textBox.Text = string.Empty;
                    UpdateCounts();
                }
            }
        }
    }

    private async void RenameScene(object? sender, RoutedEventArgs e)
    {
        if ((sender as MenuItem)?.DataContext is Scene scene)
        {
            var result = await Prompt("Rename Scene", scene.Title);
            if (!string.IsNullOrWhiteSpace(result))
                scene.Title = result;
        }
    }

    private async void DeleteScene(object? sender, RoutedEventArgs e)
    {
        if ((sender as MenuItem)?.DataContext is Scene scene && DataContext is TranscriptPanelViewModel vm)
        {
            if (await Confirm($"Delete scene '{scene.Title}'?"))
            {
                foreach (var ch in vm.Chapters)
                {
                    if (ch.Scenes.Remove(scene))
                        break;
                }

                if (Equals(_selectedItem, scene))
                {
                    _selectedItem = null;
                    _textBox.Text = string.Empty;
                    UpdateCounts();
                }
            }
        }
    }

    private async Task<string?> Prompt(string title, string initial)
    {
        var window = new Window
        {
            Title = title,
            Width = 300,
            Height = 150,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };
        var text = new TextBox { Text = initial, Margin = new Thickness(10) };
        var ok = new Button { Content = "OK", IsDefault = true, Margin = new Thickness(5) };
        var cancel = new Button { Content = "Cancel", IsCancel = true, Margin = new Thickness(5) };
        var buttons = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center };
        buttons.Children.Add(ok);
        buttons.Children.Add(cancel);
        var stack = new StackPanel();
        stack.Children.Add(text);
        stack.Children.Add(buttons);
        window.Content = stack;

        string? result = null;
        ok.Click += (_, __) => { result = text.Text; window.Close(); };
        cancel.Click += (_, __) => { window.Close(); };

        await window.ShowDialog((Window)VisualRoot!);
        return result;
    }

    private async Task<bool> Confirm(string message)
    {
        var window = new Window
        {
            Title = "Confirm",
            Width = 300,
            Height = 150,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };
        var txt = new TextBlock { Text = message, Margin = new Thickness(10), TextWrapping = TextWrapping.Wrap };
        var yes = new Button { Content = "Yes", Margin = new Thickness(5) };
        var no = new Button { Content = "No", Margin = new Thickness(5) };
        var buttons = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center };
        buttons.Children.Add(yes);
        buttons.Children.Add(no);
        var stack = new StackPanel();
        stack.Children.Add(txt);
        stack.Children.Add(buttons);
        window.Content = stack;

        var tcs = new TaskCompletionSource<bool>();
        yes.Click += (_, __) => { tcs.SetResult(true); window.Close(); };
        no.Click += (_, __) => { tcs.SetResult(false); window.Close(); };

        await window.ShowDialog((Window)VisualRoot!);
        return await tcs.Task;
    }

    private async void TreeViewPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var item = (e.Source as Control)?.FindAncestorOfType<TreeViewItem>();
        if (item == null)
            return;

        _dragItem = item.DataContext;
        if (_dragItem != null)
        {
            var data = new DataObject();
            data.Set("application/x-treeitem", _dragItem);
            await DragDrop.DoDragDrop(e, data, DragDropEffects.Move);
        }
    }

    private void TreeViewDragOver(object? sender, DragEventArgs e)
    {
        if (_dragItem == null)
            return;

        var targetItem = (e.Source as Control)?.FindAncestorOfType<TreeViewItem>()?.DataContext;
        e.DragEffects = DragDropEffects.None;

        if (_dragItem is Chapter && targetItem is Chapter)
        {
            e.DragEffects = DragDropEffects.Move;
        }
        else if (_dragItem is Scene && (targetItem is Scene || targetItem is Chapter))
        {
            e.DragEffects = DragDropEffects.Move;
        }

        e.Handled = true;
    }

    private void TreeViewDrop(object? sender, DragEventArgs e)
    {
        if (_dragItem == null || DataContext is not TranscriptPanelViewModel vm)
            return;

        var targetItem = (e.Source as Control)?.FindAncestorOfType<TreeViewItem>()?.DataContext;
        if (targetItem == null || ReferenceEquals(targetItem, _dragItem))
            return;

        if (_dragItem is Chapter draggedChapter && targetItem is Chapter targetChapterObj)
        {
            var chapters = vm.Chapters;
            var oldIndex = chapters.IndexOf(draggedChapter);
            var newIndex = chapters.IndexOf(targetChapterObj);
            if (oldIndex >= 0 && newIndex >= 0)
            {
                chapters.Move(oldIndex, newIndex);
            }
        }
        else if (_dragItem is Chapter draggedCh && targetItem is Scene targetSceneObj)
        {
            var chapters = vm.Chapters;
            var oldIndex = chapters.IndexOf(draggedCh);
            var containingChapter = vm.Chapters.First(ch => ch.Scenes.Contains(targetSceneObj));
            var newIndex = chapters.IndexOf(containingChapter);
            if (oldIndex >= 0 && newIndex >= 0)
            {
                chapters.Move(oldIndex, newIndex);
            }
        }
        else if (_dragItem is Scene draggedScene)
        {
            Chapter sourceChapter = vm.Chapters.First(ch => ch.Scenes.Contains(draggedScene));
            Chapter destChapter;
            int insertIndex;

            if (targetItem is Scene targetSceneItem)
            {
                destChapter = vm.Chapters.First(ch => ch.Scenes.Contains(targetSceneItem));
                insertIndex = destChapter.Scenes.IndexOf(targetSceneItem);
            }
            else if (targetItem is Chapter tc)
            {
                destChapter = tc;
                insertIndex = destChapter.Scenes.Count;
            }
            else return;

            if (sourceChapter == destChapter)
            {
                var oldIndex = sourceChapter.Scenes.IndexOf(draggedScene);
                if (oldIndex >= 0)
                {
                    if (insertIndex > oldIndex) insertIndex--;
                    sourceChapter.Scenes.Move(oldIndex, insertIndex);
                }
            }
            else
            {
                sourceChapter.Scenes.Remove(draggedScene);
                destChapter.Scenes.Insert(insertIndex, draggedScene);
            }
        }

        _dragItem = null;
    }
}
