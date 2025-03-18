using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Controls
{
    public class UserButton : BindableBase
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public DelegateCommand Command { get; set; }
        private ObservableCollection<UserButton> _userButtons = new ObservableCollection<UserButton>();
        public ObservableCollection<UserButton> UserButtons
        {
            get => _userButtons;
            set => SetProperty(ref _userButtons, value);
        }
    }
}