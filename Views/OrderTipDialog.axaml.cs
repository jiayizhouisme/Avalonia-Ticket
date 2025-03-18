using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GetStartedApp.Views;

public partial class OrderTipDialog : UserControl
{
    public OrderTipDialog()
    {
        InitializeComponent();
    }
    private void OnOk(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
      
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}