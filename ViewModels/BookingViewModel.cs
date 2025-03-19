using Avalonia.Automation.Provider;
using GetStartedApp.Controls;
using LayUI.Avalonia.Global;
using LayUI.Avalonia.Interfaces;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static GetStartedApp.ViewModels.BookingViewModel;
namespace GetStartedApp.ViewModels
{
    public class BookingViewModel : ViewModelBase, INavigationAware
    {
        private readonly DatabaseContext _dbContext;
        private Exhibition _exhibition;
        private string exhibitionid;
        private DateTime _selectedDate;
        private ObservableCollection<TimeSlot> _timeSlots;
        private int _selectedQuantity = 1;
        private ObservableCollection<UserInfos> _visitors;
        private ObservableCollection<DateOption> _dateOptions;
        private bool _isLoading;
        private readonly IRegionManager _regionManager;
        private string _attractionName;
        private UserInfos visitor;
        private readonly ILayDialogService _layDialogService;

        
        public ObservableCollection<DateOption> DateOptions
        {
            get => _dateOptions;
            set => SetProperty(ref _dateOptions, value);
        }

        public ObservableCollection<UserInfos> Visitors
        {
            get => _visitors;
            set => SetProperty(ref _visitors, value);
        }

        public string AttractionName
        {
            get => _attractionName;
            set => SetProperty(ref _attractionName, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        public ObservableCollection<TimeSlot> TimeSlots => _timeSlots;

        //public int SelectedQuantity
        //{
        //    get => _selectedQuantity;
        //    set
        //    {
        //        if (SetProperty(ref _selectedQuantity, Math.Clamp(value, 1, 5)))
        //        {
        //            OnPropertyChanged(nameof(RequiredVisitorsCount));
        //        }
        //    }
        //}
        public int SelectedQuantity
        {
            get => _selectedQuantity;
            set
            {
                if (SetProperty(ref _selectedQuantity, Math.Clamp(value, 1, 5)))
                {
                    int selectedCount = Visitors.Count(v => v.IsDetailsVisible);
                    if (selectedCount > _selectedQuantity)
                    {
                        foreach (var visitor in Visitors.Skip(_selectedQuantity))
                        {
                            visitor.IsDetailsVisible = false;
                        }
                        OnPropertyChanged(nameof(Visitors));
                    }
                    OnPropertyChanged(nameof(RequiredVisitorsCount));
                }
            }
        }
        public int RequiredVisitorsCount
        {
            get
            {
                
                return Math.Max(1, SelectedQuantity);
            }
        }
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    
    public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand IncrementCommand { get; private set; }//+
        public DelegateCommand DecrementCommand { get; private set; }//-
        public DelegateCommand<string> AddVisitorCommand { get; private set; }
        public DelegateCommand<UserInfos> RemoveVisitorCommand { get; private set; }
        public DelegateCommand SubmitOrderCommand { get; private set; }
        public DelegateCommand<DateOption> SelectDateCommand { get; private set; }
        public DelegateCommand<UserInfos> ShowVisitorDetailsCommand { get; private set; }
        public DelegateCommand<TimeSlot> SelectTimeSlotCommand { get; private set; }
        public DelegateCommand<UserInfos> EditVisitorCommand { get; private set; }
        public BookingViewModel(IContainerExtension container, ILayDialogService layDialog) : base(container)
        {
            _dbContext = container.Resolve<DatabaseContext>();
            _selectedDate = DateTime.Today;
            _layDialogService = container.Resolve<ILayDialogService>();
            _timeSlots = new ObservableCollection<TimeSlot>();
            _visitors = new ObservableCollection<UserInfos>();
            DateOptions = new ObservableCollection<DateOption>();
            _regionManager = container.Resolve<IRegionManager>();
            this._layDialogService = layDialog;
            InitializeCommands();
        }
        private void InitializeCommands()
        {
            IncrementCommand = new DelegateCommand(() => SelectedQuantity = Math.Min(5, SelectedQuantity + 1));//+
            DecrementCommand = new DelegateCommand(() => SelectedQuantity = Math.Max(1, SelectedQuantity - 1));//-
            RemoveVisitorCommand = new DelegateCommand<UserInfos>(RemoveVisitor);
            SubmitOrderCommand = new DelegateCommand(SubmitOrder);
            SelectDateCommand = new DelegateCommand<DateOption>(SelectDate);
            SelectTimeSlotCommand = new DelegateCommand<TimeSlot>(SelectTimeSlot);
            GoBackCommand = new DelegateCommand(GoBack);
            ShowVisitorDetailsCommand = new DelegateCommand<UserInfos>(ShowVisitorDetails);
            AddVisitorCommand = new DelegateCommand<string>(AddVisitor);
            EditVisitorCommand = new DelegateCommand<UserInfos>(EditVisitor);
        }

        private void EditVisitor(UserInfos visitor)
        {
            //if (visitor == null)
            //{
            //    System.Diagnostics.Debug.WriteLine("尝试编辑的游客对象为空，取消操作。");
            //    return;
            //}
           
                var parameters = new LayDialogParameter();
            parameters.Add("Visitor", visitor);
            parameters.Add("OnVisitorUpdated", new Action<UserInfos>(UpdateVisitorInfo));

            this._layDialogService.Show("EditVisitorDialog", parameters, res =>
            {
                if (res.Result == LayUI.Avalonia.Enums.ButtonResult.Yes)
                {
                    var updatedVisitor = res.Parameters.GetValue<UserInfos>("Visitor");
                    if (updatedVisitor != null)
                    {
                        UpdateVisitorInfo(updatedVisitor);
                    }
                }
            }, "VisitPageDialog");
        }

        private void UpdateVisitorInfo(UserInfos updatedVisitor)
        {
            var existingVisitor = Visitors.FirstOrDefault(v => v.Id == updatedVisitor.Id);
            if (existingVisitor != null)
            {
                existingVisitor.Name = updatedVisitor.Name;
                existingVisitor.PhoneNumber = updatedVisitor.PhoneNumber;
                existingVisitor.IdCard = updatedVisitor.IdCard;
                OnPropertyChanged(nameof(RequiredVisitorsCount));
                System.Diagnostics.Debug.WriteLine($"游客信息更新成功: {existingVisitor.Name}");
            }
        }
        private void AddVisitor(string messageRootName)
        {
            OnPropertyChanged(nameof(RequiredVisitorsCount));
            if (Visitors.Count < SelectedQuantity)
            {
                this._layDialogService.Show("AddVisitorDialog", null, res =>
                {
                    if (res.Result == LayUI.Avalonia.Enums.ButtonResult.Yes)
                    {
                        var addedVisitor = res.Parameters.GetValue<UserInfos>("Visitor");
                        if (addedVisitor != null)
                        {
                            Visitors.Add(addedVisitor);
                            OnPropertyChanged(nameof(RequiredVisitorsCount));
                            _dbContext.UserInfos.Add(addedVisitor);
                            try
                            {
                                _dbContext.SaveChanges();
                            }
                            catch (DbUpdateException ex)
                            {
                                Debug.WriteLine($"添加游客失败: {ex.Message}");
                            }
                        }
                    }
                }, messageRootName);
            }
        }

        //            if (res.Parameters!=null)
        //            {
        //                Visitors.Add(res.Parameters.GetValue<UserInfos>("Visitor"));
        //                OnPropertyChanged(nameof(RequiredVisitorsCount));
        //            }

        //        }, messageRootName);
        //    }
        //}
        private void RemoveVisitor(UserInfos visitor)
        {
            if (Visitors.Remove(visitor))
            {
                OnPropertyChanged(nameof(RequiredVisitorsCount));
            }
            if (visitor == null) return;
            try
            {
                _dbContext.UserInfos.Remove(visitor);
                _dbContext.SaveChanges();
                LoadVisitorsFromDatabase();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"删除游客失败: {ex.Message}");
            }
        }
        private async void ShowVisitorDetails(UserInfos visitor)
        {
            int selectedCount = Visitors.Count(v => v.IsDetailsVisible);
            if (!visitor.IsDetailsVisible && selectedCount >= SelectedQuantity)
            {
                await ShowMessageBoxAsync("提示", "参观人数已经超过了票数，请增加票数");
                
                return;
            }
            visitor.IsDetailsVisible = !visitor.IsDetailsVisible;
            OnPropertyChanged(nameof(Visitors));
        }

        private async Task LoadAvailableDatesAsync()
        {
            IsLoading = true;
            try
            {
                using (var dbContext = new DatabaseContext())
                {
                    var today = DateTime.Today;
                    int bookingDays = _exhibition.beforeDays.Value;
                    var maxDate = today.AddDays(bookingDays + 7);
                    var availableDates = _dbContext.Appointment
                       .Where(a => a.ObjectId == _exhibition.id &&
                                   a.StartTime.Date >= today &&
                                   a.EndTime.Date <= maxDate)
                       .Select(a => a.StartTime.Date)
                       .Distinct()
                       .OrderBy(d => d)
                       .ToList();

                    DateOptions.Clear();
                    Console.WriteLine($"加载到 {availableDates.Count} 个可用日期");
                    foreach (var date in availableDates)
                    {
                        DateOptions.Add(new DateOption
                        {
                            Date = date,
                            IsSelected = false,
                            IsEnabled = true
                        });
                    }

                    if (DateOptions.Any())
                    {
                        DateOptions.First().IsSelected = true;
                        SelectedDate = DateOptions.First().Date;
                        await LoadTimeSlotsAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载日期出错: {ex.Message}");
                OnPropertyChanged(nameof(DateOptions));
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadTimeSlotsAsync()
        {
            IsLoading = true;
            try
            {
                using (var dbContext = new DatabaseContext())
                {
                    var appointments = await dbContext.Appointment
                       .Where(a => a.ObjectId == _exhibition.id &&
                                   a.StartTime.Date == _selectedDate.Date)
                       .OrderBy(a => a.StartTime)
                       .Select(a => new { StartTime = a.StartTime, EndTime = a.EndTime, amount = a.amount })
                       .ToListAsync();
                    _timeSlots.Clear();
                    var displayedSlots = 0;
                    var maxTimeSlots = 3;
                    foreach (var appointment in appointments)
                    {
                        if (displayedSlots >= maxTimeSlots)
                            break;
                        var timeRange = $"{appointment.StartTime:HH:mm} - {appointment.EndTime:HH:mm}";
                        var existingSlot = _timeSlots.FirstOrDefault(t => t.TimeRange == timeRange);
                        if (existingSlot == null)
                        {
                            _timeSlots.Add(new TimeSlot
                            {
                                TimeRange = timeRange,
                                RemainingTickets = appointment.amount ?? 0,
                                Price = _exhibition.basicPrice,
                                IsSelected = false,
                                IsAvailable = appointment.amount >= 0
                            });
                            Console.WriteLine($"加载的时间范围: {timeRange}, 余票数量: {appointment.amount}");
                        }
                        displayedSlots++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载时间段出错: {ex.Message}");
                OnPropertyChanged(nameof(TimeSlots));
            }
            finally
            {
                IsLoading = false;
            }
        }
        private async Task ShowMessageBoxAsync(string title, string message, ButtonEnum buttons = ButtonEnum.Ok)
        {
            var box = MessageBoxManager.GetMessageBoxStandard(title, message, buttons);
            var result = await box.ShowAsync();
        }
        private async Task<string> ShowConfirmationDialogAsync(string title, string message)
        {
            var tcs = new TaskCompletionSource<string>();
            var parameters = new LayDialogParameter();
            parameters.Add("Title", title);
            parameters.Add("Message", message);
            parameters.Add("Button1Text", "去支付");
            parameters.Add("Button2Text", "取消");

            _layDialogService.Show("ConfirmationDialog", parameters, result =>
            {
                if (result.Result == LayUI.Avalonia.Enums.ButtonResult.Yes)
                {
                    tcs.SetResult(parameters.GetValue<string>("Button1Text"));
                }
                else if (result.Result == LayUI.Avalonia.Enums.ButtonResult.No)
                {
                    tcs.SetResult(parameters.GetValue<string>("Button2Text"));
                }
            }, null);
            return await tcs.Task;
        }
        private async void SubmitOrder()
        {
            var selectedTimeSlot = TimeSlots.FirstOrDefault(t => t.IsSelected);
            if (selectedTimeSlot == null)
            {
                await ShowMessageBoxAsync("提示", "请选择一个时间段");
                return;
            }
            if (SelectedQuantity <= 0)
            {
                await ShowMessageBoxAsync("提示", $"需选择至少1位游客");
                return;
            }

            var currentUserId = LoginViewModel.CurrentUserId;
            var selectedDate = SelectedDate.Date;
            var (startTime, endTime) = ParseTimeRange(selectedTimeSlot.TimeRange);

            try
            {
                using var dbContext = new DatabaseContext();
                var appointment = await dbContext.Appointment
                    .Include(a => a.Order)
                    .FirstOrDefaultAsync(a =>
                        a.ObjectId == _exhibition.id &&
                        a.StartTime.Date == selectedDate &&
                        a.StartTime == startTime &&
                        a.EndTime == endTime);
                if (appointment == null || appointment.amount < SelectedQuantity)
                {
                    await ShowMessageBoxAsync("预约失败", "余票不足或时间段已过期");
                    return;
                }
                if (appointment.Order.Any(o => o.UserId == currentUserId))
                {
                    await ShowMessageBoxAsync("预约失败", "您已预约过该时间段");
                    return;
                }
                var order = new Order
                {
                    Trade_No = long.Parse(GenerateTradeNo()),
                    UserId = currentUserId,
                    AppointmentId = appointment.Id,
                    ObjectId = _exhibition.id.ToString(),
                    Amount = selectedTimeSlot.Price * SelectedQuantity,
                    PayedAmount = 0,
                    Status = OrderStatus.未付款,
                    CreateTime = DateTime.Now,
                    expireDate = DateTime.Now, // 暂时
                    payedTime = DateTime.Now // 暂时
                };
                appointment.amount -= SelectedQuantity; 
                dbContext.Order.Add(order);
                await dbContext.SaveChangesAsync();
                var result = await ShowConfirmationDialogAsync("预约成功", $"已预约{SelectedQuantity}位游客，使用时间：{startTime:yyyy-MM-dd HH:mm}");

                if (result == "去支付")
                {
                    _regionManager.RequestNavigate("Nav_HomeContent", "PaymentPage");
                }
                else if (result == "取消")
                {
                    Debug.WriteLine("用户取消了操作");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"提交订单异常：{ex}");
                await ShowMessageBoxAsync("预约失败", $"操作失败：{ex.Message}");
            }
        }
        private (DateTime, DateTime) ParseTimeRange(string timeRange)
        {
            var parts = timeRange.Split('-').Select(p => p.Trim()).ToArray();
            var start = DateTime.Parse($"{SelectedDate:yyyy-MM-dd} {parts[0]}");
            var end = DateTime.Parse($"{SelectedDate:yyyy-MM-dd} {parts[1]}");
            return (start, end);
        }
        private string GenerateTradeNo()
        {
            var currentUserId = LoginViewModel.CurrentUserId;
            return $"{DateTime.Now:yyyyMMddHHmmssfff}{currentUserId}";
        }
        private async Task ShowDialog(string title, string message)
        {
            var parameters = new LayDialogParameter();
            parameters.Add("Title", title);
            parameters.Add("Message", message);
            var tcs = new TaskCompletionSource<bool>();
            _layDialogService.Show("OrderTip", parameters, result =>
            {
                tcs.SetResult(true);
            }, null);
            await tcs.Task;
        }
        private void GoBack()
        {
            _regionManager.RequestNavigate("Nav_HomeContent", "PreviousPage");
        }
        public class TimeSlot : INotifyPropertyChanged
        {
            private string _timeRange;
            private int _remainingTickets;
            private decimal _price;
            private bool _isSelected;
            private bool _isAvailable;
            public string TimeRange
            {
                get => _timeRange;
                set => SetField(ref _timeRange, value);
            }
            public int RemainingTickets
            {
                get => _remainingTickets;
                set => SetField(ref _remainingTickets, value);
            }

            public decimal Price
            {
                get => _price;
                set => SetField(ref _price, value);
            }
            public bool IsSelected
            {
                get => _isSelected;
                set => SetField(ref _isSelected, value);
            }
            public bool IsAvailable
            {
                get => _isAvailable;
                set => SetField(ref _isAvailable, value);
            }
            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
            {
                if (EqualityComparer<T>.Default.Equals(field, value)) return false;
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }
        }
        private async void SelectDate(DateOption dateOption)
        {
            if (dateOption == null || !dateOption.IsEnabled) return;
            Console.WriteLine($"开始选择日期: {dateOption.Date}");
            foreach (var option in DateOptions)
            {
                option.IsSelected = false;
            }
            dateOption.IsSelected = true;
            SelectedDate = dateOption.Date;
            Console.WriteLine($"选择日期: {SelectedDate}，开始加载时间段");
            await LoadTimeSlotsAsync();
            Console.WriteLine($"选择日期: {SelectedDate}，时间段加载完成");
        }
        private void SelectTimeSlot(TimeSlot timeSlot)
        {
            if (timeSlot == null || !timeSlot.IsAvailable) return;

            foreach (var slot in TimeSlots)
            {
                slot.IsSelected = false;
            }
            timeSlot.IsSelected = !timeSlot.IsSelected;
            Console.WriteLine($"选择时间段: {timeSlot.TimeRange}，当前选择状态: {timeSlot.IsSelected}");
            OnPropertyChanged(nameof(TimeSlots));
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _exhibition = navigationContext.Parameters.GetValue<Exhibition>("exhibition");
            AttractionName = _exhibition.name;
            LoadVisitorsFromDatabase();
            SelectedQuantity = 1;
            OnPropertyChanged(nameof(RequiredVisitorsCount));
            LoadAvailableDatesAsync().ConfigureAwait(false);
        }
        private void LoadVisitorsFromDatabase()
        {
            var currentUserId = LoginViewModel.CurrentUserId;
            var visitors = _dbContext.UserInfos
               .Where(u => u.UserId == currentUserId)
               .OrderByDescending(u => u.Id)
               .ToList();
            Visitors.Clear();
            foreach (var visitor in visitors)
            {
                Visitors.Add(visitor);
            }
            OnPropertyChanged(nameof(Visitors));
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public class DateOption : INotifyPropertyChanged
        {
            private DateTime _date;
            private bool _isSelected;
            private bool _isEnabled = true;

            public DateTime Date
            {
                get => _date;
                set => SetField(ref _date, value);
            }

            public bool IsSelected
            {
                get => _isSelected;
                set => SetField(ref _isSelected, value);
            }

            public bool IsEnabled
            {
                get => _isEnabled;
                set => SetField(ref _isEnabled, value);
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
            {
                if (EqualityComparer<T>.Default.Equals(field, value)) return false;
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }
        }
    }
}
