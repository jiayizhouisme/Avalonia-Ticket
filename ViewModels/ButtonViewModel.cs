using System;
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia.Input;
using System.Diagnostics;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Ioc;
using CommunityToolkit.Mvvm.Input;
using LayUI.Avalonia.Global;
using Prism.Commands;
using LayUI.Avalonia.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.ObjectModel;

namespace GetStartedApp.ViewModels
{
    public partial class ButtonViewModel : ViewModelBase
    {
        private readonly ButtonViewModel DataContext;
        private readonly DatabaseContext _dbContext;
        private readonly IRegionManager _regionManager;
        private ILayDialogService layDialog;
        private Bitmap _currentImage;
        private readonly string[] _imagePaths = {
            "avares://GetStartedApp/Assets/2.jpg",
            "avares://GetStartedApp/Assets/4.jpg",
            "avares://GetStartedApp/Assets/7.jpg"
        };
        private int _currentImageIndex = 0;
        public Bitmap TicketImage
        {
            get => _ticketImage;
            set => SetProperty(ref _ticketImage, value);
        }
        public Bitmap CurrentImage
        {
            get => _currentImage;
            set => SetProperty(ref _currentImage, value);
        }
        public List<ImageIndicator> ImageIndicators { get; set; } = new List<ImageIndicator>();
        public ObservableCollection<Exhibition> Exhibition { get; set; } = new ObservableCollection<Exhibition>();
        public ButtonViewModel(IContainerExtension container) : base(container)
        {
            _regionManager = container.Resolve<IRegionManager>();
            BookCommand = new DelegateCommand<Guid?>(OnBookCommandExecuted, CanBook);
            layDialog = container.Resolve<ILayDialogService>();
            _dbContext = container.Resolve<DatabaseContext>();
            InitializeIndicators();
            LoadImage(_imagePaths[_currentImageIndex]);
            LoadExhibitions();//
        }
        //private async void LoadExhibitions()//
        //{
        //    try
        //    {
        //        var exhibitions = await _dbContext.Exhibition.Take(3).ToListAsync(); 
        //        Exhibitions.Clear();
        //        foreach (var exhibition in exhibitions)
        //        {
        //            Exhibitions.Add(exhibition);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"加载景区数据失败: {ex.Message}");
        //    }
        //}
        private async void LoadExhibitions()
        {
            try
            {
                var exhibitions = await _dbContext.Exhibition
                    .Where(e => e.id != Guid.Empty)
                    .ToListAsync();

                Console.WriteLine($"数据库返回数据条数: {exhibitions.Count}");

                Exhibition.Clear();
                foreach (var exhibition in exhibitions)
                {
                    Console.WriteLine($"加载景区: {exhibition.name}, ID: {exhibition.id}");
                    Exhibition.Add(exhibition);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载数据失败: {ex.Message}");
            }
        }
        //private async void LoadExhibitions()
        //{
        //    try
        //    {
        //        var exhibitions = await _dbContext.Exhibition.ToListAsync(); 
        //        Exhibitions.Clear();
        //        foreach (var exhibition in exhibitions)
        //        {
        //            Exhibitions.Add(exhibition);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"加载景区数据失败: {ex.Message}");
        //    }
        //}
        // public ObservableCollection<Exhibition> Exhibition { get; set; } = new ObservableCollection<Exhibition>();
        public DelegateCommand<Guid?> BookCommand {  get; private set; }
        //   public DelegateCommand<string> BookCommand =>
        //_BookCommand ?? (_BookCommand = new DelegateCommand<string>(OnBookCommandExecuted, CanBook));
        private bool CanBook(Guid? exhibitionId)
        {
            if (exhibitionId == Guid.Empty)
            {
                Console.WriteLine("景区ID为空，预订按钮不可用。");
                return false;
            }

            var idCache = Exhibition.ToDictionary(e => e.id);
            bool isValid = idCache.ContainsKey((Guid)exhibitionId);

            if (!isValid)
            {
                Console.WriteLine($"未找到匹配的景区ID: {exhibitionId}");
            }

            return isValid;
        }
        //    private void OnBookCommandExecuted(string par)
        //    {
        //        NavigationParameters navigationParameters;
        //        var parameters = new NavigationParameters
        //{
        //    { "exhibition", par }, 

        //};
        //        _regionManager.RequestNavigate("Nav_HomeContent", "BookingPage", parameters);
        //    }
        private void OnBookCommandExecuted(Guid? exhibitionId)
        {
            var exhibition = _dbContext.Exhibition.FirstOrDefault(e => e.id == exhibitionId);
            if (exhibition == null)
            {
                Console.WriteLine($"未找到对应的景区信息: {exhibitionId}");
                return;
            }

            var parameters = new NavigationParameters { { "exhibition", exhibition } };
            _regionManager.RequestNavigate("Nav_HomeContent", "BookingPage", parameters);
        }
        //private void OnBookCommandExecuted(string exhibitionId)
        //{
        //    var exhibition = _dbContext.Exhibition.FirstOrDefault(e => e.id == Guid.Parse(exhibitionId));//只查询第一个id
        //    if (exhibition != null)
        //    {
        //        var parameters = new NavigationParameters
        //           {
        //               { "exhibition", exhibition }
        //           };
        //        _regionManager.RequestNavigate("Nav_HomeContent", "BookingPage", parameters);
        //    }
        //    else
        //    {
        //        Console.WriteLine("Exhibition not found.");
        //    }
        //}
        private DelegateCommand<string> _DlalogCommand;
        public DelegateCommand<string> DlalogCommand =>
            _DlalogCommand ?? (_DlalogCommand = new DelegateCommand<string>(ExecuteDlalogCommand));
        void ExecuteDlalogCommand(string messageRootName)
        {
            layDialog.Show("Message", null, res =>
            {
                var data = res;
            }, messageRootName);
        }
        private DelegateCommand<string> _DialogCommand;
        private object _exhibition;
        private Bitmap _ticketImage;
        public DelegateCommand<string> DialogCommand =>
            _DialogCommand ?? (_DialogCommand = new DelegateCommand<string>(ExecuteDialogCommand));

        void ExecuteDialogCommand(string messageRootName)
        {
            layDialog.Show("VisitorNoticePage", null, res =>
            {
                var data = res;
            }, messageRootName);
        }
        private void InitializeIndicators()
        {
            ImageIndicators.Clear();
            for (int i = 0; i < _imagePaths.Length; i++)
            {
                ImageIndicators.Add(new ImageIndicator
                {
                    Color = Brushes.Gray
                });
            }
            UpdateIndicators();
        }
        private void LoadTicketImage(string path)
        {
            try
            {
                using var stream = AssetLoader.Open(new Uri(path));
                TicketImage = new Bitmap(stream);
                Console.WriteLine($"加载门票图片成功: {path}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载门票图片失败: {ex.Message}");
            }
        }
        private void LoadImage(string path)
        {
            try
            {
                using var stream = AssetLoader.Open(new Uri(path));
                CurrentImage = new Bitmap(stream);
                Console.WriteLine($"加载图片成功: {path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载图片失败: {ex.Message}");
            }
        }
        public void NextImage()
        {
            _currentImageIndex = (_currentImageIndex + 1) % _imagePaths.Length;
            LoadImage(_imagePaths[_currentImageIndex]);
            UpdateIndicators();
        }
        public void PreviousImage()
        {
            _currentImageIndex = (_currentImageIndex - 1 + _imagePaths.Length) % _imagePaths.Length;
            LoadImage(_imagePaths[_currentImageIndex]);
            UpdateIndicators();
        }
        public void SetCurrentImage(int index)
        {
            if (index >= 0 && index < _imagePaths.Length)
            {
                _currentImageIndex = index;
                LoadImage(_imagePaths[_currentImageIndex]);
                UpdateIndicators();
            }
        }
        private void UpdateIndicators()
        {
            for (int i = 0; i < ImageIndicators.Count; i++)
            {
                ImageIndicators[i].Color = i == _currentImageIndex ? Brushes.White : Brushes.Gray;
            }
        }
    }
    public class ImageIndicator : INotifyPropertyChanged
    {
        private IBrush _color;
        public IBrush Color
        {
            get => _color;
            set
            {
                if (_color != value)
                {
                    _color = value;
                    OnPropertyChanged(nameof(Color));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}