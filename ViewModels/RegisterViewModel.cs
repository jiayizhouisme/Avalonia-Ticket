
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GetStartedApp.ViewModels
{
    public partial class RegisterViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<string> ShowMessage;
        private string _username;
        private string _password;
        private string _confirmPassword;
        public DelegateCommand RegisterCommand { get; private set; }
        public DelegateCommand NavigateToLoginCommand { get; private set; }

        public RegisterViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            this.RegisterCommand = new DelegateCommand(async () => await Register(), () => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password) && IsPasswordsValid);
            this.NavigateToLoginCommand = new DelegateCommand(() => NavigateToLogin());
        }
        private async Task Register()
        {
            try
            {
                Console.WriteLine($"Username: {Username}");
                Console.WriteLine($"Password: {Password}");
                ShowMessage?.Invoke("注册成功");
                _regionManager.RequestNavigate("Nav_MainContent", "LoginPage");
            }
            catch (Exception ex)
            {
                ShowMessage?.Invoke($"注册失败: {ex.Message}");
            }
        }
        private void NavigateToLogin()
        {
            _regionManager.RequestNavigate("Nav_MainContent", "LoginPage");
        }

        private void ValidatePasswords()
        {
            bool isValid = Password == ConfirmPassword;
            if (SetProperty(ref _isPasswordsValid, isValid))
            {
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public string Username
        {
            get => _username;
            set
            {
                SetProperty(ref _username, value);
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                RegisterCommand.RaiseCanExecuteChanged();
                ValidatePasswords();
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                SetProperty(ref _confirmPassword, value);
                RegisterCommand.RaiseCanExecuteChanged();
                ValidatePasswords();
            }
        }

        private bool _isPasswordsValid;
        public bool IsPasswordsValid
        {
            get => _isPasswordsValid;
            private set => SetProperty(ref _isPasswordsValid, value);
        }
    }
}