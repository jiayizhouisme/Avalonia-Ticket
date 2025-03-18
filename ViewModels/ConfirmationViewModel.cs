using LayUI.Avalonia.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.ViewModels
{
    public class ConfirmationViewModel : BindableBase
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private string _button1Text;
        public string Button1Text
        {
            get { return _button1Text; }
            set { SetProperty(ref _button1Text, value); }
        }

        private string _button2Text;
        public string Button2Text
        {
            get { return _button2Text; }
            set { SetProperty(ref _button2Text, value); }
        }

        public DelegateCommand OkCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        public event Action<bool> ConfirmationResult;

        public ConfirmationViewModel()
        {
            OkCommand = new DelegateCommand(OnOk);
            CancelCommand = new DelegateCommand(OnCancel);
        }

        private void OnOk()
        {
            ConfirmationResult?.Invoke(true);
        }

        private void OnCancel()
        {
            ConfirmationResult?.Invoke(false);
        }
    }
}