using LayUI.Avalonia.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Diagnostics;

namespace GetStartedApp.ViewModels
{
        public class VisitorNoticeViewModel : BindableBase, ILayDialogAware
        {
            public event Action<ILayDialogResult> RequestClose;

            private DelegateCommand _CloseCommand;
            public DelegateCommand CloseCommand =>
                _CloseCommand ?? (_CloseCommand = new DelegateCommand(ExecuteCloseCommand));

            void ExecuteCloseCommand()
            {
                RequestClose?.Invoke(null);
            }

            public bool isSingle => false;

            public void OnOpened(ILayDialogParameter parameters)
            {
            }

            public void OnClosed()
            {
            }
        }
    }
