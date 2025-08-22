using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using TheWordForge;
using TheWordForge.models;

namespace TheWordForge.panels;

public partial class TranscriptPanel : UserControl
{
    private object? _dragItem;

    public TranscriptPanel()
    {
        InitializeComponent();
        DataContext = new TranscriptPanelViewModel();

        var textBox = this.FindControl<TextBox>("TranscriptTextBox")!;
        var scopeBox = this.FindControl<ComboBox>("ScopeComboBox")!;
        var wordText = this.FindControl<TextBlock>("WordCountText")!;
        var charText = this.FindControl<TextBlock>("CharacterCountText")!;
        var tree = this.FindControl<TreeView>("ChapterTree")!;

        textBox.TextChanged += (_, __) => UpdateCounts(textBox, wordText, charText);
        scopeBox.SelectionChanged += (_, __) => UpdateCounts(textBox, wordText, charText);

        tree.AddHandler(DragDrop.DropEvent, TreeViewDrop);
        tree.AddHandler(DragDrop.DragOverEvent, TreeViewDragOver);
        tree.PointerPressed += TreeViewPointerPressed;

        UpdateCounts(textBox, wordText, charText);
    }

    private void UpdateCounts(TextBox textBox, TextBlock wordText, TextBlock charText)
    {
        var text = textBox.Text ?? string.Empty;
        var wordCount = string.IsNullOrWhiteSpace(text) ? 0 :
            text.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
        var charCount = text.Length;
        wordText.Text = $"Words: {wordCount}";
        charText.Text = $"Chars: {charCount}";
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
