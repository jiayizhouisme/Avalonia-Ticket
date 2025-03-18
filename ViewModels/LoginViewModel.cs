using CommunityToolkit.Mvvm.Input;
using GetStartedApp.Views;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using Org.BouncyCastle.Crypto.Generators;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;
namespace GetStartedApp.ViewModels
{
    public partial class LoginViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        public DatabaseContext DbContext { get; set; }
        public DelegateCommand LoginCommand { get; private set; }
        public DelegateCommand NavigateToRegisterCommand { get; private set; }
        public event Action<string> ShowMessage;
        private string _username;
        private string _password;
        public static long CurrentUserId { get; private set; }

        public LoginViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            this.DbContext = new DatabaseContext();
            this.LoginCommand = new DelegateCommand(async () => await Login(), () => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password));
            this.NavigateToRegisterCommand = new DelegateCommand(() => NavigateToRegister());
        }

        private async Task ShowMessageBoxAsync(string title, string message)
        {
            var box = MessageBoxManager.GetMessageBoxStandard(title, message);
            await box.ShowAsync();
        }

        public async Task Login()
        {
            try
            {
                if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
                {
                    await ShowMessageBoxAsync("登录提示", "用户名和密码不能为空");
                    return;
                }

                var user = await DbContext.User.FirstOrDefaultAsync(u => u.username == Username);
                var md5 = MD5.Create();
                byte[] data = System.Text.Encoding.Default.GetBytes(Password);
                byte[] md5data = md5.ComputeHash(data);
                string str = "";
                for (int i = 0; i < md5data.Length - 1; i++)
                {
                    str += md5data[i].ToString("x").PadLeft(2, '0');
                }

                if (user == null || user.password != str)
                {
                    await ShowMessageBoxAsync("登录错误", user == null ? "用户名不存在" : "密码错误");
                    return;
                }
                CurrentUserId = user.id;

                ShowMessage?.Invoke("登录成功");
                _regionManager.RequestNavigate("Nav_MainContent", "HomePage");
            }
            catch (Exception ex)
            {
                ShowMessage?.Invoke($"登录失败: {ex.Message}");
            }
        }

        
    

    private void NavigateToRegister()
        {
            _regionManager.RequestNavigate("Nav_MainContent", "RegisterPage");
        }

        public string Username
        {
            get => _username;
            set
            {
                SetProperty(ref _username, value);
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                LoginCommand.RaiseCanExecuteChanged();
            }
        }
    }
}