using Avalonia.Controls.Selection;
using Avalonia.Media.Imaging;
using GetStartedApp.Helpers;
using GetStartedApp.Views;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        private Bitmap? imageFromBinding;
        public Bitmap? ImageFromBinding { get { return imageFromBinding; } set { SetProperty(ref imageFromBinding,value); }   }
        private Bitmap? imageFromBinding2;
        public Bitmap? ImageFromBinding2
        {
            get { return imageFromBinding2; }
            set { SetProperty(ref imageFromBinding2, value); }
        }
        public HomePageViewModel(IContainerExtension container) : base(container)
        {
            Menus = CreateMenus();
            RegionManager.RegisterViewWithRegion("Nav_HomeContent", nameof(ButtonPage));
        }
        private MenuInfo MenuInfo { get; set; } 
        private ObservableCollection<MenuInfo> _Menus;
        public ObservableCollection<MenuInfo> Menus
        {
            get { return _Menus; }
            set { SetProperty(ref _Menus, value); }
        }

        private DelegateCommand<MenuInfo> _GoPageCommand;
        public DelegateCommand<MenuInfo> GoPageCommand =>
            _GoPageCommand ?? (_GoPageCommand = new DelegateCommand<MenuInfo>(ExecuteGoPageCommand));

        void ExecuteGoPageCommand(MenuInfo info)
        {
            MenuInfo = info;

            if (info.Title == "首页")
            {
                ImageFromBinding = ImageHelper.LoadFromResource(new Uri("avares://GetStartedApp/Assets/12.png"));
                ImageFromBinding2 = ImageHelper.LoadFromResource(new Uri("avares://GetStartedApp/Assets/10.png"));
            }
           
            else
            {
                ImageFromBinding = ImageHelper.LoadFromResource(new Uri("avares://GetStartedApp/Assets/01.png"));
                ImageFromBinding2 = ImageHelper.LoadFromResource(new Uri("avares://GetStartedApp/Assets/13.png"));
            }
            foreach (var menu in Menus)
            {
                menu.IsSelected = menu == info;
            }
            RegionManager.RequestNavigate("Nav_HomeContent", info.PageKey);
        }

        private ObservableCollection<MenuInfo> CreateMenus()
        {
            ObservableCollection<MenuInfo> menus = new ObservableCollection<MenuInfo>
            {
                new MenuInfo() { FontIcon = "E800", PageKey = "ButtonPage", Title = "首页", IsSelected = true,
                    DefaultImage = "avares://GetStartedApp/Assets/01.png", SelectedImage = "avares://GetStartedApp/Assets/12.png" },
                new MenuInfo() { FontIcon = "E801", PageKey = "TextBoxPage", Title = "个人中心", IsSelected = false,
                    DefaultImage = "avares://GetStartedApp/Assets/10.png", SelectedImage = "avares://GetStartedApp/Assets/13.png" }
            };
            MenuInfo = menus[0];
            ImageFromBinding = ImageHelper.LoadFromResource(new Uri("avares://GetStartedApp/Assets/12.png"));
            ImageFromBinding2 = ImageHelper.LoadFromResource(new Uri("avares://GetStartedApp/Assets/10.png"));
            return menus;
        }
    }
}
