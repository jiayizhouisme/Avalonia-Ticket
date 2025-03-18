using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using GetStartedApp.ViewModels;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
namespace GetStartedApp.Views
{
    public partial class ButtonPage : UserControl
    {
        private DispatcherTimer _timer;
        private int _currentIndex = 0;
        private IRegionManager _regionManager;
        private List<string> _imagePaths = new List<string>
        {
            "avares://GetStartedApp/Assets/2.jpg",
            "avares://GetStartedApp/Assets/4.jpg",
            "avares://GetStartedApp/Assets/7.jpg"
        };
        public ButtonPage(IContainerExtension container)
        {
            _regionManager = container.Resolve<IRegionManager>();

            InitializeComponent();
            DataContext = container.Resolve<ButtonViewModel>();

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

       
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (DataContext is ButtonViewModel viewModel)
            {
                viewModel.NextImage();
            }
        }
        public class TicketInfo
        {
            public Guid ExhibitionId { get; set; }
            public string Name { get; set; }
            public string ImagePath { get; set; }
            public decimal Price { get; set; }
            public string Description { get; set; }
        }    
    public void LocationIcon_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            _regionManager.RequestNavigate("Nav_HomeContent", "MapPage");
        }
        private void Ellipse_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (sender is Ellipse ellipse && ellipse.DataContext is ImageIndicator indicator)
            {
                if (DataContext is ButtonViewModel viewModel)
                {
                    int index = viewModel.ImageIndicators.IndexOf(indicator);
                    if (index != -1)
                    {
                        viewModel.SetCurrentImage(index);
                    }
                }
            }
        }
    }
}
