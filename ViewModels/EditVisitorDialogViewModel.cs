using LayUI.Avalonia.Global;
using LayUI.Avalonia.Interfaces;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.ViewModels
{
    public class EditVisitorDialogViewModel : ViewModelBase, ILayDialogAware
    {
        private readonly DatabaseContext _dbContext;
        private Action<UserInfos> _onVisitorUpdatedCallback;
        private UserInfos _visitor;
        private readonly IRegionManager _regionManager;
        private readonly ILayDialogService _layDialogService;
        public EditVisitorDialogViewModel(IContainerExtension container)
            : base(container)
        {
            _dbContext = container.Resolve<DatabaseContext>();
            _regionManager = container.Resolve<IRegionManager>();
            _layDialogService = container.Resolve<ILayDialogService>();
            CancelCommand = new DelegateCommand(CloseDialog);
            ConfirmCommand = new DelegateCommand(SaveVisitor);
        }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand ConfirmCommand { get; }

        public UserInfos Visitor
        {
            get => _visitor;
            set => SetProperty(ref _visitor, value);
        }

        public event Action<ILayDialogResult> RequestClose;

        public bool isSingle => false;

        private void CloseDialog()
        {
            RequestClose?.Invoke(new LayDialogResult(LayUI.Avalonia.Enums.ButtonResult.Cancel));
            _regionManager.Regions["DialogRegion"].RemoveAll();
        }

        public void OnOpened(ILayDialogParameter parameters)
        {
            if (parameters.TryGetValue("Visitor", out object visitorObj) && visitorObj is UserInfos visitor)
            {
                Visitor = visitor;
                System.Diagnostics.Debug.WriteLine($"Received visitor: {visitor.Name}");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Failed to receive visitor object.");
                Visitor = new UserInfos();
            }
            if (parameters.TryGetValue("OnVisitorUpdated", out object callbackObj) && callbackObj is Action<UserInfos> callback)
            {
                _onVisitorUpdatedCallback = callback;
                System.Diagnostics.Debug.WriteLine("Received OnVisitorUpdated callback.");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Failed to receive OnVisitorUpdated callback.");
            }
        }
        private async void SaveVisitor()
        {
            if (Visitor == null) return;
            try
            {
                var existingVisitor = await _dbContext.UserInfos.FindAsync(Visitor.Id);
                if (existingVisitor != null)
                {
                    existingVisitor.Name = Visitor.Name;
                    existingVisitor.PhoneNumber = Visitor.PhoneNumber;
                    existingVisitor.IdCard = Visitor.IdCard;
                    await _dbContext.SaveChangesAsync();
                    var box = MessageBoxManager.GetMessageBoxStandard("修改", "修改成功", ButtonEnum.Ok);
                    await box.ShowAsync();
                    _onVisitorUpdatedCallback?.Invoke(Visitor);
                    var par = new LayDialogParameter();
                    par.Add("Visitor", Visitor);
                    RequestClose?.Invoke(new LayDialogResult(LayUI.Avalonia.Enums.ButtonResult.OK, par));
                    _regionManager.Regions["DialogRegion"].RemoveAll();
                }
                else
                {
                    var box = MessageBoxManager.GetMessageBoxStandard("错误", "未找到该游客信息，保存失败。", ButtonEnum.Ok);
                    await box.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"保存游客信息失败: {ex.Message}");
                var box = MessageBoxManager.GetMessageBoxStandard("错误", $"保存游客信息失败: {ex.Message}", ButtonEnum.Ok);
                await box.ShowAsync();
            }
        }
        public void OnClosed() { }
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;
        public void OnNavigatedTo(NavigationContext navigationContext) { }
        public void OnNavigatedFrom(NavigationContext navigationContext) { }
    }
}