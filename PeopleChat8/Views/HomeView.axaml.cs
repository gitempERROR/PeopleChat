using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Avalonia.VisualTree;
using System.Collections.Specialized;
using System.Threading;

namespace PeopleChat8.Views;

public partial class HomeView : UserControl
{
    public HomeView()
    {
        InitializeComponent();

        if (listBox.Items is INotifyCollectionChanged observableCollection)
        {
            observableCollection.CollectionChanged += OnListBoxItemsChanged;
        }
    }

    private void OnListBoxItemsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        
        Dispatcher.UIThread.InvokeAsync(() => ScrollToEnd(listBox));
    }

    private async void ScrollToEnd(ListBox listBox)
    {
        var scrollViewer = listBox.FindDescendantOfType<ScrollViewer>();
        if (scrollViewer != null)
        {
            Thread.Sleep(200);
            scrollViewer.ScrollToEnd();
        }
    }
}