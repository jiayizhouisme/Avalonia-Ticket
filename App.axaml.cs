using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using GetStartedApp.ViewModels;
using GetStartedApp.Views;
using Avalonia.Logging;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Regions;
using DryIoc;
using GetStartedApp.Services;
using Xamarin.Forms;
using LayUI.Avalonia.Global;
using LayUI.Avalonia.Interfaces;
using LayUI.Avalonia;


namespace GetStartedApp
{
    public partial class App : PrismApplication
    {
        protected override AvaloniaObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            var regionManager = Container.Resolve<IRegionManager>();//
            regionManager.RequestNavigate("Nav_MainContent", "HomePage");//
        } 
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ILayDialogService, LayDialogService>();
            containerRegistry.RegisterSingleton<ILayMessage, LayMessage>();
            containerRegistry.Register<IBlogService, BlogService>();
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>(nameof(HomePage));
            containerRegistry.RegisterForNavigation<ButtonPage, ButtonViewModel>(nameof(ButtonPage));
            containerRegistry.RegisterForNavigation<TextBoxPage>(nameof(TextBoxPage));
            containerRegistry.RegisterForNavigation<LoginPage, LoginViewModel>(nameof(LoginPage));
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterViewModel>(nameof(RegisterPage));
            containerRegistry.RegisterForNavigation<MapPage>(nameof(MapPage));
            containerRegistry.RegisterForNavigation<DialogPage, DialogPageViewModel>(nameof(DialogPage));
            containerRegistry.RegisterForNavigation<Message, MessageViewModel>(nameof(Message));
            containerRegistry.RegisterForNavigation<VisitorNoticePage, VisitorNoticeViewModel>(nameof(VisitorNoticePage));
            containerRegistry.RegisterForNavigation<BookingPage, BookingViewModel>("BookingPage");
            containerRegistry.RegisterForNavigation<AddVisitorDialog, AddVisitorDialogViewModel>("AddVisitorDialog");
            containerRegistry.RegisterForNavigation<EditVisitorDialog, EditVisitorDialogViewModel>("EditVisitorDialog");
            containerRegistry.RegisterForNavigation<OrderTipDialog, OrderTipDialogViewModel>("OrderTip");
            containerRegistry.RegisterForNavigation<ConfirmationDialog, ConfirmationViewModel>("ConfirmationDialog");
          
            var layDialog = Container.Resolve<ILayDialogService>();
            layDialog.RegisterDialog<Message>(nameof(Message));
            containerRegistry.Register<ILayDialogService, LayDialogService>();
            layDialog.RegisterDialog<VisitorNoticePage>(nameof(VisitorNoticePage));
            layDialog.RegisterDialog<AddVisitorDialog>("AddVisitorDialog");
            layDialog.RegisterDialog<EditVisitorDialog>("EditVisitorDialog");
            layDialog.RegisterDialog<ConfirmationDialog>("ConfirmationDialog");

        }
    }
    }