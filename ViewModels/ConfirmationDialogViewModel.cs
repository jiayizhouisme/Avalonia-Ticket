using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using LayUI.Avalonia.Enums;
using System;
using LayUI.Avalonia.Global;
using LayUI.Avalonia.Interfaces;
using Prism.Ioc;

namespace GetStartedApp.ViewModels
{
    public class ConfirmationDialogViewModel : ViewModelBase, ILayDialogAware
    {
        public string Title { get; set; } = "提示";
        public string Message { get; set; } = "您已成功预约，请选择后续操作。";
        public string ConfirmText { get; set; } = "去支付";
        public string CancelText { get; set; } = "取消";

        public DelegateCommand ConfirmCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand CloseCommand { get; }

        public bool isSingle => false;

        private readonly Action<bool> _closeCallback;
        private readonly IRegionManager _regionManager;

        public event Action<ILayDialogResult> RequestClose;

        public ConfirmationDialogViewModel(LayDialogParameter parameters, IContainerExtension container) : base(container)
        {
            _closeCallback = parameters.GetValue<Action<bool>>("CloseCallback") ?? ((result) => { });
            _regionManager = container.Resolve<IRegionManager>();
            ConfirmCommand = new DelegateCommand(() =>
            {
                _closeCallback(true);
                RequestClose?.Invoke(new LayDialogResult(ButtonResult.OK));

                try
                {
                    _regionManager.RequestNavigate("Nav_HomeContent", "PaymentPage");
                    System.Diagnostics.Debug.WriteLine("尝试导航到支付页面");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"导航到支付页面出错: {ex.Message}");
                }
            });
            CancelCommand = new DelegateCommand(() =>
            {
                _closeCallback(false);
                RequestClose?.Invoke(new LayDialogResult(ButtonResult.Cancel));
            });
            CloseCommand = new DelegateCommand(() =>
            {
                _closeCallback(false);
                RequestClose?.Invoke(new LayDialogResult(ButtonResult.Cancel));
            });
        }

        public void OnOpened(ILayDialogParameter parameters)
        {
        }

        public void OnClosed()
        {
        }
    }
}