using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GetStartedApp.Services;
using GetStartedApp.ViewModels;
using Prism.Navigation;
using Prism.Regions;
using System.Runtime.InteropServices.JavaScript;
namespace GetStartedApp
{
    public partial class LoginPage : UserControl
    {
        public LoginPage(IRegionManager regionManager)
        {
            InitializeComponent();
            var viewModel = new LoginViewModel(regionManager);
            this.DataContext = viewModel;
        }
    }
}
