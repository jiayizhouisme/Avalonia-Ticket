using GetStartedApp.ViewModels;
using LayUI.Avalonia.Interfaces;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GetStartedApp.ViewModels
{
    public class DialogPageViewModel : ViewModelBase
    {
        private ILayDialogService layDialog;

        public DialogPageViewModel(IContainerExtension container) : base(container)
        {
            layDialog = container.Resolve<ILayDialogService>();
        }
        private DelegateCommand<string> _DialogCommand;
        public DelegateCommand<string> DialogCommand =>
            _DialogCommand ?? (_DialogCommand = new DelegateCommand<string>(ExecuteDialogCommand));

        void ExecuteDialogCommand(string messageRootName)
        {
            if (messageRootName == "VisitorNotice")
            {
                layDialog.Show("VisitorNotice", null, res =>
                {
                    var data = res;
                }, messageRootName);
            }
            else
            {
                layDialog.Show("Message", null, res =>
                {
                    var data = res;
                }, messageRootName);
            }
        }
        private DelegateCommand _PrismDlalogCommand;
        public DelegateCommand PrismDlalogCommand =>
            _PrismDlalogCommand ?? (_PrismDlalogCommand = new DelegateCommand(ExecutePrismDlalogCommand));

        void ExecutePrismDlalogCommand()
        {
            layDialog.Show("MessageBox", null, null);
        }
    }
}