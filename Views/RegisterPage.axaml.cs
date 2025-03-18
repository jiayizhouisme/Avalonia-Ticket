using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GetStartedApp.Services;
using GetStartedApp.ViewModels;
using MsBox.Avalonia;
using Prism.Navigation;
using Prism.Regions;

namespace GetStartedApp
{
    public partial class RegisterPage : UserControl
    {
        public RegisterPage(IRegionManager regionManager)
        {
            InitializeComponent();
            var viewModel = new RegisterViewModel(regionManager);
            this.DataContext = viewModel;
        }
    }
}