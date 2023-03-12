using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Notepad.ViewModels;

namespace Notepad.Views;

public partial class FilesControl : UserControl
{
    public FilesControl()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    private void DoubleTap(object sender, RoutedEventArgs e) {
        var mwvm = (MainWindowViewModel?) DataContext;
        if (mwvm == null) return;

        var src = e.Source;
        if (src == null) return;

        var name = src.GetType().Name;
        if (name == "Image" || name == "ContentPresenter" || name == "TextBlock") mwvm.DoubleTap();
    }
}