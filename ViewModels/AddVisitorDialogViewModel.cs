using LayUI.Avalonia.Global;
using LayUI.Avalonia.Interfaces;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls; 
using System.Threading.Tasks;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Xamarin.Forms;

namespace GetStartedApp.ViewModels
{
    public class AddVisitorDialogViewModel : ViewModelBase, ILayDialogAware
    {
        private readonly IRegionManager _regionManager;
        private readonly ILayDialogService _layDialogService;
        private readonly DatabaseContext _dbContext;
        private Action<UserInfos> _onVisitorAddedCallback;
        private long? _userId;

        public AddVisitorDialogViewModel(IContainerExtension container)
            : base(container)
        {
            _layDialogService = container.Resolve<ILayDialogService>();
            _regionManager = container.Resolve<IRegionManager>();
            _dbContext = container.Resolve<DatabaseContext>();
            CancelCommand = new DelegateCommand(CloseDialog);
            ConfirmCommand = new DelegateCommand(AddVisitor);
        }

        public DelegateCommand CancelCommand { get; }
        public DelegateCommand ConfirmCommand { get; }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }

        private string _idCard;
        public string IdCard
        {
            get => _idCard;
            set => SetProperty(ref _idCard, value);
        }

        public event Action<ILayDialogResult> RequestClose;

        public bool isSingle => throw new NotImplementedException();

        private void CloseDialog()
        {
            RequestClose?.Invoke(new LayDialogResult(LayUI.Avalonia.Enums.ButtonResult.No));
            _regionManager.Regions["DialogRegion"].RemoveAll();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.TryGetValue<Action<UserInfos>>("OnVisitorAdded", out var callback))
            {
                _onVisitorAddedCallback = callback;
            }

            if (navigationContext.Parameters.TryGetValue<long>("UserId", out var userId))
            {
                _userId = userId;
            }
        }

        private async void AddVisitor()
        {
            Debug.WriteLine($"AddVisitor: Name={Name}, Phone={PhoneNumber}, IdCard={IdCard}");
            var newVisitor = new UserInfos
            {
                Name = Name,
                PhoneNumber = PhoneNumber,
                IdCard = IdCard,
                UserId = LoginViewModel.CurrentUserId
            };
            try
            {
                _dbContext.UserInfos.Add(newVisitor);
                await _dbContext.SaveChangesAsync();
                var box = MessageBoxManager.GetMessageBoxStandard("添加", "添加成功", ButtonEnum.Ok);
                var result = await box.ShowAsync();
                _onVisitorAddedCallback?.Invoke(newVisitor);
                 var par=new LayDialogParameter();
                par.Add("Visitor",newVisitor);
                RequestClose?.Invoke(new LayDialogResult(LayUI.Avalonia.Enums.ButtonResult.Yes,par));
                _regionManager.Regions["DialogRegion"].RemoveAll();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"保存游客信息失败: {ex.Message}");
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;
        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        public void OnOpened(ILayDialogParameter parameters)
        {

        }

        public void OnClosed()
        {

        }
    }
}