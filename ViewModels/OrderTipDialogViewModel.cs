using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.ViewModels
{
    public class OrderTipDialogViewModel : BindableBase
    {
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
