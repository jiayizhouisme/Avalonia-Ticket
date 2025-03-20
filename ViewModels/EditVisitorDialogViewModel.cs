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
            Visitor = parameters.GetValue<UserInfos>("Visitor");
            
           if(Visitor==null)
            {
                Debug.WriteLine("Failed to receive visitor object.");
                Visitor = new UserInfos();
            }
            _onVisitorUpdatedCallback = parameters.GetValue<Action<UserInfos>>("OnVisitorUpdated");
           
          
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
                    _onVisitorUpdatedCallback?.Invoke(existingVisitor);
                    var par = new LayDialogParameter();
                    par.Add("Visitor", existingVisitor);
                    RequestClose?.Invoke(new LayDialogResult(LayUI.Avalonia.Enums.ButtonResult.OK, par));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"保存游客信息失败: {ex.Message}");
            }
        }
        public void OnClosed() { }
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;
        public void OnNavigatedTo(NavigationContext navigationContext) { }
        public void OnNavigatedFrom(NavigationContext navigationContext) { }
    }
}