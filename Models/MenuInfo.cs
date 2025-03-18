using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.ViewModels
{
    /// <summary>
    /// 菜单子项Model
    /// </summary>
    public class MenuInfo : INotifyPropertyChanged
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                   // OnPropertyChanged(nameof(CurrentImage)); 
                    OnPropertyChanged(nameof(TextColor)); 
                }
            }
        }

        public string FontIcon { get; set; }
        public string PageKey { get; set; }
        public string Title { get; set; }
        public string DefaultImage { get; set; }
        public string SelectedImage { get; set; }
       // public string CurrentImage => IsSelected ? SelectedImage : DefaultImage; 
        public string TextColor => IsSelected ? "Orange" : "Gray"; 

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string CurrentImage
        {
            get
            {
                var image = IsSelected ? SelectedImage : DefaultImage;
                Debug.WriteLine($"CurrentImage: {image}");
                return image;
            }
        }
    }
}